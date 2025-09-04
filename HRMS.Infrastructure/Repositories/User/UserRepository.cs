using HRMS.Domain.Entities.Timesheet;
using HRMS.Domain.Entities.User;
using HRMS.Infrastructure.Interfaces.User;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task AddUserAsync(UsersModel user)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_user(@p_id, @p_organization_id, @p_employee_id, @p_username, @p_email, @p_password_hash, @p_is_active, @p_is_locked, @p_last_login_at, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", user.Id);
            cmd.Parameters.AddWithValue("p_organization_id", user.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", (object)user.EmployeeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_username", user.Username);
            cmd.Parameters.AddWithValue("p_email", user.Email);
            cmd.Parameters.AddWithValue("p_password_hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("p_is_active", user.IsActive);
            cmd.Parameters.AddWithValue("p_is_locked", user.IsLocked);
            cmd.Parameters.AddWithValue("p_last_login_at", (object)user.LastLoginAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_created_at", user.CreatedAt);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user to the database.", ex);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public async Task<UsersModel> GetUserByIdAsync(Guid userId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_user_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", userId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "user_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                UsersModel usersmodel = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM user_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usersmodel = new UsersModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            EmployeeId = reader.IsDBNull(reader.GetOrdinal("employee_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("employee_id")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            IsLocked = reader.GetBoolean(reader.GetOrdinal("is_locked")),
                            LastLoginAt = reader.IsDBNull(reader.GetOrdinal("last_login_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("last_login_at")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return usersmodel;
            }

        }
        public async Task<List<UsersModel>> GetAllUsersByOrganizationAsync(Guid orgId)
        {
            var usersList = new List<UsersModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_users_by_orgid(@p_id,@ref)", conn))
                {
                    cmd.Parameters.AddWithValue("p_id", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "users_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM users_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new UsersModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            EmployeeId = reader.IsDBNull(reader.GetOrdinal("employee_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("employee_id")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            IsLocked = reader.GetBoolean(reader.GetOrdinal("is_locked")),
                            LastLoginAt = reader.IsDBNull(reader.GetOrdinal("last_login_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("last_login_at")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        usersList.Add(user);
                    }
                }
                return usersList;
            }
        }
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("SELECT fn_delete_user(@p_id)", conn))
                {
                    cmd.Parameters.AddWithValue("p_id", userId);
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    await tx.CommitAsync();
                    return rowsAffected > 0;
                }
            }
        }
        public async Task UpdateUserAsync(UsersModel user)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var cmd = new NpgsqlCommand("CALL sp_update_user(@p_id, @p_organization_id, @p_employee_id, @p_username, @p_email, @p_password_hash, @p_is_active, @p_is_locked, @p_last_login_at, @p_updated_at)", conn))
            {
                cmd.Parameters.AddWithValue("p_id", user.Id);
                cmd.Parameters.AddWithValue("p_organization_id", user.OrganizationId);
                cmd.Parameters.AddWithValue("p_employee_id", (object)user.EmployeeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("p_username", user.Username);
                cmd.Parameters.AddWithValue("p_email", user.Email);
                cmd.Parameters.AddWithValue("p_password_hash", user.PasswordHash);
                cmd.Parameters.AddWithValue("p_is_active", user.IsActive);
                cmd.Parameters.AddWithValue("p_is_locked", user.IsLocked);
                cmd.Parameters.AddWithValue("p_last_login_at", (object)user.LastLoginAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("p_updated_at", user.UpdatedAt);

                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}