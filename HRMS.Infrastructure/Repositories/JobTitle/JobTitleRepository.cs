using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Departments;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Infrastructure.Interfaces.JobTitle;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.JobTitle
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly string _connectionString;

        public JobTitleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<JobTitleModel> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            try
            {
                using (var tx = conn.BeginTransaction())
                {

                    using (var cmd = new NpgsqlCommand("CALL sp_get_jobtitle_by_id(@id, @ref)", conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value = "jobtitle_ref"
                        });
                        await cmd.ExecuteNonQueryAsync();
                    }
                    JobTitleModel jobTitle = null;

                    var fetchCmd = new NpgsqlCommand("FETCH ALL FROM jobtitle_ref;", conn);
                    using (var reader = await fetchCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            jobTitle = new JobTitleModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                                CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                                Level = reader.IsDBNull(reader.GetOrdinal("level")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("level")),
                                JobCode = reader.IsDBNull(reader.GetOrdinal("job_code")) ? string.Empty : reader.GetString(reader.GetOrdinal("job_code")),
                                PayGradeId = reader.IsDBNull(reader.GetOrdinal("pay_grade_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("pay_grade_id")),
                                DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("department_id")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            };
                        }
                    }
                    await tx.CommitAsync();
                    return jobTitle;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<List<JobTitleModel>> GetJobTitleByCompanyIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_jobtitle_by_company_id(@comId, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("comId", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "jobtitle_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var jobtitles = new List<JobTitleModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM jobtitle_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var jobtitle = new JobTitleModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            Title = reader.GetString(reader.GetOrdinal("title")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                            Level = reader.IsDBNull(reader.GetOrdinal("level")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("level")),
                            JobCode = reader.IsDBNull(reader.GetOrdinal("job_code")) ? string.Empty : reader.GetString(reader.GetOrdinal("job_code")),
                            PayGradeId = reader.IsDBNull(reader.GetOrdinal("pay_grade_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("pay_grade_id")),
                            DepartmentId = reader.IsDBNull(reader.GetOrdinal("department_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("department_id")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        jobtitles.Add(jobtitle);
                    }
                }
                await tx.CommitAsync();
                return jobtitles;
            }
        }
        public async Task CreateJobTitleAsync(JobTitleModel model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            try
            {
                var cmd = new NpgsqlCommand("CALL sp_insert_job_title(@p_id, @p_organization_id, @p_division_id, @p_company_id, @p_title, @p_description, @p_level, @p_job_code, @p_pay_grade_id, @p_department_id, @p_is_active, @p_created_at)", conn);

                cmd.Parameters.AddWithValue("@p_id", model.Id);
                cmd.Parameters.AddWithValue("@p_organization_id", model.OrganizationId);
                cmd.Parameters.AddWithValue("@p_division_id", (object)model.DivisionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_company_id", (object)model.CompanyId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_title", model.Title);
                cmd.Parameters.AddWithValue("@p_description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_level", (object)model.Level ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_job_code", (object)model.JobCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_pay_grade_id", (object)model.PayGradeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_department_id", (object)model.DepartmentId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_is_active", model.IsActive);
                cmd.Parameters.AddWithValue("@p_created_at", (object)model.CreatedAt ?? DateTime.UtcNow);


                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task UpdateJobTitleAsync(JobTitleModel model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            try
            {
                var cmd = new NpgsqlCommand("CALL sp_update_job_title(@p_id, @p_organization_id, @p_division_id, @p_company_id, @p_title, @p_description, @p_level, @p_job_code, @p_pay_grade_id, @p_department_id, @p_is_active, @p_updated_at)", conn);
                cmd.Parameters.AddWithValue("@p_id", model.Id);
                cmd.Parameters.AddWithValue("@p_organization_id", model.OrganizationId);
                cmd.Parameters.AddWithValue("@p_division_id", (object)model.DivisionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_company_id", (object)model.CompanyId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_title", model.Title);
                cmd.Parameters.AddWithValue("@p_description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_level", (object)model.Level ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_job_code", (object)model.JobCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_pay_grade_id", (object)model.PayGradeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_department_id", (object)model.DepartmentId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_is_active", model.IsActive);
                cmd.Parameters.AddWithValue("@p_updated_at", DateTime.UtcNow);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_jobtitle(@id)", conn);
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
