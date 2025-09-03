using HRMS.Domain.Entities.PayrollWages;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.PayrollWages;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.PayrollWages
{
    public class PayrollWagesRepository : IPayrollWagesRepository
    {
        private readonly string _connectionString;

        public PayrollWagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreatePayrollWagesAsync(PayrollWagesModel payrollWages)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_insert_payroll_wages(@p_id, @p_payroll_id, @p_employee_id, @p_wage_type, @p_wage_amount, @p_hours_worked, @p_rate_per_hour, @p_taxable, @p_remarks, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", payrollWages.Id);
            cmd.Parameters.AddWithValue("p_payroll_id", payrollWages.PayrollId);
            cmd.Parameters.AddWithValue("p_employee_id", payrollWages.EmployeeId);
            cmd.Parameters.AddWithValue("p_wage_type", payrollWages.WageType);
            cmd.Parameters.AddWithValue("p_wage_amount", payrollWages.WageAmount);
            cmd.Parameters.AddWithValue("p_hours_worked", (object)payrollWages.HoursWorked ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_rate_per_hour", (object)payrollWages.RatePerHour ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_taxable", payrollWages.Taxable);
            cmd.Parameters.AddWithValue("p_remarks", payrollWages.Remarks);
            cmd.Parameters.AddWithValue("p_created_at", (object)payrollWages.CreatedAt ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<PayrollWagesModel> GetPayrollWagesByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_payrollwages_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrollwages_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                PayrollWagesModel payrollWages = null;
                using (var cmd = new NpgsqlCommand("FETCH ALL IN payrollwages_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new PayrollWagesModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                WageType = reader.GetString(reader.GetOrdinal("wage_type")),
                                WageAmount = reader.GetDecimal(reader.GetOrdinal("wage_amount")),
                                HoursWorked = reader.IsDBNull(reader.GetOrdinal("hours_worked")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("hours_worked")),
                                RatePerHour = reader.IsDBNull(reader.GetOrdinal("rate_per_hour")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rate_per_hour")),
                                Taxable = reader.GetBoolean(reader.GetOrdinal("taxable")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            };
                        }
                    }
                }
                return payrollWages;
            }
        }

        public async Task<List<PayrollWagesModel>> GetPayrollWagesByPayrollIdAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allpayrollwages_by_payroll_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", Id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrollwages_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var payrollWagesList = new List<PayrollWagesModel>();
                using (var cmd = new NpgsqlCommand("FETCH ALL IN payrollwages_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payrollWagesList.Add(new PayrollWagesModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                WageType = reader.GetString(reader.GetOrdinal("wage_type")),
                                WageAmount = reader.GetDecimal(reader.GetOrdinal("wage_amount")),
                                HoursWorked = reader.IsDBNull(reader.GetOrdinal("hours_worked")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("hours_worked")),
                                RatePerHour = reader.IsDBNull(reader.GetOrdinal("rate_per_hour")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rate_per_hour")),
                                Taxable = reader.GetBoolean(reader.GetOrdinal("taxable")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            });
                        }
                    }
                }
                return payrollWagesList;
            }
        }

        public async Task<List<PayrollWagesModel>> GetPayrollWagesByEmpIdAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allpayrollwages_by_employee_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", Id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrollwages_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var payrollWagesList = new List<PayrollWagesModel>();
                using (var cmd = new NpgsqlCommand("FETCH ALL IN payrollwages_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payrollWagesList.Add(new PayrollWagesModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                WageType = reader.GetString(reader.GetOrdinal("wage_type")),
                                WageAmount = reader.GetDecimal(reader.GetOrdinal("wage_amount")),
                                HoursWorked = reader.IsDBNull(reader.GetOrdinal("hours_worked")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("hours_worked")),
                                RatePerHour = reader.IsDBNull(reader.GetOrdinal("rate_per_hour")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rate_per_hour")),
                                Taxable = reader.GetBoolean(reader.GetOrdinal("taxable")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            });
                        }
                    }
                }
                return payrollWagesList;
            }
        }

        public async Task UpdatePayrollWagesAsync(PayrollWagesModel payrollWages)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_update_payrollwages(@p_id, @p_payroll_id, @p_employee_id, @p_wage_type, @p_wage_amount, @p_hours_worked, @p_rate_per_hour, @p_taxable, @p_remarks, @p_updated_at)", conn);
            
            cmd.Parameters.AddWithValue("p_id", payrollWages.Id);
            cmd.Parameters.AddWithValue("p_payroll_id", payrollWages.PayrollId);
            cmd.Parameters.AddWithValue("p_employee_id", payrollWages.EmployeeId);
            cmd.Parameters.AddWithValue("p_wage_type", payrollWages.WageType);
            cmd.Parameters.AddWithValue("p_wage_amount", payrollWages.WageAmount);
            cmd.Parameters.AddWithValue("p_hours_worked", (object)payrollWages.HoursWorked ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_rate_per_hour", (object)payrollWages.RatePerHour ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_taxable", payrollWages.Taxable);
            cmd.Parameters.AddWithValue("p_remarks", payrollWages.Remarks);
            cmd.Parameters.AddWithValue("p_updated_at", (object)payrollWages.UpdatedAt ?? DBNull.Value);
            
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<bool> DeletePayrollWagesAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT fn_delete_payrollwages(@id)", conn);
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