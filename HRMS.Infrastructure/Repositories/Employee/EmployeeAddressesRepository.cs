using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Employee
{
    public class EmployeeAddressesRepository : IEmployeeAddressesRepository
    {
        private readonly string _connectionString;

        public EmployeeAddressesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task AddEmployeeAddressesAsync(EmployeeAddresses address)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_employee_address(" +
                "@p_id, @p_employee_id, @p_address_type, @p_address_line1, @p_address_line2, @p_landmark, " +
                "@p_city, @p_state, @p_zip_code, @p_country_code, @p_is_primary, " +
                "@p_valid_from, @p_valid_to, @p_phone, @p_mobile, @p_email, @p_created_at);", conn);

            cmd.Parameters.AddWithValue("p_id", address.Id);
            cmd.Parameters.AddWithValue("p_employee_id", address.EmployeeId);
            cmd.Parameters.AddWithValue("p_address_type", address.AddressType);
            cmd.Parameters.AddWithValue("p_address_line1", address.AddressLine1);
            cmd.Parameters.AddWithValue("p_address_line2", (object)address.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_landmark", (object)address.Landmark ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_city", address.City);
            cmd.Parameters.AddWithValue("p_state", address.State);
            cmd.Parameters.AddWithValue("p_zip_code", address.ZipCode);
            cmd.Parameters.AddWithValue("p_country_code", address.CountryCode);
            cmd.Parameters.AddWithValue("p_is_primary", address.IsPrimary);
            cmd.Parameters.AddWithValue("p_valid_from", address.ValidFrom);
            cmd.Parameters.AddWithValue("p_valid_to", address.ValidTo);
            cmd.Parameters.AddWithValue("p_phone", (object)address.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_mobile", (object)address.Mobile ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_email", (object)address.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_created_at", address.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<EmployeeAddresses> GetEmployeeAddressesByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_employee_address_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "address_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                EmployeeAddresses address = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM address_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        address = new EmployeeAddresses
                        {
                            Id = reader.GetGuid(0),
                            EmployeeId = reader.GetGuid(1),
                            AddressType = reader.GetInt32(2),
                            AddressLine1 = reader.GetString(3),
                            AddressLine2 = reader.GetString(4),
                            Landmark = reader.GetString(5),
                            City = reader.GetString(6),
                            State = reader.GetString(7),
                            ZipCode = reader.GetString(8),
                            CountryCode = reader.GetString(9),
                            IsPrimary = reader.GetBoolean(10),
                            ValidFrom = reader.GetDateTime(11),
                            ValidTo = reader.GetDateTime(12),
                            Phone = reader.GetString(13),
                            Mobile = reader.GetString(14),
                            Email = reader.GetString(15),
                            CreatedAt = reader.GetDateTime(16),
                            UpdateAt = reader.GetDateTime(17)
                        };
                    }
                }
                await tx.CommitAsync();
                return address;
            }
        }

        public async Task UpdateEmployeeAddressesAsync(EmployeeAddresses entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "CALL sp_update_employee_addresses(@id, @employeeId, @addressType, @addr1, @addr2, @landmark, @city, @state, @zipCode, @countryCode, @isPrimary, @validFrom, @validTo, @phone, @mobile, @email, @updated)",
                conn);

            cmd.Parameters.AddWithValue("id", entity.Id);
            cmd.Parameters.AddWithValue("employeeId", entity.EmployeeId);
            cmd.Parameters.AddWithValue("addressType", entity.AddressType);
            cmd.Parameters.AddWithValue("addr1", entity.AddressLine1);
            cmd.Parameters.AddWithValue("addr2", entity.AddressLine2 ?? string.Empty);
            cmd.Parameters.AddWithValue("landmark", entity.Landmark ?? string.Empty);
            cmd.Parameters.AddWithValue("city", entity.City);
            cmd.Parameters.AddWithValue("state", entity.State ?? string.Empty);
            cmd.Parameters.AddWithValue("zipCode", entity.ZipCode ?? string.Empty);
            cmd.Parameters.AddWithValue("countryCode", entity.CountryCode);
            cmd.Parameters.AddWithValue("isPrimary", entity.IsPrimary);
            cmd.Parameters.AddWithValue("validFrom", entity.ValidFrom);
            cmd.Parameters.AddWithValue("validTo", entity.ValidTo);
            cmd.Parameters.AddWithValue("phone", entity.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("mobile", entity.Mobile ?? string.Empty);
            cmd.Parameters.AddWithValue("email", entity.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("updated", entity.UpdateAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteEmployeeAddressesAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_employeeaddress(@id)", conn);
            cmd.Parameters.AddWithValue("id", id);

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0);
            }

            return false;
        }

        public async Task<List<EmployeeAddresses>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var list = new List<EmployeeAddresses>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_alladdresses_by_empid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "adresses_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM adresses_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new EmployeeAddresses
                        {
                            Id = reader.GetGuid(0),
                            EmployeeId = reader.GetGuid(1),
                            AddressType = reader.GetInt32(2),
                            AddressLine1 = reader.GetString(3),
                            AddressLine2 = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Landmark = reader.IsDBNull(5) ? null : reader.GetString(5),
                            City = reader.GetString(6),
                            State = reader.IsDBNull(7) ? null : reader.GetString(7),
                            ZipCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                            CountryCode = reader.GetString(9),
                            IsPrimary = reader.GetBoolean(10),
                            ValidFrom = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                            ValidTo = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12),
                            Phone = reader.IsDBNull(13) ? null : reader.GetString(13),
                            Mobile = reader.IsDBNull(14) ? null : reader.GetString(14),
                            Email = reader.IsDBNull(15) ? null : reader.GetString(15),
                            CreatedAt = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16),
                            UpdateAt = reader.IsDBNull(17) ? (DateTime?)null : reader.GetDateTime(17)
                        });
                }
                return list;
            }
        }
    }

}

