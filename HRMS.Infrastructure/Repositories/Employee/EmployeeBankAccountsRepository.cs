using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Employee
{
    public class EmployeeBankAccountsRepository : IEmployeeBankAccountsRepository
    {
        private readonly string _connectionString;

        public EmployeeBankAccountsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task AddEmployeeBankAccountsAsync(EmployeeBankAccounts entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_employee_bank_account(" +
             "@p_id, @p_employee_id, @p_account_type, @p_is_primary, @p_bank_name, @p_account_number, " +
             "@p_account_holder_name, @p_branch_name, @p_ifsc_code, @p_swift_code, @p_routing_number, " +
             "@p_micr_code, @p_country_code, @p_currency_code, @p_effective_from, @p_effective_to, @p_is_verified, @p_notes , @p_created_at);", conn);

            cmd.Parameters.AddWithValue("p_id", entity.Id);
            cmd.Parameters.AddWithValue("p_employee_id", entity.EmployeeId);
            cmd.Parameters.AddWithValue("p_account_type", entity.AccountType);
            cmd.Parameters.AddWithValue("p_is_primary", entity.IsPrimary);
            cmd.Parameters.AddWithValue("p_bank_name", entity.BankName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_account_number", entity.AccountNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("p_account_holder_name", entity.AccountHolderName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_branch_name", entity.BranchName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_ifsc_code", entity.IfscCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_swift_code", entity.SwiftCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_routing_number", entity.RoutingNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("p_micr_code", entity.MicrCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_country_code", entity.CountryCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_currency_code", entity.CurrencyCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_effective_from", entity.EffectiveFrom);
            cmd.Parameters.AddWithValue("p_effective_to", entity.EffectiveTo);
            cmd.Parameters.AddWithValue("p_is_verified", entity.IsVerified);
            cmd.Parameters.AddWithValue("p_notes", entity.Notes ?? string.Empty);
            cmd.Parameters.AddWithValue("p_created_at", entity.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeBankAccounts> GetEmployeeBankAccountsByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_employee_bank_account_by_id(@p_id, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("p_id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlTypes.NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "employee_bank_cursor"
                    });

                    await cmd.ExecuteNonQueryAsync();
                }

                var fetch = new NpgsqlCommand($"FETCH ALL FROM employee_bank_cursor", conn);
                var reader = await fetch.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new EmployeeBankAccounts
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("id")),
                        EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                        AccountType = reader.GetInt32(reader.GetOrdinal("account_type")),
                        IsPrimary = reader.GetBoolean(reader.GetOrdinal("is_primary")),
                        BankName = reader.IsDBNull(reader.GetOrdinal("bank_name")) ? null : reader.GetString(reader.GetOrdinal("bank_name")),
                        AccountNumber = reader.IsDBNull(reader.GetOrdinal("account_number")) ? null : reader.GetString(reader.GetOrdinal("account_number")),
                        AccountHolderName = reader.IsDBNull(reader.GetOrdinal("account_holder_name")) ? null : reader.GetString(reader.GetOrdinal("account_holder_name")),
                        BranchName = reader.IsDBNull(reader.GetOrdinal("branch_name")) ? null : reader.GetString(reader.GetOrdinal("branch_name")),
                        IfscCode = reader.IsDBNull(reader.GetOrdinal("ifsc_code")) ? null : reader.GetString(reader.GetOrdinal("ifsc_code")),
                        SwiftCode = reader.IsDBNull(reader.GetOrdinal("swift_code")) ? null : reader.GetString(reader.GetOrdinal("swift_code")),
                        RoutingNumber = reader.IsDBNull(reader.GetOrdinal("routing_number")) ? null : reader.GetString(reader.GetOrdinal("routing_number")),
                        MicrCode = reader.IsDBNull(reader.GetOrdinal("micr_code")) ? null : reader.GetString(reader.GetOrdinal("micr_code")),
                        CountryCode = reader.IsDBNull(reader.GetOrdinal("country_code")) ? null : reader.GetString(reader.GetOrdinal("country_code")),
                        CurrencyCode = reader.IsDBNull(reader.GetOrdinal("currency_code")) ? null : reader.GetString(reader.GetOrdinal("currency_code")),
                        EffectiveFrom = reader.IsDBNull(reader.GetOrdinal("effective_from")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("effective_from")),
                        EffectiveTo = reader.IsDBNull(reader.GetOrdinal("effective_to")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("effective_to")),
                        IsVerified = reader.GetBoolean(reader.GetOrdinal("is_verified")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                    };
                }

                return null;
            }
        }

        public async Task<List<EmployeeBankAccounts>> GetAllByEmployeeIdAsync(Guid employeeId)
        {
            var list = new List<EmployeeBankAccounts>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allbankaccounts_by_empid(@p_id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("p_id", employeeId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "bankaccounts_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM bankaccounts_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new EmployeeBankAccounts
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                            AccountType = reader.GetInt32(reader.GetOrdinal("account_type")),
                            IsPrimary = reader.GetBoolean(reader.GetOrdinal("is_primary")),
                            BankName = reader.IsDBNull(reader.GetOrdinal("bank_name")) ? null : reader.GetString(reader.GetOrdinal("bank_name")),
                            AccountNumber = reader.IsDBNull(reader.GetOrdinal("account_number")) ? null : reader.GetString(reader.GetOrdinal("account_number")),
                            AccountHolderName = reader.IsDBNull(reader.GetOrdinal("account_holder_name")) ? null : reader.GetString(reader.GetOrdinal("account_holder_name")),
                            BranchName = reader.IsDBNull(reader.GetOrdinal("branch_name")) ? null : reader.GetString(reader.GetOrdinal("branch_name")),
                            IfscCode = reader.IsDBNull(reader.GetOrdinal("ifsc_code")) ? null : reader.GetString(reader.GetOrdinal("ifsc_code")),
                            SwiftCode = reader.IsDBNull(reader.GetOrdinal("swift_code")) ? null : reader.GetString(reader.GetOrdinal("swift_code")),
                            RoutingNumber = reader.IsDBNull(reader.GetOrdinal("routing_number")) ? null : reader.GetString(reader.GetOrdinal("routing_number")),
                            MicrCode = reader.IsDBNull(reader.GetOrdinal("micr_code")) ? null : reader.GetString(reader.GetOrdinal("micr_code")),
                            CountryCode = reader.IsDBNull(reader.GetOrdinal("country_code")) ? null : reader.GetString(reader.GetOrdinal("country_code")),
                            CurrencyCode = reader.IsDBNull(reader.GetOrdinal("currency_code")) ? null : reader.GetString(reader.GetOrdinal("currency_code")),
                            EffectiveFrom = reader.IsDBNull(reader.GetOrdinal("effective_from")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("effective_from")),
                            EffectiveTo = reader.IsDBNull(reader.GetOrdinal("effective_to")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("effective_to")),
                            IsVerified = reader.GetBoolean(reader.GetOrdinal("is_verified")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        });
                }
                return list;
            }
        }

        public async Task UpdateEmployeeBankAccountsAsync(EmployeeBankAccounts entity)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


            var cmd = new NpgsqlCommand($"CALL sp_update_employee_bank_account(@p_id, @p_employee_id, @p_account_type, @p_is_primary, @p_bank_name, @p_account_number, @p_account_holder_name, @p_branch_name, @p_ifsc_code, @p_swift_code, @p_routing_number, @p_micr_code, @p_country_code, @p_currency_code, @p_effective_from, @p_effective_to, @p_is_verified, @p_notes, @p_updated_at);", conn);

            cmd.Parameters.AddWithValue("p_id", entity.Id);
            cmd.Parameters.AddWithValue("p_employee_id", entity.EmployeeId);
            cmd.Parameters.AddWithValue("p_account_type", entity.AccountType);
            cmd.Parameters.AddWithValue("p_is_primary", entity.IsPrimary);
            cmd.Parameters.AddWithValue("p_bank_name", entity.BankName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_account_number", entity.AccountNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("p_account_holder_name", entity.AccountHolderName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_branch_name", entity.BranchName ?? string.Empty);
            cmd.Parameters.AddWithValue("p_ifsc_code", entity.IfscCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_swift_code", entity.SwiftCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_routing_number", entity.RoutingNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("p_micr_code", entity.MicrCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_country_code", entity.CountryCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_currency_code", entity.CurrencyCode ?? string.Empty);
            cmd.Parameters.AddWithValue("p_effective_from", entity.EffectiveFrom);
            cmd.Parameters.AddWithValue("p_effective_to", entity.EffectiveTo);
            cmd.Parameters.AddWithValue("p_is_verified", entity.IsVerified);
            cmd.Parameters.AddWithValue("p_notes", entity.Notes ?? string.Empty);
            cmd.Parameters.AddWithValue("p_updated_at", entity.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteEmployeeBankAccountsAsync(Guid id)
        {
             var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_bankaccount(@id)", conn);
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