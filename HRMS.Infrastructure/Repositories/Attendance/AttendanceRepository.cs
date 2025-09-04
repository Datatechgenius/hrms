using HRMS.Domain.Entities.Attendance;
using HRMS.Domain.Entities.Company;
using HRMS.Infrastructure.Interfaces.Attendance;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Attendance
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly string _connectionString;

        public AttendanceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateAttendanceAsync(AttendanceModel attendance)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = @"CALL sp_insert_attendance(@p_id, @p_employee_id, @p_organization_id, @p_attendance_date,@p_check_in_time, @p_check_out_time, @p_total_worked_hours,@p_attendance_status, @p_remarks, @p_source,@p_geo_latitude, @p_geo_longitude, @p_approval_status,@p_approved_by, @p_approval_remarks, @p_created_at);";
            using (var command = new NpgsqlCommand(cmd, conn))
            {
                command.Parameters.AddWithValue("p_id", attendance.Id);
                command.Parameters.AddWithValue("p_employee_id", attendance.EmployeeId);
                command.Parameters.AddWithValue("p_organization_id", attendance.OrganizationId);
                command.Parameters.AddWithValue("p_attendance_date", attendance.AttendanceDate);
                command.Parameters.AddWithValue("p_check_in_time", attendance.CheckInTime);
                command.Parameters.AddWithValue("p_check_out_time", attendance.CheckOutTime);
                command.Parameters.AddWithValue("p_total_worked_hours", attendance.TotalWorkedHours);
                command.Parameters.AddWithValue("p_attendance_status", attendance.AttendanceStatus);
                command.Parameters.AddWithValue("p_remarks", attendance.Remarks ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_source", attendance.Source ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_geo_latitude", attendance.GeoLatitude ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_geo_longitude", attendance.GeoLongitude ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_approval_status", attendance.ApprovalStatus ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_approved_by", attendance.ApprovedBy ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_approval_remarks", attendance.ApprovalRemarks ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_created_at", attendance.CreatedAt);
                await command.ExecuteNonQueryAsync();
            }
            await conn.CloseAsync();
        }

        public async Task<AttendanceModel> GetAttendanceByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_attendance_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "attendance_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                AttendanceModel attendance = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM attendance_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        attendance = new AttendanceModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            AttendanceDate = reader.GetDateTime(reader.GetOrdinal("attendance_date")),
                            CheckInTime = reader.IsDBNull(reader.GetOrdinal("check_in_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("check_in_time")),
                            CheckOutTime = reader.IsDBNull(reader.GetOrdinal("check_out_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("check_out_time")),
                            TotalWorkedHours = reader.IsDBNull(reader.GetOrdinal("total_worked_hours")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("total_worked_hours")),
                            AttendanceStatus = reader.GetInt32(reader.GetOrdinal("attendance_status")),
                            Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                            Source = reader.IsDBNull(reader.GetOrdinal("source")) ? null : reader.GetString(reader.GetOrdinal("source")),
                            GeoLatitude = reader.IsDBNull(reader.GetOrdinal("geo_latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("geo_latitude")),
                            GeoLongitude = reader.IsDBNull(reader.GetOrdinal("geo_longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("geo_longitude")),
                            ApprovalStatus = reader.IsDBNull(reader.GetOrdinal("approval_status")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("approval_status")),
                            ApprovedBy = reader.IsDBNull(reader.GetOrdinal("approved_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("approved_by")),
                            ApprovalRemarks = reader.IsDBNull(reader.GetOrdinal("approval_remarks")) ? null : reader.GetString(reader.GetOrdinal("approval_remarks")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return attendance;
            }
        }

        public async Task UpdateAttendanceAsync(AttendanceModel attendance)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = @"CALL sp_update_attendance(@p_id, @p_employee_id, @p_organization_id, @p_attendance_date,@p_check_in_time, @p_check_out_time, @p_total_worked_hours,@p_attendance_status, @p_remarks, @p_source,@p_geo_latitude, @p_geo_longitude, @p_approval_status,@p_approved_by, @p_approval_remarks, @p_updated_at);";
            using (var command = new NpgsqlCommand(cmd, conn))
            {
                command.Parameters.AddWithValue("p_id", attendance.Id);
                command.Parameters.AddWithValue("p_employee_id", attendance.EmployeeId);
                command.Parameters.AddWithValue("p_organization_id", attendance.OrganizationId);
                command.Parameters.AddWithValue("p_attendance_date", attendance.AttendanceDate);
                command.Parameters.AddWithValue("p_check_in_time", attendance.CheckInTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_check_out_time", attendance.CheckOutTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_total_worked_hours", attendance.TotalWorkedHours ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_attendance_status", attendance.AttendanceStatus);
                command.Parameters.AddWithValue("p_remarks", attendance.Remarks ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_source", attendance.Source ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_geo_latitude", attendance.GeoLatitude ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_geo_longitude", attendance.GeoLongitude ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_approval_status", attendance.ApprovalStatus ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_approved_by", attendance.ApprovedBy ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_approval_remarks", attendance.ApprovalRemarks ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_updated_at", DateTime.UtcNow);
                await command.ExecuteNonQueryAsync();
            }
            await conn.CloseAsync();
        }
        public async Task<List<AttendanceModel>> GetAllAttendanceByEmployeeIdAsync(Guid employeeId)
        {
            var allattendance = new List<AttendanceModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


            using (var tx = conn.BeginTransaction())
            {
                // Step 1: Call the stored procedure that opens the refcursor
                using (var cmd = new NpgsqlCommand("CALL sp_get_allattendance_by_empid(@id, @ref)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "allattendance_cursor"
                    });

                    await cmd.ExecuteNonQueryAsync();
                }

                using (var fetch = new NpgsqlCommand("FETCH ALL FROM allattendance_cursor;", conn, tx))
                using (var reader = await fetch.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var attendance = new AttendanceModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            AttendanceDate = reader.GetDateTime(reader.GetOrdinal("attendance_date")),
                            CheckInTime = reader.IsDBNull(reader.GetOrdinal("check_in_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("check_in_time")),
                            CheckOutTime = reader.IsDBNull(reader.GetOrdinal("check_out_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("check_out_time")),
                            TotalWorkedHours = reader.IsDBNull(reader.GetOrdinal("total_worked_hours")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("total_worked_hours")),
                            AttendanceStatus = reader.GetInt32(reader.GetOrdinal("attendance_status")),
                            Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                            Source = reader.IsDBNull(reader.GetOrdinal("source")) ? null : reader.GetString(reader.GetOrdinal("source")),
                            GeoLatitude = reader.IsDBNull(reader.GetOrdinal("geo_latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("geo_latitude")),
                            GeoLongitude = reader.IsDBNull(reader.GetOrdinal("geo_longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("geo_longitude")),
                            ApprovalStatus = reader.IsDBNull(reader.GetOrdinal("approval_status")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("approval_status")),
                            ApprovedBy = reader.IsDBNull(reader.GetOrdinal("approved_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("approved_by")),
                            ApprovalRemarks = reader.IsDBNull(reader.GetOrdinal("approval_remarks")) ? null : reader.GetString(reader.GetOrdinal("approval_remarks")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        allattendance.Add(attendance);
                    }
                }

                await tx.CommitAsync();
            }
            return allattendance;

        }
        public async Task<bool> DeleteAttendanceAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_attendance(@id)", conn);
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
