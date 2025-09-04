using HRMS.Domain.Entities;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.Employee;
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
    public class ProjectAssignmentRepository : IProjectAssignmentRepository
    {
        private readonly string _connectionString;

        public ProjectAssignmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ProjectAssignmentInsertAsync(ProjectAssignmentModel project)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_project_assignment(@id, @project_id, @employee_id, @role, @reporting_manager_id, @allocation_percent, @billing_type, @start_date, @end_date, @status, @notes, @created_at);", conn);

            cmd.Parameters.AddWithValue("id", project.Id);
            cmd.Parameters.AddWithValue("project_id", project.ProjectId);
            cmd.Parameters.AddWithValue("employee_id", project.EmployeeId);
            cmd.Parameters.AddWithValue("role", project.Role);
            cmd.Parameters.AddWithValue("reporting_manager_id", project.ReportingManagerId);
            cmd.Parameters.AddWithValue("allocation_percent", project.AllocationPercent);
            cmd.Parameters.AddWithValue("billing_type", project.BillingType); // must match enum value
            cmd.Parameters.AddWithValue("start_date", project.StartDate);
            cmd.Parameters.AddWithValue("end_date", project.EndDate);
            cmd.Parameters.AddWithValue("status", project.Status);
            cmd.Parameters.AddWithValue("notes", project.Notes);
            cmd.Parameters.AddWithValue("created_at", project.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }
        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            // Use GetFieldValue directly for Npgsql enum mapping
            return reader.GetFieldValue<TEnum>(idx);
        }

        public async Task<ProjectAssignmentModel> GetProjectAssignmentByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_project_assignment_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "project_assignment_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new NpgsqlCommand("FETCH ALL IN project_assignment_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ProjectAssignmentModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                ProjectId = reader.GetGuid(reader.GetOrdinal("project_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Role = reader.IsDBNull(reader.GetOrdinal("role")) ? null : reader.GetString(reader.GetOrdinal("role")),
                                ReportingManagerId = reader.IsDBNull(reader.GetOrdinal("reporting_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("reporting_manager_id")),
                                AllocationPercent = reader.IsDBNull(reader.GetOrdinal("allocation_percent")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("allocation_percent")),
                                BillingType = ReadNullableEnum<TaskBillingTypeEnum>(reader, "billing_type"),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                                EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                                Status = reader.GetInt32(reader.GetOrdinal("status")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        public async Task<List<ProjectAssignmentModel>> GetAllByEmployeeIdAsync(Guid employeeId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL all_assignments_by_employee_id(@employee_id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("employee_id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "project_assignments_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new NpgsqlCommand("FETCH ALL IN project_assignments_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var assignments = new List<ProjectAssignmentModel>();
                        while (await reader.ReadAsync())
                        {
                            assignments.Add(new ProjectAssignmentModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                ProjectId = reader.GetGuid(reader.GetOrdinal("project_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Role = reader.IsDBNull(reader.GetOrdinal("role")) ? null : reader.GetString(reader.GetOrdinal("role")),
                                ReportingManagerId = reader.IsDBNull(reader.GetOrdinal("reporting_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("reporting_manager_id")),
                                AllocationPercent = reader.IsDBNull(reader.GetOrdinal("allocation_percent")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("allocation_percent")),
                                BillingType = ReadNullableEnum<TaskBillingTypeEnum>(reader, "billing_type"),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                                EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                                Status = reader.GetInt32(reader.GetOrdinal("status")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            }
                            );
                        }
                        return assignments;
                    }
                }
            }
        }
        public async Task<bool> UpdateProjectAssignmentAsync(ProjectAssignmentModel project)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("CALL sp_update_project_assignment(@id, @project_id, @employee_id, @role, @reporting_manager_id, @allocation_percent, @billing_type, @start_date, @end_date, @status, @notes, @updated_at);", conn);
            cmd.Parameters.AddWithValue("id", project.Id);
            cmd.Parameters.AddWithValue("project_id", project.ProjectId);
            cmd.Parameters.AddWithValue("employee_id", project.EmployeeId);
            cmd.Parameters.AddWithValue("role", project.Role ?? string.Empty);
            cmd.Parameters.AddWithValue("reporting_manager_id", project.ReportingManagerId.HasValue ? (object)project.ReportingManagerId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("allocation_percent", project.AllocationPercent.HasValue ? (object)project.AllocationPercent.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("billing_type", project.BillingType.HasValue ? (object)project.BillingType.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("start_date", project.StartDate);
            cmd.Parameters.AddWithValue("end_date", project.EndDate.HasValue ? (object)project.EndDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("status", project.Status);
            cmd.Parameters.AddWithValue("notes", project.Notes ?? string.Empty);
            cmd.Parameters.AddWithValue("updated_at", project.UpdatedAt); 
            var result = await cmd.ExecuteNonQueryAsync();
            var outRows = new NpgsqlParameter("o_rows", NpgsqlTypes.NpgsqlDbType.Integer)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outRows);

            await cmd.ExecuteNonQueryAsync();
            return outRows.Value != DBNull.Value && Convert.ToInt32(outRows.Value) > 0;
        }   

        public async Task<bool> DeleteProjectAssignmentAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_project_assignment(@id)", conn);
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
