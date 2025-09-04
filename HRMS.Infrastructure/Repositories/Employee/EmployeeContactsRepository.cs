using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.Employee;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeContactsRepository : IEmployeeContactsRepository
    {
        private readonly string _connectionString;

        public EmployeeContactsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task AddEmployeeContactsAsync(EmployeeContacts entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand($"CALL sp_insert_employeecontacts(@p_id, @p_employee_id, @p_contact_type, @p_first_name, @p_last_name, @p_relation, @p_mobile, @p_phone, @p_email, @p_address_line1, @p_address_line2, @p_city, @p_state, @p_country_code, @p_zip_code, @p_is_primary, @p_is_emergency, @p_created_at);", conn);

            cmd.Parameters.AddWithValue("p_id", entity.Id);
            cmd.Parameters.AddWithValue("p_employee_id", entity.EmployeeId);
            cmd.Parameters.AddWithValue("p_contact_type", entity.ContactType);
            cmd.Parameters.AddWithValue("p_first_name", entity.FirstName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_last_name", entity.LastName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_relation", entity.Relation ?? string.Empty);
            cmd.Parameters.AddWithValue("p_mobile", entity.Mobile ?? string.Empty);
            cmd.Parameters.AddWithValue("p_phone", entity.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("p_email", entity.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("p_address_line1", entity.AddressLine1 ?? string.Empty);
            cmd.Parameters.AddWithValue("p_address_line2", entity.AddressLine2 ?? string.Empty);
            cmd.Parameters.AddWithValue("p_city", entity.City ?? string.Empty);
            cmd.Parameters.AddWithValue("p_state", entity.State ?? string.Empty);
            cmd.Parameters.AddWithValue("p_country_code", entity.CountryCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_zip_code", entity.ZipCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_is_primary", entity.IsPrimary);
            cmd.Parameters.AddWithValue("p_is_emergency", entity.IsEmergency);
            cmd.Parameters.AddWithValue("p_created_at", entity.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeContacts> GetContactByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {

                using (var cmd = new NpgsqlCommand("CALL sp_get_employeecontact_by_id(@id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "employeecontact_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                EmployeeContacts contact = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM employeecontact_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        contact = new EmployeeContacts
                        {
                            Id = reader.GetGuid(0),
                            EmployeeId = reader.GetGuid(1),
                            ContactType = reader.GetInt32(2),
                            FirstName = reader.GetString(3),
                            LastName = reader.GetString(4),
                            Relation = reader.GetString(5),
                            Mobile = reader.GetString(6),
                            Phone = reader.GetString(7),
                            Email = reader.GetString(8),
                            AddressLine1 = reader.GetString(9),
                            AddressLine2 = reader.GetString(10),
                            City = reader.GetString(11),
                            State = reader.GetString(12),
                            CountryCode = reader.GetString(13),
                            ZipCode = reader.GetString(14),
                            IsPrimary = reader.GetBoolean(15),
                            IsEmergency = reader.GetBoolean(16),
                            CreatedAt = reader.IsDBNull(17) ? (DateTime?)null : reader.GetDateTime(17),
                            UpdatedAt = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
                        };

                    }
                }
                await tx.CommitAsync();
                return contact;
            }
        }

        public async Task<List<EmployeeContacts>> GetAllContactsByEmployeeIdAsync(Guid employeeId)
        {
            var list = new List<EmployeeContacts>();
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

             using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allcontacts_by_empid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "contacts_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }   
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM contacts_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new EmployeeContacts
                        {
                          Id = reader.GetGuid(0),
                            EmployeeId = reader.GetGuid(1),
                            ContactType = reader.GetInt32(2),
                            FirstName = reader.GetString(3),
                            LastName = reader.GetString(4),
                            Relation = reader.GetString(5),
                            Mobile = reader.GetString(6),
                            Phone = reader.GetString(7),
                            Email = reader.GetString(8),
                            AddressLine1 = reader.GetString(9),
                            AddressLine2 = reader.GetString(10),
                            City = reader.GetString(11),
                            State = reader.GetString(12),
                            CountryCode = reader.GetString(13),
                            ZipCode = reader.GetString(14),
                            IsPrimary = reader.GetBoolean(15),
                            IsEmergency = reader.GetBoolean(16),
                            CreatedAt = reader.IsDBNull(17) ? (DateTime?)null : reader.GetDateTime(17),
                            UpdatedAt = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
                        });
                }
                return list;
            }
        }

        public async Task UpdateEmployeeContactsAsync(EmployeeContacts entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


            var cmd = new NpgsqlCommand("CALL sp_update_employee_contacts(@p_id, @p_employee_id, @p_contact_type, @p_first_name, @p_last_name, @p_relation, @p_mobile, @p_phone, @p_email, @p_address_line1, @p_address_line2, @p_city, @p_state, @p_country_code, @p_zip_code, @p_is_primary, @p_is_emergency, @p_updated_at);", conn);

            cmd.Parameters.AddWithValue("p_id", entity.Id);
            cmd.Parameters.AddWithValue("p_employee_id", entity.EmployeeId);
            cmd.Parameters.AddWithValue("p_contact_type", entity.ContactType);
            cmd.Parameters.AddWithValue("p_first_name", entity.FirstName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_last_name", entity.LastName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_relation", entity.Relation ?? string.Empty);
            cmd.Parameters.AddWithValue("p_mobile", entity.Mobile ?? string.Empty);
            cmd.Parameters.AddWithValue("p_phone", entity.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("p_email", entity.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("p_address_line1", entity.AddressLine1 ?? string.Empty);
            cmd.Parameters.AddWithValue("p_address_line2", entity.AddressLine2 ?? string.Empty);
            cmd.Parameters.AddWithValue("p_city", entity.City ?? string.Empty);
            cmd.Parameters.AddWithValue("p_state", entity.State ?? string.Empty);
            cmd.Parameters.AddWithValue("p_country_code", entity.CountryCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_zip_code", entity.ZipCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_is_primary", entity.IsPrimary);
            cmd.Parameters.AddWithValue("p_is_emergency", entity.IsEmergency);
            cmd.Parameters.AddWithValue("p_updated_at", entity.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteEmployeeContactsAsync(Guid id)
        {
             var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_employeecontact(@id)", conn);
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