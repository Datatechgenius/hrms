using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Employee
{
    public class EmployeeFamilyMembersRepository : IEmployeeFamilyMembersRepository
    {
        private readonly string _connectionString;

        public EmployeeFamilyMembersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(EmployeeFamilyMembers entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var command = new NpgsqlCommand("CALL sp_insert_employee_family_member(@p_id, @p_employee_id, @p_first_name, @p_last_name, @p_display_name, @p_relation_type, @p_gender, @p_date_of_birth, @p_email, @p_mobile, @p_profession, @p_address_line1, @p_address_line2, @p_city, @p_state, @p_country_code, @p_zip_code, @p_is_dependent, @p_is_emergency, @p_is_nominee, @p_created_at)", conn);

            command.Parameters.AddWithValue("p_id", entity.Id);
            command.Parameters.AddWithValue("p_employee_id", entity.EmployeeId);
            command.Parameters.AddWithValue("p_first_name", entity.FirstName);
            command.Parameters.AddWithValue("p_last_name", entity.LastName);
            command.Parameters.AddWithValue("p_display_name", (object)entity.DisplayName ?? DBNull.Value);
            command.Parameters.AddWithValue("p_relation_type", entity.RelationType);
            command.Parameters.AddWithValue("p_gender", (object)entity.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("p_date_of_birth", (object)entity.DateOfBirth ?? DBNull.Value);
            command.Parameters.AddWithValue("p_email", (object)entity.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("p_mobile", (object)entity.Mobile ?? DBNull.Value);
            command.Parameters.AddWithValue("p_profession", (object)entity.Profession ?? DBNull.Value);
            command.Parameters.AddWithValue("p_address_line1", (object)entity.AddressLine1 ?? DBNull.Value);
            command.Parameters.AddWithValue("p_address_line2", (object)entity.AddressLine2 ?? DBNull.Value);
            command.Parameters.AddWithValue("p_city", (object)entity.City ?? DBNull.Value);
            command.Parameters.AddWithValue("p_state", (object)entity.State ?? DBNull.Value);
            command.Parameters.AddWithValue("p_country_code", (object)entity.CountryCode ?? DBNull.Value);
            command.Parameters.AddWithValue("p_zip_code", (object)entity.ZipCode ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_dependent", (object)entity.IsDependent ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_emergency", (object)entity.IsEmergency ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_nominee", (object)entity.IsNominee ?? DBNull.Value);
            command.Parameters.AddWithValue("p_created_at", entity.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeFamilyMembers> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_familymember_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "family_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                EmployeeFamilyMembers employeefamily = null;

                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM family_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        employeefamily = new EmployeeFamilyMembers
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? null : reader.GetString(reader.GetOrdinal("display_name")),
                            RelationType = reader.GetInt32(reader.GetOrdinal("relation_type")),
                            Gender = ReadNullableEnum<Gender>(reader, "gender"),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            Mobile = reader.IsDBNull(reader.GetOrdinal("mobile")) ? null : reader.GetString(reader.GetOrdinal("mobile")),
                            Profession = reader.IsDBNull(reader.GetOrdinal("profession")) ? null : reader.GetString(reader.GetOrdinal("profession")),
                            AddressLine1 = reader.IsDBNull(reader.GetOrdinal("address_line1")) ? null : reader.GetString(reader.GetOrdinal("address_line1")),
                            AddressLine2 = reader.IsDBNull(reader.GetOrdinal("address_line2")) ? null : reader.GetString(reader.GetOrdinal("address_line2")),
                            City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString(reader.GetOrdinal("city")),
                            State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                            CountryCode = reader.IsDBNull(reader.GetOrdinal("country_code")) ? null : reader.GetString(reader.GetOrdinal("country_code")),
                            ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
                            IsDependent = reader.GetBoolean(reader.GetOrdinal("is_dependent")),
                            IsEmergency = reader.GetBoolean(reader.GetOrdinal("is_emergency")),
                            IsNominee = reader.GetBoolean(reader.GetOrdinal("is_nominee")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        };
                    }
                }
                await tx.CommitAsync();
                return employeefamily;
            }
        }
        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            // Use GetFieldValue directly for Npgsql enum mapping
            return reader.GetFieldValue<TEnum>(idx);
        }

        public async Task<List<EmployeeFamilyMembers>> GetAllByEmployeeIdAsync(Guid employeeId)
        {
            var familieslist = new List<EmployeeFamilyMembers>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allfamilies_by_empid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "families_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM families_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        familieslist.Add(new EmployeeFamilyMembers
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            DisplayName = reader.IsDBNull(reader.GetOrdinal("display_name")) ? null : reader.GetString(reader.GetOrdinal("display_name")),
                            RelationType = reader.GetInt32(reader.GetOrdinal("relation_type")),
                            Gender = ReadNullableEnum<Gender>(reader, "gender"),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            Mobile = reader.IsDBNull(reader.GetOrdinal("mobile")) ? null : reader.GetString(reader.GetOrdinal("mobile")),
                            Profession = reader.IsDBNull(reader.GetOrdinal("profession")) ? null : reader.GetString(reader.GetOrdinal("profession")),
                            AddressLine1 = reader.IsDBNull(reader.GetOrdinal("address_line1")) ? null : reader.GetString(reader.GetOrdinal("address_line1")),
                            AddressLine2 = reader.IsDBNull(reader.GetOrdinal("address_line2")) ? null : reader.GetString(reader.GetOrdinal("address_line2")),
                            City = reader.IsDBNull(reader.GetOrdinal("city")) ? null : reader.GetString(reader.GetOrdinal("city")),
                            State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                            CountryCode = reader.IsDBNull(reader.GetOrdinal("country_code")) ? null : reader.GetString(reader.GetOrdinal("country_code")),
                            ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
                            IsDependent = reader.GetBoolean(reader.GetOrdinal("is_dependent")),
                            IsEmergency = reader.GetBoolean(reader.GetOrdinal("is_emergency")),
                            IsNominee = reader.GetBoolean(reader.GetOrdinal("is_nominee")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))

                        });
                }
                return familieslist;
            }
        }

        public async Task UpdateAsync(EmployeeFamilyMembers entity)
        {

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var command = new NpgsqlCommand("CALL sp_update_employee_family_member(@p_id, @p_employee_id, @p_first_name, @p_last_name, @p_display_name, @p_relation_type, @p_gender, @p_date_of_birth, @p_email, @p_mobile, @p_profession, @p_address_line1, @p_address_line2, @p_city, @p_state, @p_country_code, @p_zip_code, @p_is_dependent, @p_is_emergency, @p_is_nominee, @p_updated_at)", conn);

            command.Parameters.AddWithValue("p_id", entity.Id);
            command.Parameters.AddWithValue("p_employee_id", entity.EmployeeId);
            command.Parameters.AddWithValue("p_first_name", entity.FirstName);
            command.Parameters.AddWithValue("p_last_name", entity.LastName);
            command.Parameters.AddWithValue("p_display_name", (object)entity.DisplayName ?? DBNull.Value);
            command.Parameters.AddWithValue("p_relation_type", entity.RelationType);
            command.Parameters.AddWithValue("p_gender", (object)entity.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("p_date_of_birth", (object)entity.DateOfBirth ?? DBNull.Value);
            command.Parameters.AddWithValue("p_email", (object)entity.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("p_mobile", (object)entity.Mobile ?? DBNull.Value);
            command.Parameters.AddWithValue("p_profession", (object)entity.Profession ?? DBNull.Value);
            command.Parameters.AddWithValue("p_address_line1", (object)entity.AddressLine1 ?? DBNull.Value);
            command.Parameters.AddWithValue("p_address_line2", (object)entity.AddressLine2 ?? DBNull.Value);
            command.Parameters.AddWithValue("p_city", (object)entity.City ?? DBNull.Value);
            command.Parameters.AddWithValue("p_state", (object)entity.State ?? DBNull.Value);
            command.Parameters.AddWithValue("p_country_code", (object)entity.CountryCode ?? DBNull.Value);
            command.Parameters.AddWithValue("p_zip_code", (object)entity.ZipCode ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_dependent", (object)entity.IsDependent ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_emergency", (object)entity.IsEmergency ?? DBNull.Value);
            command.Parameters.AddWithValue("p_is_nominee", (object)entity.IsNominee ?? DBNull.Value);
            command.Parameters.AddWithValue("p_updated_at", entity.UpdatedAt);


            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_familymember(@id)", conn);
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
