using HRMS.Domain.Entities.Projects;
using HRMS.Domain.Entities.Timesheet;
using HRMS.Infrastructure.Interfaces.Timesheet;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Timesheet
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly string _connectionString;

        public TimesheetRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateTimesheetAsync(TimesheetModel timesheet)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_timesheet(@p_id, @p_organization_id, @p_employee_id, @p_project_id, @p_task_title, @p_work_date, @p_hours_worked, @p_work_type, @p_description, @p_approval_status, @p_approved_by, @p_approved_at, @p_rejection_reason, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", timesheet.Id);
            cmd.Parameters.AddWithValue("p_organization_id", timesheet.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", timesheet.EmployeeId);
            cmd.Parameters.AddWithValue("p_project_id", timesheet.ProjectId);
            cmd.Parameters.AddWithValue("p_task_title", timesheet.TaskTitle);
            cmd.Parameters.AddWithValue("p_work_date", timesheet.WorkDate);
            cmd.Parameters.AddWithValue("p_hours_worked", timesheet.HoursWorked);
            cmd.Parameters.AddWithValue("p_work_type", timesheet.WorkType);
            cmd.Parameters.AddWithValue("p_description", timesheet.Description);
            cmd.Parameters.AddWithValue("p_approval_status", (object)timesheet.ApprovalStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_approved_by", timesheet.ApprovedBy.HasValue ? (object)timesheet.ApprovedBy.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_approved_at", timesheet.ApprovedAt.HasValue ? (object)timesheet.ApprovedAt.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_rejection_reason", timesheet.RejectionReason ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_created_at", timesheet.CreatedAt);

            cmd.ExecuteNonQuery();
            await conn.CloseAsync();
        }
        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            // Use GetFieldValue directly for Npgsql enum mapping
            return reader.GetFieldValue<TEnum>(idx);
        }
        public async Task<TimesheetModel> GetTimesheetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_timesheet_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "timesheet_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                TimesheetModel timesheets = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM timesheet_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        timesheets = new TimesheetModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            ProjectId = reader.IsDBNull(reader.GetOrdinal("project_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("project_id")),
                            TaskTitle = reader.GetString(reader.GetOrdinal("task_title")),
                            WorkDate = reader.GetDateTime(reader.GetOrdinal("work_date")),
                            HoursWorked = reader.GetDecimal(reader.GetOrdinal("hours_worked")),
                            WorkType = reader.GetString(reader.GetOrdinal("work_type")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            ApprovalStatus = ReadNullableEnum<TimeEntryStatus>(reader, "approval_status") ?? TimeEntryStatus.unsubmitted,
                            ApprovedBy = reader.IsDBNull(reader.GetOrdinal("approved_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("approved_by")),
                            ApprovedAt = reader.IsDBNull(reader.GetOrdinal("approved_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("approved_at")),
                            RejectionReason = reader.IsDBNull(reader.GetOrdinal("rejection_reason")) ? null : reader.GetString(reader.GetOrdinal("rejection_reason")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return timesheets;
            }

        }
        public async Task<List<TimesheetModel>> GetAllTimesheetsByEmployeeIdAsync(Guid employeeId)
        {
            var list = new List<TimesheetModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_timesheets_by_emp_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "timesheet_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM timesheet_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new TimesheetModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            ProjectId = reader.IsDBNull(reader.GetOrdinal("project_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("project_id")),
                            TaskTitle = reader.GetString(reader.GetOrdinal("task_title")),
                            WorkDate = reader.GetDateTime(reader.GetOrdinal("work_date")),
                            HoursWorked = reader.GetDecimal(reader.GetOrdinal("hours_worked")),
                            WorkType = reader.GetString(reader.GetOrdinal("work_type")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            ApprovalStatus = ReadNullableEnum<TimeEntryStatus>(reader, "approval_status") ?? TimeEntryStatus.unsubmitted,
                            ApprovedBy = reader.IsDBNull(reader.GetOrdinal("approved_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("approved_by")),
                            ApprovedAt = reader.IsDBNull(reader.GetOrdinal("approved_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("approved_at")),
                            RejectionReason = reader.IsDBNull(reader.GetOrdinal("rejection_reason")) ? null : reader.GetString(reader.GetOrdinal("rejection_reason")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        });
                }
                return list;
            }
            
        }
        public async Task UpdateTimesheetAsync(TimesheetModel timesheet)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("CALL sp_update_timesheet(@p_id, @p_organization_id, @p_employee_id, @p_project_id, @p_task_title, @p_work_date, @p_hours_worked, @p_work_type, @p_description, @p_approval_status, @p_approved_by, @p_approved_at, @p_rejection_reason, @p_updated_at)", conn);

            cmd.Parameters.AddWithValue("p_id", timesheet.Id);
            cmd.Parameters.AddWithValue("p_organization_id", timesheet.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", timesheet.EmployeeId);
            cmd.Parameters.AddWithValue("p_project_id", timesheet.ProjectId.HasValue ? (object)timesheet.ProjectId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_task_title", timesheet.TaskTitle);
            cmd.Parameters.AddWithValue("p_work_date", timesheet.WorkDate);
            cmd.Parameters.AddWithValue("p_hours_worked", timesheet.HoursWorked);
            cmd.Parameters.AddWithValue("p_work_type", timesheet.WorkType);
            cmd.Parameters.AddWithValue("p_description", timesheet.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_approval_status", (object)timesheet.ApprovalStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_approved_by", timesheet.ApprovedBy.HasValue ? (object)timesheet.ApprovedBy.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_approved_at", timesheet.ApprovedAt.HasValue ? (object)timesheet.ApprovedAt.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_rejection_reason", timesheet.RejectionReason ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_updated_at", timesheet.UpdatedAt.HasValue ? (object)timesheet.UpdatedAt.Value : DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public async Task<bool> DeleteTimesheetAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_attendance(@id)", conn);
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