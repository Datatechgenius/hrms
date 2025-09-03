using HRMS.Domain.Entities.Projects;
using HRMS.Domain.Entities.RolePermission;
using HRMS.Infrastructure.Interfaces.RolePermission;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.RolePermission
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly string _connectionString;
        public RolePermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateRolePermissionAsync(RolePermissionModel rolePermission)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var command = new NpgsqlCommand("CALL sp_insert_role_permission(@p_id , @p_role_id, @p_module, @p_permission, @p_created_at)", conn);

            command.Parameters.AddWithValue("p_id", rolePermission.Id);
            command.Parameters.AddWithValue("p_role_id", rolePermission.RoleId);
            command.Parameters.AddWithValue("p_module", rolePermission.Module);
            command.Parameters.AddWithValue("p_permission", rolePermission.Permission);
            command.Parameters.AddWithValue("p_created_at", rolePermission.CreatedAt.HasValue ? (object)rolePermission.CreatedAt.Value : DBNull.Value);
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating role permission", ex);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public async Task<RolePermissionModel> GetPermissionsByIdAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty", nameof(Id));
            }
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_permissions_by_role_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", Id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "permission_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                RolePermissionModel permission = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM permission_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        permission = new RolePermissionModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            RoleId = reader.GetGuid(reader.GetOrdinal("role_id")),
                            Module = reader.GetString(reader.GetOrdinal("module")),
                            Permission = reader.GetString(reader.GetOrdinal("permission")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return permission;
            }
        }

        public async Task<List<RolePermissionModel>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_permissions_by_role_id(@role_id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("role_id", roleId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "permission_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                List<RolePermissionModel> permissions = new List<RolePermissionModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM permission_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var permission = new RolePermissionModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            RoleId = reader.GetGuid(reader.GetOrdinal("role_id")),
                            Module = reader.GetString(reader.GetOrdinal("module")),
                            Permission = reader.GetString(reader.GetOrdinal("permission")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        permissions.Add(permission);
                    }
                }
                await tx.CommitAsync();
                return permissions;
            }
        }

        public async Task UpdateRolePermissionAsync(RolePermissionModel rolePermission)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_update_role_permission(@p_id, @p_role_id, @p_module, @p_permission, @p_updated_at)", conn);
            cmd.Parameters.AddWithValue("p_id", rolePermission.Id);
            cmd.Parameters.AddWithValue("p_role_id", rolePermission.RoleId);
            cmd.Parameters.AddWithValue("p_module", rolePermission.Module);
            cmd.Parameters.AddWithValue("p_permission", rolePermission.Permission);
            cmd.Parameters.AddWithValue("p_updated_at", rolePermission.UpdatedAt.HasValue ? (object)rolePermission.UpdatedAt.Value : DBNull.Value);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating role permission", ex);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public async Task<bool> DeleteAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_role_permission(@id)", conn);
            cmd.Parameters.AddWithValue("id", Id);

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0);
            }

            return false;
        }

    }
}
