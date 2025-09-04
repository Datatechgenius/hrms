using HRMS.Domain.Entities.PayrollDeduction;
using HRMS.Domain.Entities.PayrollWages;
using HRMS.Infrastructure.Interfaces.PayrollDeduction;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.PayrollDeduction
{
    public class PayrollDeductionRepository : IPayrollDeductionRepository
    {
        private readonly string _connectionString;
        public PayrollDeductionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreatePayrollDeductionAsync(PayrollDeductionModel model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_insert_payrolldeduction(@p_id, @p_payrollid, @p_componentname, @p_amount)", conn);

            cmd.Parameters.AddWithValue("p_id", model.Id);
            cmd.Parameters.AddWithValue("p_payrollid", model.PayrollId);
            cmd.Parameters.AddWithValue("p_componentname", model.ComponentName);
            cmd.Parameters.AddWithValue("p_amount", model.Amount);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<PayrollDeductionModel> GetPayrollDeductionByIdAsync(Guid id)
        {
            PayrollDeductionModel payrollDeduction = null;
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_payrolldeduction_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrolldeduction_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                PayrollWagesModel payrollWages = null;
                using (var cmd = new NpgsqlCommand("FETCH ALL IN payrolldeduction_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new PayrollDeductionModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                ComponentName = reader.GetString(reader.GetOrdinal("component_name")),
                                Amount = reader.GetDecimal(reader.GetOrdinal("amount"))
                            };
                        }
                    }
                }
                return payrollDeduction;
            }
        }

        public async Task<List<PayrollDeductionModel>> GetAllPayrollDeductionByPayrollIdAsync(Guid payrollId)
        {
            List<PayrollDeductionModel> payrollDeductions = new List<PayrollDeductionModel>();
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_payrolldeduction_by_payrollid(@payroll_id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("payroll_id", payrollId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrolldeduction_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new NpgsqlCommand("FETCH ALL IN payrolldeduction_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payrollDeductions.Add(new PayrollDeductionModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                ComponentName = reader.GetString(reader.GetOrdinal("component_name")),
                                Amount = reader.GetDecimal(reader.GetOrdinal("amount"))
                            });
                        }
                    }
                }
                return payrollDeductions;
            }
        }

        public async Task UpdatePayrollDeductionAsync(PayrollDeductionModel model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_update_payrolldeduction(@p_id, @p_payrollid, @p_componentname, @p_amount)", conn);
            
            cmd.Parameters.AddWithValue("p_id", model.Id);
            cmd.Parameters.AddWithValue("p_payrollid", model.PayrollId);
            cmd.Parameters.AddWithValue("p_componentname", model.ComponentName);
            cmd.Parameters.AddWithValue("p_amount", model.Amount);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeletePayrollDeductionAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT fn_delete_payrolldeduction(@p_id)", conn);
            cmd.Parameters.AddWithValue("p_id", id);
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0);
            }

            return false;
        }
    }
}
