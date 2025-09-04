using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Contingent;
using HRMS.Infrastructure.Interfaces.Contingent;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Contingent
{
    public class ContingentRepository : IContingentRepository
    {
        private readonly string _connectionString;

        public ContingentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> AddContingentAsync(ContingentDto model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_contingent(@id, @organization_id, @name, @code, @description, @is_billable, @is_active, @created_at);", conn);

            cmd.Parameters.AddWithValue("id", model.Id);
            cmd.Parameters.AddWithValue("organization_id", model.OrganizationId);
            cmd.Parameters.AddWithValue("name", model.Name);
            cmd.Parameters.AddWithValue("code", model.Code);
            cmd.Parameters.AddWithValue("description", model.Description);
            cmd.Parameters.AddWithValue("is_billable", model.isBillable);
            cmd.Parameters.AddWithValue("is_active", model.isActive);
            cmd.Parameters.AddWithValue("created_at", model.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
            return model.Id;
        }

        public async Task<ContingentDto> GetContingentByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_contingent_by_id(@id, @ref)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "contingent_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }

                ContingentDto company = null;

                using (var fetchCmd = new NpgsqlCommand("FETCH ALL FROM contingent_ref;", conn, tx))
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Read nullable updated_at
                        var updatedAtOrdinal = reader.GetOrdinal("updated_at");
                        DateTime? updatedAt = reader.IsDBNull(updatedAtOrdinal)
                            ? (DateTime?)null
                            : reader.GetDateTime(updatedAtOrdinal);

                        company = new ContingentDto
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Code = reader.IsDBNull(reader.GetOrdinal("code"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("code")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("description")),
                            isBillable = reader.GetBoolean(reader.GetOrdinal("is_billable")),
                            isActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = updatedAt
                        };
                    }
                }

                await tx.CommitAsync();
                return company;
            }
        }


        public async Task<List<ContingentDto>> GetAllContingentsByOrgIdAsync(Guid orgId)
        {
            var contingents = new List<ContingentDto>();
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                // Step 1: Call the stored procedure that opens the refcursor
                using (var cmd = new NpgsqlCommand("CALL sp_get_allcontingenttype_by_orgid(@id, @ref)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("id", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "company_cursor"
                    });

                    await cmd.ExecuteNonQueryAsync();
                }

                using (var fetch = new NpgsqlCommand("FETCH ALL FROM company_cursor;", conn, tx))
                using (var reader = await fetch.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var updatedAtOrdinal = reader.GetOrdinal("updated_at");
                        DateTime? updatedAt = reader.IsDBNull(updatedAtOrdinal)
                            ? (DateTime?)null
                            : reader.GetDateTime(updatedAtOrdinal);

                        var company = new ContingentDto
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Code = reader.IsDBNull(reader.GetOrdinal("code"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("code")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("description")),
                            isBillable = reader.GetBoolean(reader.GetOrdinal("is_billable")),
                            isActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = updatedAt
                        };

                        contingents.Add(company);
                    }
                }
                await tx.CommitAsync();
            }
            return contingents;
        }

        public async Task UpdateContingentAsync(ContingentDto model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_update_contingent(@id, @organization_id, @name, @code, @description, @is_billable, @is_active, @updated_at);", conn);
            cmd.Parameters.AddWithValue("id", model.Id);
            cmd.Parameters.AddWithValue("organization_id", model.OrganizationId);
            cmd.Parameters.AddWithValue("name", model.Name);
            cmd.Parameters.AddWithValue("code", model.Code ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("description", model.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("is_billable", model.isBillable);
            cmd.Parameters.AddWithValue("is_active", model.isActive);
            cmd.Parameters.AddWithValue("updated_at", model.UpdatedAt);
           
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteContingentAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_contingent(@id)", conn);
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

