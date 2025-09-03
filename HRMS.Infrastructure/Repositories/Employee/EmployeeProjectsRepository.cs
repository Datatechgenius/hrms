using HRMS.Domain.Entities.Employee;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Infrastructure.Interfaces.Employee;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Employee
{
    public class EmployeeProjectsRepository : IEmployeeProjectsRepository
    {
        private readonly string _connectionString;

        public EmployeeProjectsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddEmployeeProjectsAsync(EmployeeProjects entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_employee_project(@id, @employee_id, @project_id, @role, @start_date, @end_date, @is_billable, @allocation_percent, @reporting_manager_id, @status, @created_at);", conn);

            cmd.Parameters.AddWithValue("id", entity.Id);
            cmd.Parameters.AddWithValue("employee_id", entity.EmployeeId);
            cmd.Parameters.AddWithValue("project_id", entity.ProjectId);
            cmd.Parameters.AddWithValue("role", entity.Role);
            cmd.Parameters.AddWithValue("start_date", entity.StartDate);
            cmd.Parameters.AddWithValue("end_date", entity.EndDate);
            cmd.Parameters.AddWithValue("is_billable", entity.IsBillable);
            cmd.Parameters.AddWithValue("allocation_percent", entity.AllocationPercent);
            cmd.Parameters.AddWithValue("reporting_manager_id", entity.ReportingManagerId);
            cmd.Parameters.AddWithValue("status", entity.Status);
            cmd.Parameters.AddWithValue("created_at", entity.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeProjects> GetEmployeeProjectsByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL  sp_get_employee_project_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "projects_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                EmployeeProjects projects = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM projects_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        projects = new EmployeeProjects
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            ProjectId = reader.GetGuid(reader.GetOrdinal("project_id")),
                            Role = reader.IsDBNull(reader.GetOrdinal("role")) ? null : reader.GetString(reader.GetOrdinal("role")),
                            StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                            IsBillable = reader.GetBoolean(reader.GetOrdinal("is_billable")),
                            AllocationPercent = reader.GetInt32(reader.GetOrdinal("allocation_percent")),
                            ReportingManagerId = reader.GetGuid(reader.GetOrdinal("reporting_manager_id")),
                            Status = reader.GetInt32(reader.GetOrdinal("status")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return projects;
            }

        }

        public async Task<List<EmployeeProjects>> GetAllByEmployeeIdAsync(Guid employeeId)
        {
            var list = new List<EmployeeProjects>();
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allprojects_by_empid(@empId, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("empId", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "projects_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM projects_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var porject = new EmployeeProjects
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            ProjectId = reader.GetGuid(reader.GetOrdinal("project_id")),
                            Role = reader.IsDBNull(reader.GetOrdinal("role")) ? null : reader.GetString(reader.GetOrdinal("role")),
                            StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                            IsBillable = reader.GetBoolean(reader.GetOrdinal("is_billable")),
                            AllocationPercent = reader.GetInt32(reader.GetOrdinal("allocation_percent")),
                            ReportingManagerId = reader.GetGuid(reader.GetOrdinal("reporting_manager_id")),
                            Status = reader.GetInt32(reader.GetOrdinal("status")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        list.Add(porject);
                    }
                }
                await tx.CommitAsync();
                return list;
            }
        }

        public async Task UpdateEmployeeProjectsAsync(EmployeeProjects project)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"CALL update_employee_project(@p_id,@p_employee_id,@p_project_id,@p_role,@p_start_date,@p_end_date,@p_is_billable,@p_allocation_percent,@p_reporting_manager_id,@p_status,@p_updated_at);";

            var command = new NpgsqlCommand(sql, conn);
            command.Parameters.AddWithValue("p_id", project.Id);
            command.Parameters.AddWithValue("p_employee_id", project.EmployeeId);
            command.Parameters.AddWithValue("p_project_id", project.ProjectId);
            command.Parameters.AddWithValue("p_role", (object)project.Role ?? DBNull.Value);
            command.Parameters.AddWithValue("p_start_date", (object)project.StartDate ?? DBNull.Value);
            command.Parameters.AddWithValue("p_end_date", (object)project.EndDate ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_billable", project.IsBillable);
            command.Parameters.AddWithValue("p_allocation_percent", project.AllocationPercent);
            command.Parameters.AddWithValue("p_reporting_manager_id", project.ReportingManagerId);
            command.Parameters.AddWithValue("p_status", project.Status);
            command.Parameters.AddWithValue("p_updated_at", (object)project.UpdatedAt ?? DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteEmployeeProjectsAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_employee_projects(@id)", conn);
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
