using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;
using NpgsqlTypes;

namespace HRMS.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly string _connectionString;

        public OrganizationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> OrganizationInsertAsync(OrganizationModel org)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                        CALL sp_insert_organization(
                        @p_id, @p_name, @p_description, @p_tax_id,
                        @p_phone, @p_email, @p_website, @p_industry, @p_org_type,
                        @p_size, @p_incorporation_date, @p_address_line1, @p_address_line2,
                        @p_city, @p_state, @p_zip_code, @p_country_code, @p_timezone,
                        @p_currency_code, @p_logo_url, @p_status
                        );", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.AddWithValue("p_id", org.Id);
            cmd.Parameters.AddWithValue("p_name", org.Name);
            cmd.Parameters.AddWithValue("p_description", (object)org.Description ?? DBNull.Value);
            //cmd.Parameters.AddWithValue("p_company_id", (object)org.CompanyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_tax_id", (object)org.TaxId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_phone", (object)org.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", (object)org.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_website", (object)org.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_industry", (object)org.Industry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_org_type", (object)org.OrgType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_size", (object)org.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_incorporation_date", (object)org.IncorporationDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_address_line1", (object)org.AddressLine1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_address_line2", (object)org.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_city", (object)org.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_state", (object)org.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_zip_code", (object)org.ZipCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_country_code", (object)org.CountryCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_timezone", (object)org.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_currency_code", (object)org.CurrencyCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_logo_url", (object)org.LogoUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_status", (object)org.Status ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            return org.Id;
        }


        public async Task<OrganizationModel> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Replace BeginTransactionAsync with BeginTransaction as BeginTransactionAsync is not available in the current Npgsql version.  
            var tx = conn.BeginTransaction();

            var callCmd = new NpgsqlCommand(@"
               CALL sp_get_organization_by_id(@p_id, @p_ref);
           ", conn, tx)
            {
                CommandType = CommandType.Text
            };
            callCmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Uuid, id);
            var refParam = new NpgsqlParameter("p_ref", NpgsqlDbType.Refcursor)
            {
                Direction = ParameterDirection.InputOutput,
                Value = "org_cursor"
            };
            callCmd.Parameters.Add(refParam);
            await callCmd.ExecuteNonQueryAsync();

            var fetchCmd = new NpgsqlCommand(@"FETCH ALL IN ""org_cursor"";", conn, tx);
            var reader = await fetchCmd.ExecuteReaderAsync();

            OrganizationModel org = null;
            if (await reader.ReadAsync())
            {
                org = new OrganizationModel
                {
                    Id = reader.GetGuid(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                    TaxId = reader.IsDBNull(reader.GetOrdinal("tax_id")) ? null : reader.GetString(reader.GetOrdinal("tax_id")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                    Website = reader.IsDBNull(reader.GetOrdinal("website")) ? null : reader.GetString(reader.GetOrdinal("website")),
                    Industry = reader.IsDBNull(reader.GetOrdinal("industry")) ? null : reader.GetString(reader.GetOrdinal("industry")),
                    OrgType = reader.IsDBNull(reader.GetOrdinal("org_type")) ? null : reader.GetString(reader.GetOrdinal("org_type")),
                    Size = reader.IsDBNull(reader.GetOrdinal("size")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("size")),
                    IncorporationDate = reader.IsDBNull(reader.GetOrdinal("incorporation_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("incorporation_date")),
                    AddressLine1 = reader.IsDBNull(reader.GetOrdinal("address_line1")) ? null : reader.GetString(reader.GetOrdinal("address_line1")),
                    AddressLine2 = reader.IsDBNull(reader.GetOrdinal("address_line2")) ? null : reader.GetString(reader.GetOrdinal("address_line2")),
                    City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString(reader.GetOrdinal("city")),
                    State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                    ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
                    CountryCode = reader.IsDBNull(reader.GetOrdinal("country_code")) ? null : reader.GetString(reader.GetOrdinal("country_code")),
                    Timezone = reader.IsDBNull(reader.GetOrdinal("timezone")) ? null : reader.GetString(reader.GetOrdinal("timezone")),
                    CurrencyCode = reader.IsDBNull(reader.GetOrdinal("currency_code")) ? null : reader.GetString(reader.GetOrdinal("currency_code")),
                    LogoUrl = reader.IsDBNull(reader.GetOrdinal("logo_url")) ? null : reader.GetString(reader.GetOrdinal("logo_url")),
                    Status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"))
                };
            }

            reader.Close();
            tx.Commit();
            conn.Close();

            return org;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Using CALL with CommandType.Text
            var cmd = new NpgsqlCommand("SELECT fn_delete_organization(@id)", conn);
            cmd.Parameters.AddWithValue("id", id);
            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0); 
            }
            return false;
        }

        public async Task<bool> UpdateAsync(OrganizationModel org)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("sp_update_organization", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("p_id", org.Id);
            cmd.Parameters.AddWithValue("p_name", org.Name);
            cmd.Parameters.AddWithValue("p_description", (object)org.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_tax_id", (object)org.TaxId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_phone", (object)org.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", (object)org.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_website", (object)org.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_industry", (object)org.Industry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_org_type", (object)org.OrgType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_size", (object)org.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_incorporation_date", (object)org.IncorporationDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_address_line1", (object)org.AddressLine1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_address_line2", (object)org.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_city", (object)org.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_state", (object)org.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_zip_code", (object)org.ZipCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_country_code", (object)org.CountryCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_timezone", (object)org.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_currency_code", (object)org.CurrencyCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_logo_url", (object)org.LogoUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_status", (object)org.Status ?? DBNull.Value);

            // OUT parameter
            var outRows = new NpgsqlParameter("o_rows", NpgsqlTypes.NpgsqlDbType.Integer)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outRows);

            await cmd.ExecuteNonQueryAsync();
            return outRows.Value != DBNull.Value && Convert.ToInt32(outRows.Value) > 0;
        }
    }
}