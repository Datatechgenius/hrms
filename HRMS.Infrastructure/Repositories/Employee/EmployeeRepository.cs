using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Company;
using HRMS.Infrastructure.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddEmployeeAsync(EmployeeModel employee)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL public.sp_insert_employee(" +
                "@id, @employee_number, @display_name, @first_name, @middle_name, @last_name, @email, @personal_email," +
                "@mobile_phone, @work_phone, @gender, @date_of_birth, @marital_status, @marriage_date, @joining_date," +
                "@probation_end_date, @employment_status, @account_status, @invitation_status," +
                "@is_profile_complete, @is_private, @organization_id, @division_id, @company_id, @department_id," +
                "@job_title_id, @secondary_job_title_id, @contingent_type_id, @reports_to_id, @l2_manager_id," +
                "@dotted_line_manager_id, @attendance_number, @resignation_submitted_date, @professional_summary," +
                "@created_at);", conn);

            cmd.Parameters.AddWithValue("id", employee.Id);
            cmd.Parameters.AddWithValue("employee_number", employee.EmployeeNumber);
            cmd.Parameters.AddWithValue("display_name", (object)employee.DisplayName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("first_name", employee.FirstName);
            cmd.Parameters.AddWithValue("middle_name", (object)employee.MiddleName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("last_name", employee.LastName);
            cmd.Parameters.AddWithValue("email", employee.Email);
            cmd.Parameters.AddWithValue("personal_email", (object)employee.PersonalEmail ?? DBNull.Value);
            cmd.Parameters.AddWithValue("mobile_phone", (object)employee.MobilePhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("work_phone", (object)employee.WorkPhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("gender", (object)employee.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("date_of_birth", (object)employee.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("marital_status", (object)employee.MaritalStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("marriage_date", (object)employee.MarriageDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("joining_date", (object)employee.JoiningDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("exit_date", (object)employee.ExitDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("probation_end_date", (object)employee.ProbationEndDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("employment_status", employee.EmploymentStatus);
            cmd.Parameters.AddWithValue("account_status", employee.AccountStatus);
            cmd.Parameters.AddWithValue("invitation_status", (object)employee.InvitationStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_profile_complete", employee.IsProfileComplete);
            cmd.Parameters.AddWithValue("is_private", employee.IsPrivate);
            cmd.Parameters.AddWithValue("organization_id", employee.OrganizationId);
            cmd.Parameters.AddWithValue("division_id", (object)employee.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("company_id", (object)employee.CompanyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("department_id", (object)employee.DepartmentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("job_title_id", (object)employee.JobTitleId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("secondary_job_title_id", (object)employee.SecondaryJobTitleId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("contingent_type_id", (object)employee.ContingentTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("reports_to_id", (object)employee.ReportsToId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("l2_manager_id", (object)employee.ManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("dotted_line_manager_id", (object)employee.DottedLineManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("attendance_number", (object)employee.AttendanceNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("resignation_submitted_date", (object)employee.ResignationSubmittedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("professional_summary", (object)employee.ProfessionalSummary ?? DBNull.Value);
            cmd.Parameters.AddWithValue("created_at", employee.CreatedAt);
            //cmd.Parameters.AddWithValue("updated_at", employee.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();

        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_employee_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "employee_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                EmployeeModel employee = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM employee_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        employee = new EmployeeModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("employee_number")),
                            DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? null : reader.GetString(reader.GetOrdinal("display_name")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            MiddleName = reader.IsDBNull(reader.GetOrdinal("middle_name")) ? null : reader.GetString(reader.GetOrdinal("middle_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PersonalEmail = reader.IsDBNull(reader.GetOrdinal("personal_email")) ? null : reader.GetString(reader.GetOrdinal("personal_email")),
                            MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobile_phone")) ? null : reader.GetString(reader.GetOrdinal("mobile_phone")),
                            WorkPhone = reader.IsDBNull(reader.GetOrdinal("work_phone")) ? null : reader.GetString(reader.GetOrdinal("work_phone")),
                            Gender = ReadNullableEnum<Gender>(reader, "gender"),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                            MaritalStatus = ReadNullableEnum<MaritalStatus>(reader, "marital_status"),
                            MarriageDate = reader.IsDBNull(reader.GetOrdinal("marriage_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("marriage_date")),
                            JoiningDate = reader.IsDBNull(reader.GetOrdinal("joining_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("joining_date")),
                            ExitDate = reader.IsDBNull(reader.GetOrdinal("exit_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("exit_date")),
                            ProbationEndDate = reader.IsDBNull(reader.GetOrdinal("probation_end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("probation_end_date")),
                            EmploymentStatus = ReadNullableEnum<EmployeeStatus>(reader, "employment_status"),
                            AccountStatus = ReadNullableEnum<AccountStatus>(reader, "account_status"),
                            InvitationStatus = ReadNullableEnum<InvitationStatus>(reader, "invitation_status"),
                            IsProfileComplete = reader.GetBoolean(reader.GetOrdinal("is_profile_complete")),
                            IsPrivate = reader.GetBoolean(reader.GetOrdinal("is_private")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("department_id")),
                            JobTitleId = reader.IsDBNull(reader.GetOrdinal("job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("job_title_id")),
                            SecondaryJobTitleId = reader.IsDBNull(reader.GetOrdinal("secondary_job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("secondary_job_title_id")),
                            ContingentTypeId = reader.IsDBNull(reader.GetOrdinal("contingent_type_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("contingent_type_id")),
                            ReportsToId = reader.IsDBNull(reader.GetOrdinal("reports_to_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("reports_to_id")),
                            ManagerId = reader.IsDBNull(reader.GetOrdinal("l2_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("l2_manager_id")),
                            DottedLineManagerId = reader.IsDBNull(reader.GetOrdinal("dotted_line_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("dotted_line_manager_id")),
                            AttendanceNumber = reader.IsDBNull(reader.GetOrdinal("attendance_number")) ? null : reader.GetString(reader.GetOrdinal("attendance_number")),
                            ResignationSubmittedDate = reader.IsDBNull(reader.GetOrdinal("resignation_submitted_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resignation_submitted_date")),
                            ProfessionalSummary = reader.IsDBNull(reader.GetOrdinal("professional_summary")) ? null : reader.GetString(reader.GetOrdinal("professional_summary")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return employee;
            }
        }
        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            // Use GetFieldValue directly for Npgsql enum mapping
            return reader.GetFieldValue<TEnum>(idx);
        }

        public async Task UpdateEmployeeAsync(EmployeeModel employee)
        {   
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_update_employee(" +
                "@id, @employee_number, @display_name, @first_name, @middle_name, @last_name, @email, @personal_email," +
                "@mobile_phone, @work_phone, @gender, @date_of_birth, @marital_status, @marriage_date, @joining_date," +
                "@exit_date, @probation_end_date, @employment_status, @account_status, @invitation_status," +
                "@is_profile_complete, @is_private, @organization_id, @division_id, @company_id, @department_id," +
                "@job_title_id, @secondary_job_title_id, @contingent_type_id, @reports_to_id, @l2_manager_id," +
                "@dotted_line_manager_id, @attendance_number, @resignation_submitted_date, @professional_summary," +
                "@updated_at);", conn);

            cmd.Parameters.AddWithValue("id", employee.Id);
            cmd.Parameters.AddWithValue("employee_number", employee.EmployeeNumber);
            cmd.Parameters.AddWithValue("display_name", (object)employee.DisplayName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("first_name", employee.FirstName);
            cmd.Parameters.AddWithValue("middle_name", (object)employee.MiddleName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("last_name", employee.LastName);
            cmd.Parameters.AddWithValue("email", employee.Email);
            cmd.Parameters.AddWithValue("personal_email", (object)employee.PersonalEmail ?? DBNull.Value);
            cmd.Parameters.AddWithValue("mobile_phone", (object)employee.MobilePhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("work_phone", (object)employee.WorkPhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("gender", (object)employee.Gender ?? DBNull.Value);
            cmd.Parameters.AddWithValue("date_of_birth", (object)employee.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("marital_status", (object)employee.MaritalStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("marriage_date", (object)employee.MarriageDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("joining_date", (object)employee.JoiningDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("exit_date", (object)employee.ExitDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("probation_end_date", (object)employee.ProbationEndDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("employment_status", employee.EmploymentStatus);
            cmd.Parameters.AddWithValue("account_status", employee.AccountStatus);
            cmd.Parameters.AddWithValue("invitation_status", (object)employee.InvitationStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_profile_complete", employee.IsProfileComplete);
            cmd.Parameters.AddWithValue("is_private", employee.IsPrivate);
            cmd.Parameters.AddWithValue("organization_id", employee.OrganizationId);
            cmd.Parameters.AddWithValue("division_id", (object)employee.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("company_id", (object)employee.CompanyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("department_id", (object)employee.DepartmentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("job_title_id", (object)employee.JobTitleId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("secondary_job_title_id", (object)employee.SecondaryJobTitleId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("contingent_type_id", (object)employee.ContingentTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("reports_to_id", (object)employee.ReportsToId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("l2_manager_id", (object)employee.ManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("dotted_line_manager_id", (object)employee.DottedLineManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("attendance_number", (object)employee.AttendanceNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("resignation_submitted_date", (object)employee.ResignationSubmittedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("professional_summary", (object)employee.ProfessionalSummary ?? DBNull.Value);
            cmd.Parameters.AddWithValue("updated_at", employee.UpdatedAt);

                await cmd.ExecuteNonQueryAsync();
        }
        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_employee(@id)", conn);
            cmd.Parameters.AddWithValue("id", id);

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0); 
            }

            return false;
        }
        public async Task<List<EmployeeModel>> GetAllEmployeesByOrgAsync(Guid orgId)
        {
            var list = new List<EmployeeModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allemployee_by_orgid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "employees_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM employees_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new EmployeeModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("employee_number")),
                            DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? null : reader.GetString(reader.GetOrdinal("display_name")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            MiddleName = reader.IsDBNull(reader.GetOrdinal("middle_name")) ? null : reader.GetString(reader.GetOrdinal("middle_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PersonalEmail = reader.IsDBNull(reader.GetOrdinal("personal_email")) ? null : reader.GetString(reader.GetOrdinal("personal_email")),
                            MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobile_phone")) ? null : reader.GetString(reader.GetOrdinal("mobile_phone")),
                            WorkPhone = reader.IsDBNull(reader.GetOrdinal("work_phone")) ? null : reader.GetString(reader.GetOrdinal("work_phone")),
                            Gender = ReadNullableEnum<Gender>(reader, "gender"),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                            MaritalStatus = ReadNullableEnum<MaritalStatus>(reader, "marital_status"),
                            MarriageDate = reader.IsDBNull(reader.GetOrdinal("marriage_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("marriage_date")),
                            JoiningDate = reader.IsDBNull(reader.GetOrdinal("joining_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("joining_date")),
                            ExitDate = reader.IsDBNull(reader.GetOrdinal("exit_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("exit_date")),
                            ProbationEndDate = reader.IsDBNull(reader.GetOrdinal("probation_end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("probation_end_date")),
                            EmploymentStatus = ReadNullableEnum<EmployeeStatus>(reader, "employment_status"),
                            AccountStatus = ReadNullableEnum<AccountStatus>(reader, "account_status"),
                            InvitationStatus = ReadNullableEnum<InvitationStatus>(reader, "invitation_status"),
                            IsProfileComplete = reader.GetBoolean(reader.GetOrdinal("is_profile_complete")),
                            IsPrivate = reader.GetBoolean(reader.GetOrdinal("is_private")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("department_id")),
                            JobTitleId = reader.IsDBNull(reader.GetOrdinal("job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("job_title_id")),
                            SecondaryJobTitleId = reader.IsDBNull(reader.GetOrdinal("secondary_job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("secondary_job_title_id")),
                            ContingentTypeId = reader.IsDBNull(reader.GetOrdinal("contingent_type_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("contingent_type_id")),
                            ReportsToId = reader.IsDBNull(reader.GetOrdinal("reports_to_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("reports_to_id")),
                            ManagerId = reader.IsDBNull(reader.GetOrdinal("l2_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("l2_manager_id")),
                            DottedLineManagerId = reader.IsDBNull(reader.GetOrdinal("dotted_line_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("dotted_line_manager_id")),
                            AttendanceNumber = reader.IsDBNull(reader.GetOrdinal("attendance_number")) ? null : reader.GetString(reader.GetOrdinal("attendance_number")),
                            ResignationSubmittedDate = reader.IsDBNull(reader.GetOrdinal("resignation_submitted_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resignation_submitted_date")),
                            ProfessionalSummary = reader.IsDBNull(reader.GetOrdinal("professional_summary")) ? null : reader.GetString(reader.GetOrdinal("professional_summary")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return list;
            }
        }
        public async Task<List<EmployeeModel>> GetAllEmployeesByDivAsync(Guid divId)
        {
            var list = new List<EmployeeModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allemployee_by_divid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", divId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "employees_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM employees_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new EmployeeModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("employee_number")),
                            DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? null : reader.GetString(reader.GetOrdinal("display_name")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            MiddleName = reader.IsDBNull(reader.GetOrdinal("middle_name")) ? null : reader.GetString(reader.GetOrdinal("middle_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PersonalEmail = reader.IsDBNull(reader.GetOrdinal("personal_email")) ? null : reader.GetString(reader.GetOrdinal("personal_email")),
                            MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobile_phone")) ? null : reader.GetString(reader.GetOrdinal("mobile_phone")),
                            WorkPhone = reader.IsDBNull(reader.GetOrdinal("work_phone")) ? null : reader.GetString(reader.GetOrdinal("work_phone")),
                            Gender = ReadNullableEnum<Gender>(reader, "gender"),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                            MaritalStatus = ReadNullableEnum<MaritalStatus>(reader, "marital_status"),
                            MarriageDate = reader.IsDBNull(reader.GetOrdinal("marriage_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("marriage_date")),
                            JoiningDate = reader.IsDBNull(reader.GetOrdinal("joining_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("joining_date")),
                            ExitDate = reader.IsDBNull(reader.GetOrdinal("exit_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("exit_date")),
                            ProbationEndDate = reader.IsDBNull(reader.GetOrdinal("probation_end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("probation_end_date")),
                            EmploymentStatus = ReadNullableEnum<EmployeeStatus>(reader, "employment_status"),
                            AccountStatus = ReadNullableEnum<AccountStatus>(reader, "account_status"),
                            InvitationStatus = ReadNullableEnum<InvitationStatus>(reader, "invitation_status"),
                            IsProfileComplete = reader.GetBoolean(reader.GetOrdinal("is_profile_complete")),
                            IsPrivate = reader.GetBoolean(reader.GetOrdinal("is_private")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("department_id")),
                            JobTitleId = reader.IsDBNull(reader.GetOrdinal("job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("job_title_id")),
                            SecondaryJobTitleId = reader.IsDBNull(reader.GetOrdinal("secondary_job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("secondary_job_title_id")),
                            ContingentTypeId = reader.IsDBNull(reader.GetOrdinal("contingent_type_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("contingent_type_id")),
                            ReportsToId = reader.IsDBNull(reader.GetOrdinal("reports_to_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("reports_to_id")),
                            ManagerId = reader.IsDBNull(reader.GetOrdinal("l2_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("l2_manager_id")),
                            DottedLineManagerId = reader.IsDBNull(reader.GetOrdinal("dotted_line_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("dotted_line_manager_id")),
                            AttendanceNumber = reader.IsDBNull(reader.GetOrdinal("attendance_number")) ? null : reader.GetString(reader.GetOrdinal("attendance_number")),
                            ResignationSubmittedDate = reader.IsDBNull(reader.GetOrdinal("resignation_submitted_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resignation_submitted_date")),
                            ProfessionalSummary = reader.IsDBNull(reader.GetOrdinal("professional_summary")) ? null : reader.GetString(reader.GetOrdinal("professional_summary")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return list;
            }
        }
        public async Task<List<EmployeeModel>> GetAllEmployeesByCompAsync(Guid comId)
        {
            var list = new List<EmployeeModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allemployee_by_comid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", comId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "employees_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM employees_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new EmployeeModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("employee_number")),
                            DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? null : reader.GetString(reader.GetOrdinal("display_name")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            MiddleName = reader.IsDBNull(reader.GetOrdinal("middle_name")) ? null : reader.GetString(reader.GetOrdinal("middle_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PersonalEmail = reader.IsDBNull(reader.GetOrdinal("personal_email")) ? null : reader.GetString(reader.GetOrdinal("personal_email")),
                            MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobile_phone")) ? null : reader.GetString(reader.GetOrdinal("mobile_phone")),
                            WorkPhone = reader.IsDBNull(reader.GetOrdinal("work_phone")) ? null : reader.GetString(reader.GetOrdinal("work_phone")),
                            Gender = ReadNullableEnum<Gender>(reader, "gender"),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                            MaritalStatus = ReadNullableEnum<MaritalStatus>(reader, "marital_status"),
                            MarriageDate = reader.IsDBNull(reader.GetOrdinal("marriage_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("marriage_date")),
                            JoiningDate = reader.IsDBNull(reader.GetOrdinal("joining_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("joining_date")),
                            ExitDate = reader.IsDBNull(reader.GetOrdinal("exit_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("exit_date")),
                            ProbationEndDate = reader.IsDBNull(reader.GetOrdinal("probation_end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("probation_end_date")),
                            EmploymentStatus = ReadNullableEnum<EmployeeStatus>(reader, "employment_status"),
                            AccountStatus = ReadNullableEnum<AccountStatus>(reader, "account_status"),
                            InvitationStatus = ReadNullableEnum<InvitationStatus>(reader, "invitation_status"),
                            IsProfileComplete = reader.GetBoolean(reader.GetOrdinal("is_profile_complete")),
                            IsPrivate = reader.GetBoolean(reader.GetOrdinal("is_private")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("department_id")),
                            JobTitleId = reader.IsDBNull(reader.GetOrdinal("job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("job_title_id")),
                            SecondaryJobTitleId = reader.IsDBNull(reader.GetOrdinal("secondary_job_title_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("secondary_job_title_id")),
                            ContingentTypeId = reader.IsDBNull(reader.GetOrdinal("contingent_type_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("contingent_type_id")),
                            ReportsToId = reader.IsDBNull(reader.GetOrdinal("reports_to_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("reports_to_id")),
                            ManagerId = reader.IsDBNull(reader.GetOrdinal("l2_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("l2_manager_id")),
                            DottedLineManagerId = reader.IsDBNull(reader.GetOrdinal("dotted_line_manager_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("dotted_line_manager_id")),
                            AttendanceNumber = reader.IsDBNull(reader.GetOrdinal("attendance_number")) ? null : reader.GetString(reader.GetOrdinal("attendance_number")),
                            ResignationSubmittedDate = reader.IsDBNull(reader.GetOrdinal("resignation_submitted_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resignation_submitted_date")),
                            ProfessionalSummary = reader.IsDBNull(reader.GetOrdinal("professional_summary")) ? null : reader.GetString(reader.GetOrdinal("professional_summary")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return list;
            }
        }
    }
}