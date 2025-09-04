using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Projects;
using HRMS.Infrastructure.Interfaces.Projects;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateProjectAsync(ProjectsModel project)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_project(@p_id, @p_organization_id, @p_division_id, @p_company_id, @p_name, @p_client_name, @p_description, @p_project_code, @p_start_date, @p_end_date, @p_status, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", project.Id);
            cmd.Parameters.AddWithValue("p_organization_id", project.OrganizationId);
            cmd.Parameters.AddWithValue("p_division_id", (object)project.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_company_id", (object)project.CompanyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_name", project.Name);
            cmd.Parameters.AddWithValue("p_client_name", project.ClientName);
            cmd.Parameters.AddWithValue("p_description", project.Description);
            cmd.Parameters.AddWithValue("p_project_code", project.ProjectCode);
            cmd.Parameters.AddWithValue("p_start_date", (object)project.StartDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_end_date", (object)project.EndDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_status", project.Status);
            cmd.Parameters.AddWithValue("p_created_at", project.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }
        public async Task<ProjectsModel> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_project_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "project_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                ProjectsModel projects = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM project_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        projects = new ProjectsModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            ClientName = reader.GetString(reader.GetOrdinal("client_name")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            ProjectCode = reader.GetString(reader.GetOrdinal("project_code")),
                            StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                            Status = ReadNullableEnum<ProjectStatusEnum>(reader, "status"),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return projects;
            }

        }
        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            // Use GetFieldValue directly for Npgsql enum mapping
            return reader.GetFieldValue<TEnum>(idx);
        }
        public async Task<List<ProjectsModel>> GetAllProjectsByCompanyIdAsync(Guid comId)
        {
            var list = new List<ProjectsModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allproject_by_comid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", comId);
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
                        list.Add(new ProjectsModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            ClientName = reader.GetString(reader.GetOrdinal("client_name")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            ProjectCode = reader.GetString(reader.GetOrdinal("project_code")),
                            StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                            Status = ReadNullableEnum<ProjectStatusEnum>(reader, "status"),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return list;
            }
        }

        public async Task<List<ProjectsModel>> GetAllProjectsByDivisionIdAsync(Guid divId)
        {
            var list = new List<ProjectsModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allproject_by_divid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", divId);
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
                        list.Add(new ProjectsModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            ClientName = reader.GetString(reader.GetOrdinal("client_name")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            ProjectCode = reader.GetString(reader.GetOrdinal("project_code")),
                            StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                            Status = ReadNullableEnum<ProjectStatusEnum>(reader, "status"),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return list;
            }
        }

        public async Task<List<ProjectsModel>> GetAllProjectsByOrganizationIdAsync(Guid orgId)
        {
            var list = new List<ProjectsModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allproject_by_orgid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", orgId);
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
                        list.Add(new ProjectsModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            ClientName = reader.GetString(reader.GetOrdinal("client_name")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            ProjectCode = reader.GetString(reader.GetOrdinal("project_code")),
                            StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("end_date")),
                            Status = ReadNullableEnum<ProjectStatusEnum>(reader, "status"),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return list;
            }
        }

        public async Task UpdateProjectAsync(ProjectsModel project)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("CALL sp_update_project(@p_id, @p_organization_id, @p_division_id, @p_company_id, @p_name, @p_client_name, @p_description, @p_project_code, @p_start_date, @p_end_date, @p_status, @p_updated_at)", conn);
            
            cmd.Parameters.AddWithValue("p_id", project.Id);
            cmd.Parameters.AddWithValue("p_organization_id", project.OrganizationId);
            cmd.Parameters.AddWithValue("p_division_id", (object)project.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_company_id", (object)project.CompanyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_name", project.Name);
            cmd.Parameters.AddWithValue("p_client_name", project.ClientName);
            cmd.Parameters.AddWithValue("p_description", project.Description);
            cmd.Parameters.AddWithValue("p_project_code", project.ProjectCode);
            cmd.Parameters.AddWithValue("p_start_date", (object)project.StartDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_end_date", (object)project.EndDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_status", project.Status);
            cmd.Parameters.AddWithValue("p_updated_at", project.UpdatedAt);

            var reader = await cmd.ExecuteReaderAsync();
        }
        public async Task<bool> DeleteProjectAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_project(@id)", conn);
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
