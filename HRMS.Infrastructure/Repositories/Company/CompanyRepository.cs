using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Company;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Company
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateCompanyAsync(CompanyModel entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_company(@id, @name, @org_id, @country, @currency, @code, @division_id, @legal_name, @tax_id, @duns_number, @incorporation_date, @timezone, @address_line1, @address_line2, @city, @state, @zip_code, @phone, @email, @website, @is_active , @created_at , @updated_at)", conn);

            cmd.Parameters.AddWithValue("id", entity.Id);
            cmd.Parameters.AddWithValue("name", entity.Name);
            cmd.Parameters.AddWithValue("org_id", entity.OrganizationId);
            cmd.Parameters.AddWithValue("country", entity.CountryCode);
            cmd.Parameters.AddWithValue("currency", entity.CurrencyCode);
            cmd.Parameters.AddWithValue("code", (object)entity.Code ?? DBNull.Value);
            cmd.Parameters.AddWithValue("division_id", (object)entity.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("legal_name", (object)entity.LegalName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("tax_id", (object)entity.TaxId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("duns_number", (object)entity.DunsNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("incorporation_date", (object)entity.IncorporationDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("timezone", (object)entity.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("address_line1", (object)entity.AddressLine1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("address_line2", (object)entity.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("city", (object)entity.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("state", (object)entity.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("zip_code", (object)entity.ZipCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("phone", (object)entity.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("email", (object)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("website", (object)entity.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_active", entity.IsActive);
            cmd.Parameters.AddWithValue("created_at", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("updated_at", DateTime.UtcNow);


            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<GetCompanyModel> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                    
                    using(var cmd = new NpgsqlCommand("CALL sp_get_company_by_id(@id, @ref)", conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value     = "company_ref"
                        });
                        await cmd.ExecuteNonQueryAsync();
                    }
                    GetCompanyModel company = null;
           
                    var fetchCmd = new NpgsqlCommand("FETCH ALL FROM company_ref;", conn);
                    using (var reader = await fetchCmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                company = new GetCompanyModel
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                    DivisionId     = reader.GetGuid(reader.GetOrdinal("division_id")),
                                    CountryCode = reader.GetString(reader.GetOrdinal("country_code")),
                                    CurrencyCode = reader.GetString(reader.GetOrdinal("currency_code")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                                    LegalName = reader.IsDBNull(reader.GetOrdinal("legal_name")) ? null : reader.GetString(reader.GetOrdinal("legal_name")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                                };
                            }
                        }
                    await tx.CommitAsync();
                    return company;
            } 
            
        }

        public async Task<List<CompanyModel>> GetCompaniesByOrganizationIdAsync(Guid orgId)
        {
            var companies = new List<CompanyModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using(var tx = conn.BeginTransaction())
            { 
                // Step 1: Call the stored procedure that opens the refcursor
                using (var cmd = new NpgsqlCommand("CALL sp_get_allcompanies_by_orgid(@id, @ref)", conn, tx))
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
                        var company = new CompanyModel
                        {
                            Id             = reader.GetGuid(reader.GetOrdinal("id")),
                            Name           = reader.GetString(reader.GetOrdinal("name")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId     = reader.GetGuid(reader.GetOrdinal("division_id")),
                            CountryCode    = reader.IsDBNull(reader.GetOrdinal("country_code")) 
                                                ? null : reader.GetString(reader.GetOrdinal("country_code")),
                            CurrencyCode   = reader.IsDBNull(reader.GetOrdinal("currency_code")) 
                                                ? null : reader.GetString(reader.GetOrdinal("currency_code")),
                            Email          = reader.IsDBNull(reader.GetOrdinal("email")) 
                                                ? null : reader.GetString(reader.GetOrdinal("email")),
                            LegalName      = reader.IsDBNull(reader.GetOrdinal("legal_name")) 
                                                ? null : reader.GetString(reader.GetOrdinal("legal_name")),
                            IsActive       = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt      = reader.GetDateTime(reader.GetOrdinal("created_at"))
                        };

                        companies.Add(company);
                    }
                }

            await tx.CommitAsync();
            }
            return companies;
        }

        public async Task<List<CompanyModel>> GetCompaniesByDivisionIdAsync(Guid orgId)
        {
            var companies = new List<CompanyModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using(var tx = conn.BeginTransaction())
            { 
                // Step 1: Call the stored procedure that opens the refcursor
                using (var cmd = new NpgsqlCommand("CALL sp_get_allcompanies_by_divid(@id, @ref)", conn, tx))
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
                        var company = new CompanyModel
                        {
                            Id             = reader.GetGuid(reader.GetOrdinal("id")),
                            Name           = reader.GetString(reader.GetOrdinal("name")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId     = reader.GetGuid(reader.GetOrdinal("division_id")),
                            CountryCode    = reader.IsDBNull(reader.GetOrdinal("country_code")) 
                                                ? null : reader.GetString(reader.GetOrdinal("country_code")),
                            CurrencyCode   = reader.IsDBNull(reader.GetOrdinal("currency_code")) 
                                                ? null : reader.GetString(reader.GetOrdinal("currency_code")),
                            Email          = reader.IsDBNull(reader.GetOrdinal("email")) 
                                                ? null : reader.GetString(reader.GetOrdinal("email")),
                            LegalName      = reader.IsDBNull(reader.GetOrdinal("legal_name")) 
                                                ? null : reader.GetString(reader.GetOrdinal("legal_name")),
                            IsActive       = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt      = reader.GetDateTime(reader.GetOrdinal("created_at"))
                        };

                        companies.Add(company);
                    }
                }

            await tx.CommitAsync();
            }
            return companies;
        }

        public async Task UpdateCompanyAsync(CompanyModel entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_update_company(@id, @name, @code, @org_id, @division_id, @legal_name, @tax_id, @duns, @incorp_date, @country, @currency, @timezone, @addr1, @addr2, @city, @state, @zip, @phone, @email, @website, @is_active, @updated)", conn);
            cmd.Parameters.AddWithValue("id", entity.Id);
            cmd.Parameters.AddWithValue("name", entity.Name);
            cmd.Parameters.AddWithValue("code", (object)entity.Code ?? DBNull.Value);
            cmd.Parameters.AddWithValue("org_id", entity.OrganizationId);
            cmd.Parameters.AddWithValue("division_id", (object)entity.DivisionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("legal_name", (object)entity.LegalName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("tax_id", (object)entity.TaxId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("duns", (object)entity.DunsNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("incorp_date", (object)entity.IncorporationDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("country", entity.CountryCode);
            cmd.Parameters.AddWithValue("currency", entity.CurrencyCode);
            cmd.Parameters.AddWithValue("timezone", (object)entity.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("addr1", (object)entity.AddressLine1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("addr2", (object)entity.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("city", (object)entity.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("state", (object)entity.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("zip", (object)entity.ZipCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("phone", (object)entity.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("email", (object)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("website", (object)entity.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_active", entity.IsActive);
            cmd.Parameters.AddWithValue("updated", entity.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_company(@id)", conn);
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

