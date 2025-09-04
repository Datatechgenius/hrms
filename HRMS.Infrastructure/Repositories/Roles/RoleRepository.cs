using HRMS.Domain.Entities.Projects;
using HRMS.Domain.Entities.Roles;
using HRMS.Infrastructure.Interfaces.Roles;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        public RoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateRoleAsync(RoleModel role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role cannot be null");
            }

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_role(@p_id, @p_organization_id, @p_name, @p_description, @p_is_system_role, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", role.Id);
            cmd.Parameters.AddWithValue("p_organization_id", role.OrganizationId);
            cmd.Parameters.AddWithValue("p_name", role.Name);
            cmd.Parameters.AddWithValue("p_description", (object)role.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_is_system_role", role.IsSystemRole);
            cmd.Parameters.AddWithValue("p_created_at", role.CreatedAt);

            cmd.ExecuteNonQuery();
        }

        public async Task<RoleModel> GetRoleByIdAsync(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty", nameof(roleId));
            }
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_role_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", roleId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "role_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                RoleModel role = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM role_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        role = new RoleModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            IsSystemRole = reader.GetBoolean(reader.GetOrdinal("is_system_role")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return role;
            }
        }
        public async Task<List<RoleModel>> GetAllRolesByOrganizationAsync(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
            {
                throw new ArgumentException("Organization ID cannot be empty", nameof(organizationId));
            }
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_roles_by_organization(@organization_id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("organization_id", organizationId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "roles_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var roles = new List<RoleModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM roles_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var role = new RoleModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            IsSystemRole = reader.GetBoolean(reader.GetOrdinal("is_system_role")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        roles.Add(role);
                    }
                }
                await tx.CommitAsync();
                return roles;
            }
        }
        public async Task UpdateRoleAsync(RoleModel role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role cannot be null");
            }
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_update_role(@p_id, @p_organization_id, @p_name, @p_description, @p_is_system_role, @p_updated_at)", conn);
            cmd.Parameters.AddWithValue("p_id", role.Id);
            cmd.Parameters.AddWithValue("p_organization_id", role.OrganizationId);
            cmd.Parameters.AddWithValue("p_name", role.Name);
            cmd.Parameters.AddWithValue("p_description", (object)role.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_is_system_role", role.IsSystemRole);
            cmd.Parameters.AddWithValue("p_updated_at", role.UpdatedAt);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_role(@id)", conn);
            cmd.Parameters.AddWithValue("id", roleId);

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0);
            }

            return false;
        }
    }
}