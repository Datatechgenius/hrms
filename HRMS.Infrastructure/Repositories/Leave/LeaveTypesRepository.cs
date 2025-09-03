using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Leave;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.Employee;
using HRMS.Infrastructure.Interfaces.Leave;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.ProjectAssigment
{
    public class LeaveTypesRepository : ILeaveTypesRepository
    {
        private readonly string _connectionString;

        public LeaveTypesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertAsync(LeaveTypesModel leaveType)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                CALL sp_insert_leave_types(
                    @p_id, @p_org_id, @p_title, @p_code, @p_description,
                    @p_is_paid, @p_carry_forward, @p_max_days_year, @p_max_days_month,
                    @p_requires_approval, @p_is_encashable, @p_gender_restriction, 
                    @p_marital_restriction, @p_start_month, @p_is_active, @p_created_at
                );", conn);

            cmd.Parameters.AddWithValue("p_id", leaveType.Id);
            cmd.Parameters.AddWithValue("p_org_id", leaveType.OrganizationId);
            cmd.Parameters.AddWithValue("p_title", leaveType.Title);
            cmd.Parameters.AddWithValue("p_code", leaveType.Code);
            cmd.Parameters.AddWithValue("p_description", (object)leaveType.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_is_paid", leaveType.IsPaid);
            cmd.Parameters.AddWithValue("p_carry_forward", leaveType.CarryForward);
            cmd.Parameters.AddWithValue("p_max_days_year", leaveType.MaxDaysPerYear);
            cmd.Parameters.AddWithValue("p_max_days_month", leaveType.MaxDaysPerMonth);
            cmd.Parameters.AddWithValue("p_requires_approval", leaveType.RequiresApproval);
            cmd.Parameters.AddWithValue("p_is_encashable", leaveType.IsEncashable);
            cmd.Parameters.AddWithValue("p_gender_restriction", leaveType.GenderRestriction);
            cmd.Parameters.AddWithValue("p_marital_restriction", leaveType.MaritalRestriction);
            cmd.Parameters.AddWithValue("p_start_month", leaveType.StartMonth);
            cmd.Parameters.AddWithValue("p_is_active", leaveType.IsActive);
            cmd.Parameters.AddWithValue("p_created_at", leaveType.CreatedAt);


            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<LeaveTypesModel> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var tx = conn.BeginTransaction();

            var callCmd = new NpgsqlCommand("CALL sp_get_leave_types_by_id(@p_id, @p_ref);", conn, tx);
            callCmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Uuid, id);
            var refParam = new NpgsqlParameter("p_ref", NpgsqlDbType.Refcursor)
            {
                Direction = ParameterDirection.InputOutput,
                Value = "lt_cursor"
            };
            callCmd.Parameters.Add(refParam);

            await callCmd.ExecuteNonQueryAsync();

            var fetchCmd = new NpgsqlCommand(@"FETCH ALL IN lt_cursor;", conn, tx);
            var reader = await fetchCmd.ExecuteReaderAsync();

            LeaveTypesModel model = null;
            if (await reader.ReadAsync())
            {
                model = new LeaveTypesModel
                {
                    Id = reader.GetGuid(reader.GetOrdinal("id")),
                    OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    Code = reader.GetString(reader.GetOrdinal("code")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                    IsPaid = reader.GetBoolean(reader.GetOrdinal("is_paid")),
                    CarryForward = reader.GetBoolean(reader.GetOrdinal("carry_forward")),
                    MaxDaysPerYear = reader.GetInt32(reader.GetOrdinal("max_days_per_year")),
                    MaxDaysPerMonth = reader.GetInt32(reader.GetOrdinal("max_days_per_month")),
                    RequiresApproval = reader.GetBoolean(reader.GetOrdinal("requires_approval")),
                    IsEncashable = reader.GetBoolean(reader.GetOrdinal("is_encashable")),
                    GenderRestriction = reader.GetInt32(reader.GetOrdinal("gender_restriction")),
                    MaritalRestriction = reader.GetInt32(reader.GetOrdinal("marital_restriction")),
                    StartMonth = reader.GetInt32(reader.GetOrdinal("start_month")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at"))? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("created_at")),
                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at"))? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("updated_at")),
                };
            }

            reader.Close();
            await tx.CommitAsync();
            return model;
        }

       public async Task<List<LeaveTypesModel>> GetAllAsync(Guid orgId)
    {
        var list = new List<LeaveTypesModel>();

        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using (var tx = conn.BeginTransaction()) 
        {
            using (var callCmd = new NpgsqlCommand("CALL sp_get_all_leave_types_by_orgid(@p_org_id, @p_ref);", conn))
            {
                callCmd.Parameters.AddWithValue("p_org_id", NpgsqlTypes.NpgsqlDbType.Uuid, orgId);
                callCmd.Parameters.Add(new NpgsqlParameter("p_ref", NpgsqlTypes.NpgsqlDbType.Refcursor)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = "lt_cursor"
                });

                await callCmd.ExecuteNonQueryAsync();
            }

            var fetchCmd = new NpgsqlCommand("FETCH ALL IN lt_cursor;", conn);
            using (var reader = await fetchCmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var model = new LeaveTypesModel
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("id")),
                        OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        Code = reader.GetString(reader.GetOrdinal("code")),
                        Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                        IsPaid = reader.GetBoolean(reader.GetOrdinal("is_paid")),
                        CarryForward = reader.GetBoolean(reader.GetOrdinal("carry_forward")),
                        MaxDaysPerYear = reader.GetInt32(reader.GetOrdinal("max_days_per_year")),
                        MaxDaysPerMonth = reader.GetInt32(reader.GetOrdinal("max_days_per_month")),
                        RequiresApproval = reader.GetBoolean(reader.GetOrdinal("requires_approval")),
                        IsEncashable = reader.GetBoolean(reader.GetOrdinal("is_encashable")),
                        GenderRestriction = reader.GetInt32(reader.GetOrdinal("gender_restriction")),
                        MaritalRestriction = reader.GetInt32(reader.GetOrdinal("marital_restriction")),
                        StartMonth = reader.GetInt32(reader.GetOrdinal("start_month")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at")),
                    };
                    list.Add(model);
                }
            }

            await tx.CommitAsync();
        }

        return list;
    }

        public async Task UpdateAsync(LeaveTypesModel leaveType)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                CALL sp_update_leave_types(
                    @p_id, @p_org_id, @p_title, @p_code, @p_description,
                    @p_is_paid, @p_carry_forward, @p_max_days_year, @p_max_days_month,
                    @p_requires_approval, @p_is_encashable, @p_gender_restriction, 
                    @p_marital_restriction, @p_start_month, @p_is_active, @updated_at
                );", conn);

            cmd.Parameters.AddWithValue("p_id", leaveType.Id);
            cmd.Parameters.AddWithValue("p_org_id", leaveType.OrganizationId);
            cmd.Parameters.AddWithValue("p_title", leaveType.Title);
            cmd.Parameters.AddWithValue("p_code", leaveType.Code);
            cmd.Parameters.AddWithValue("p_description", (object)leaveType.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_is_paid", leaveType.IsPaid);
            cmd.Parameters.AddWithValue("p_carry_forward", leaveType.CarryForward);
            cmd.Parameters.AddWithValue("p_max_days_year", leaveType.MaxDaysPerYear);
            cmd.Parameters.AddWithValue("p_max_days_month", leaveType.MaxDaysPerMonth);
            cmd.Parameters.AddWithValue("p_requires_approval", leaveType.RequiresApproval);
            cmd.Parameters.AddWithValue("p_is_encashable", leaveType.IsEncashable);
            cmd.Parameters.AddWithValue("p_gender_restriction", leaveType.GenderRestriction);
            cmd.Parameters.AddWithValue("p_marital_restriction", leaveType.MaritalRestriction);
            cmd.Parameters.AddWithValue("p_start_month", leaveType.StartMonth);
            cmd.Parameters.AddWithValue("p_is_active", leaveType.IsActive);
            cmd.Parameters.AddWithValue("p_updated_at", leaveType.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_leave_types(@id)", conn);
            cmd.Parameters.AddWithValue("id", id);

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0);
            }
            return false;
        }
    }
}