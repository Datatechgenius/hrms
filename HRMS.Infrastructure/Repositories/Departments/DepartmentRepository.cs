using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Departments;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Departments;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    { 
        private readonly string _connectionString;

        public DepartmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateDepartmentAsync(DepartmentModel dept)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


            var cmd = new NpgsqlCommand("CALL sp_insert_department(@id,@name, @code, @org, @div, @comp, @email, @phone, @active ,@created_at , @updated_at);", conn);


            cmd.Parameters.AddWithValue("@id", dept.Id);
            cmd.Parameters.AddWithValue("@name", dept.Name);
            cmd.Parameters.AddWithValue("@code", (object)dept.Code ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@org", dept.OrganizationId);
            cmd.Parameters.AddWithValue("@div", dept.DivisionId);
            cmd.Parameters.AddWithValue("@comp", dept.CompanyId );
            cmd.Parameters.AddWithValue("@email", (object)dept.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object)dept.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@active", dept.IsActive);
            cmd.Parameters.AddWithValue("created_at", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("updated_at", DateTime.UtcNow);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<DepartmentModel> GetDepartmentByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                    
                using(var cmd = new NpgsqlCommand("CALL sp_get_department_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value     = "dapartmant_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                DepartmentModel company = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM dapartmant_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        company = new DepartmentModel
                        {
                            Id             = reader.GetGuid(reader.GetOrdinal("id")),
                            Name           = reader.GetString(reader.GetOrdinal("name")),
                            Code           = reader.IsDBNull(reader.GetOrdinal("code")) ? null : reader.GetString(reader.GetOrdinal("code")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId     = reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId      = reader.GetGuid(reader.GetOrdinal("company_id")),
                            Email          = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            Phone          = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                            IsActive       = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt      = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt      = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
            await tx.CommitAsync();
            return company;
            } 
        }
        public async Task UpdateDepartmentAsync(DepartmentModel department)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                CALL public.sp_update_department(
                    @id, @name, @code,
                    @organization_id, @division_id, @company_id,
                    @email, @phone, @is_active, @updated_at);", conn);

            cmd.Parameters.AddWithValue("id", department.Id);
            cmd.Parameters.AddWithValue("name", department.Name);
            cmd.Parameters.AddWithValue("code", department.Code);
            cmd.Parameters.AddWithValue("organization_id", department.OrganizationId);
            cmd.Parameters.AddWithValue("division_id", department.DivisionId);
            cmd.Parameters.AddWithValue("company_id", department.CompanyId);
            cmd.Parameters.AddWithValue("email", department.Email);
            cmd.Parameters.AddWithValue("phone", department.Phone);
            cmd.Parameters.AddWithValue("is_active", department.IsActive);
            cmd.Parameters.AddWithValue("updated_at", department.UpdatedAt);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine($"Procedure executed. Rows affected: {rowsAffected}");
        }

        public async Task<List<DepartmentModel>> GetDepartmantsByCompanyIdAsync(Guid orgId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_departments_by_company_id(@orgId, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("orgId", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "departments_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var departments = new List<DepartmentModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM departments_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var department = new DepartmentModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Code = reader.IsDBNull(reader.GetOrdinal("code")) ? null : reader.GetString(reader.GetOrdinal("code")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        departments.Add(department);
                    }
                }
                await tx.CommitAsync();
                return departments;
            }
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_department(@id)", conn);
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
