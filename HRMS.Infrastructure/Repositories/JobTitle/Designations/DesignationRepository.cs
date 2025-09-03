using HRMS.Domain.Entities.Departments;
using HRMS.Domain.Entities.Designations;
using HRMS.Infrastructure.Interfaces.Designations;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Designations
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly string _connectionString;
        public DesignationRepository(string conn) 
        { 
            _connectionString = conn;
        }

        public async Task CreateDesignationAsync(DesignationModel designation)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            try
            {
                var command = new NpgsqlCommand("CALL public.sp_insert_designations(@id, @organization_id, @title, @code, @description, @department_id, @level, @is_billable, @status, @created_at, @updated_at)", conn);

                command.Parameters.AddWithValue("@id", designation.Id);
                command.Parameters.AddWithValue("@organization_id", designation.OrganizationId);
                command.Parameters.AddWithValue("@title", designation.Title);
                command.Parameters.AddWithValue("@code", designation.Code);
                command.Parameters.AddWithValue("@description", designation.Description);
                command.Parameters.AddWithValue("@department_id", designation.DepartmentId);
                command.Parameters.AddWithValue("@level", designation.Level);
                command.Parameters.AddWithValue("@is_billable", designation.IsBillable);
                command.Parameters.AddWithValue("@status", designation.Status);
                command.Parameters.AddWithValue("@created_at", designation.CreatedAt);
                command.Parameters.AddWithValue("@updated_at", designation.UpdatedAt);
                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public async Task<GetDesignationModel> GetDesignationByIdAsync(Guid Id)
        {
             var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                    
                using(var cmd = new NpgsqlCommand("CALL sp_get_designation_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", Id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value     = "dapartmant_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                GetDesignationModel designation = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM dapartmant_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        designation = new GetDesignationModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            Title = reader.GetString(reader.GetOrdinal("title")),
                            Code = reader.IsDBNull(reader.GetOrdinal("code")) ? null : reader.GetString(reader.GetOrdinal("code")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            DepartmentId = reader.GetGuid(reader.GetOrdinal("department_id")),
                            Level = reader.GetInt32(reader.GetOrdinal("level")),
                            IsBillable = reader.GetBoolean(reader.GetOrdinal("is_billable")),
                            Status = reader.GetBoolean(reader.GetOrdinal("status")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
            await tx.CommitAsync();
            return designation;
            } 
        }
        public async Task UpdateDesignationAsync(DesignationModel designation)
        {
          var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                CALL public.sp_update_designation(
                    @id, @organization_id, @title, @code, 
                    @description, @department_id, @level, 
                    @is_billable, @status, @updated_at);", conn);
            cmd.Parameters.AddWithValue("id", designation.Id);
            cmd.Parameters.AddWithValue("organization_id", designation.OrganizationId);
            cmd.Parameters.AddWithValue("title", designation.Title);
            cmd.Parameters.AddWithValue("code", designation.Code);
            cmd.Parameters.AddWithValue("description", designation.Description);
            cmd.Parameters.AddWithValue("department_id", designation.DepartmentId);
            cmd.Parameters.AddWithValue("level", designation.Level);
            cmd.Parameters.AddWithValue("is_billable", designation.IsBillable);
            cmd.Parameters.AddWithValue("status", designation.Status);
            cmd.Parameters.AddWithValue("updated_at", designation.UpdatedAt);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine($"Procedure executed. Rows affected: {rowsAffected}");
        }

        public async Task<List<GetDesignationModel>> GetDesignationByDepartmentIdAsync(Guid orgId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_designations_by_department_id(@orgId, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("orgId", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "designations_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var designations = new List<GetDesignationModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM designations_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var designation = new GetDesignationModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            Title = reader.GetString(reader.GetOrdinal("title")),
                            Code = reader.IsDBNull(reader.GetOrdinal("code")) ? null : reader.GetString(reader.GetOrdinal("code")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            DepartmentId = reader.GetGuid(reader.GetOrdinal("department_id")),
                            Level = reader.GetInt32(reader.GetOrdinal("level")),
                            IsBillable = reader.GetBoolean(reader.GetOrdinal("is_billable")),
                            Status = reader.GetBoolean(reader.GetOrdinal("status")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        designations.Add(designation);
                    }
                }
                await tx.CommitAsync();
                return designations;
            }
        }
        public async Task<bool> DeleteDesignationAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_designation(@id)", conn);
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
