using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Payroll;
using HRMS.Infrastructure.Interfaces.Payroll;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Payroll
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly string _connectionString;

        public PayrollRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreatePayrollAsync(PayrollModel payroll)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var command = new NpgsqlCommand("CALL sp_insert_payroll(@p_id, @p_organization_id, @p_company_id, @p_payroll_month, @p_payroll_name, @p_status, @p_processed_by, @p_processed_date, @p_total_employees, @p_total_wages, @p_total_deductions, @p_total_net_pay, @p_pay_date, @p_remarks, @p_is_reversal, @p_original_payroll_id, @p_created_at)", conn);

            command.Parameters.AddWithValue("p_id", payroll.Id);
            command.Parameters.AddWithValue("p_organization_id", payroll.OrganizationId);
            command.Parameters.AddWithValue("p_company_id", payroll.CompanyId);
            command.Parameters.AddWithValue("p_payroll_month", payroll.PayrollMonth);
            command.Parameters.AddWithValue("p_payroll_name", payroll.PayrollName);
            command.Parameters.AddWithValue("p_status", payroll.Status);
            command.Parameters.AddWithValue("p_processed_by", payroll.ProcessedBy);
            command.Parameters.AddWithValue("p_processed_date", payroll.ProcessedDate.HasValue ? (object)payroll.ProcessedDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("p_total_employees", payroll.TotalEmployees);
            command.Parameters.AddWithValue("p_total_wages", payroll.TotalWages);
            command.Parameters.AddWithValue("p_total_deductions", payroll.TotalDeductions);
            command.Parameters.AddWithValue("p_total_net_pay", payroll.TotalNetPay);
            command.Parameters.AddWithValue("p_pay_date", payroll.PayDate.HasValue ? (object)payroll.PayDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("p_remarks", payroll.Remarks ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("p_is_reversal", payroll.IsReversal);
            command.Parameters.AddWithValue("p_original_payroll_id", payroll.Id);
            command.Parameters.AddWithValue("p_created_at", payroll.CreatedAt.HasValue ? (object)payroll.CreatedAt.Value : DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<PayrollModel> GetPayrollByIdAsync(Guid id)
        {
            PayrollModel payroll = null;

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_payroll_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payroll_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }


                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM payroll_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        payroll = new PayrollModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            CompanyId = reader.GetGuid(reader.GetOrdinal("company_id")),
                            PayrollMonth = reader.GetDateTime(reader.GetOrdinal("payroll_month")),
                            PayrollName = reader.GetString(reader.GetOrdinal("payroll_name")),
                            Status = ReadNullableEnum<PayrollRunStatusEnum>(reader, "status"),
                            ProcessedBy = reader.IsDBNull(reader.GetOrdinal("processed_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("processed_by")),
                            ProcessedDate = reader.IsDBNull(reader.GetOrdinal("processed_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("processed_date")),
                            TotalEmployees = reader.GetInt32(reader.GetOrdinal("total_employees")),
                            TotalWages = reader.GetDecimal(reader.GetOrdinal("total_wages")),
                            TotalDeductions = reader.GetDecimal(reader.GetOrdinal("total_deductions")),
                            TotalNetPay = reader.GetDecimal(reader.GetOrdinal("total_net_pay")),
                            PayDate = reader.IsDBNull(reader.GetOrdinal("pay_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("pay_date")),
                            Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                            IsReversal = reader.GetBoolean(reader.GetOrdinal("is_reversal")),
                            OriginalPayrollId = reader.IsDBNull(reader.GetOrdinal("original_payroll_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("original_payroll_id")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
                await tx.CommitAsync();
                return payroll;
            }
        }

        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            // Use GetFieldValue directly for Npgsql enum mapping
            return reader.GetFieldValue<TEnum>(idx);
        }

        public async Task UpdatePayrollAsync(PayrollModel payroll)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var command = new NpgsqlCommand("CALL sp_update_payroll(@p_id, @p_organization_id, @p_company_id, @p_payroll_month, @p_payroll_name, @p_status, @p_processed_by, @p_processed_date, @p_total_employees, @p_total_wages, @p_total_deductions, @p_total_net_pay, @p_pay_date, @p_remarks, @p_is_reversal, @p_original_payroll_id, @p_updated_at)", conn);

            command.Parameters.AddWithValue("p_id", payroll.Id);
            command.Parameters.AddWithValue("p_organization_id", payroll.OrganizationId);
            command.Parameters.AddWithValue("p_company_id", payroll.CompanyId);
            command.Parameters.AddWithValue("p_payroll_month", payroll.PayrollMonth);
            command.Parameters.AddWithValue("p_payroll_name", payroll.PayrollName);
            command.Parameters.AddWithValue("p_status", (object)payroll.Status ?? DBNull.Value);
            command.Parameters.AddWithValue("p_processed_by", (object)payroll.ProcessedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("p_processed_date", payroll.ProcessedDate.HasValue ? (object)payroll.ProcessedDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("p_total_employees", payroll.TotalEmployees);
            command.Parameters.AddWithValue("p_total_wages", payroll.TotalWages);
            command.Parameters.AddWithValue("p_total_deductions", payroll.TotalDeductions);
            command.Parameters.AddWithValue("p_total_net_pay", payroll.TotalNetPay);
            command.Parameters.AddWithValue("p_pay_date", payroll.PayDate.HasValue ? (object)payroll.PayDate.Value : DBNull.Value);
            command.Parameters.AddWithValue("p_remarks", payroll.Remarks ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("p_is_reversal", payroll.IsReversal);
            command.Parameters.AddWithValue("p_original_payroll_id", (object)payroll.OriginalPayrollId ?? DBNull.Value);
            command.Parameters.AddWithValue("p_updated_at", payroll.UpdatedAt.HasValue ? (object)payroll.UpdatedAt.Value : DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<PayrollModel>> GetAllPayrollByOrgId(Guid orgId)
        {
            var list = new List<PayrollModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allpayroll_by_orgid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrolls_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM payrolls_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new PayrollModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            CompanyId = reader.GetGuid(reader.GetOrdinal("company_id")),
                            PayrollMonth = reader.GetDateTime(reader.GetOrdinal("payroll_month")),
                            PayrollName = reader.GetString(reader.GetOrdinal("payroll_name")),
                            Status = ReadNullableEnum<PayrollRunStatusEnum>(reader, "status"),
                            ProcessedBy = reader.IsDBNull(reader.GetOrdinal("processed_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("processed_by")),
                            ProcessedDate = reader.IsDBNull(reader.GetOrdinal("processed_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("processed_date")),
                            TotalEmployees = reader.GetInt32(reader.GetOrdinal("total_employees")),
                            TotalWages = reader.GetDecimal(reader.GetOrdinal("total_wages")),
                            TotalDeductions = reader.GetDecimal(reader.GetOrdinal("total_deductions")),
                            TotalNetPay = reader.GetDecimal(reader.GetOrdinal("total_net_pay")),
                            PayDate = reader.IsDBNull(reader.GetOrdinal("pay_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("pay_date")),
                            Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                            IsReversal = reader.GetBoolean(reader.GetOrdinal("is_reversal")),
                            OriginalPayrollId = reader.IsDBNull(reader.GetOrdinal("original_payroll_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("original_payroll_id")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        });
                }
                return list;
            }
        }

        public async Task<List<PayrollModel>> GetAllPayrollByComId(Guid comId)
        {
            var list = new List<PayrollModel>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_allpayroll_by_comid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", comId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payrolls_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM payrolls_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        list.Add(new PayrollModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            CompanyId = reader.GetGuid(reader.GetOrdinal("company_id")),
                            PayrollMonth = reader.GetDateTime(reader.GetOrdinal("payroll_month")),
                            PayrollName = reader.GetString(reader.GetOrdinal("payroll_name")),
                            Status = ReadNullableEnum<PayrollRunStatusEnum>(reader, "status"),
                            ProcessedBy = reader.IsDBNull(reader.GetOrdinal("processed_by")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("processed_by")),
                            ProcessedDate = reader.IsDBNull(reader.GetOrdinal("processed_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("processed_date")),
                            TotalEmployees = reader.GetInt32(reader.GetOrdinal("total_employees")),
                            TotalWages = reader.GetDecimal(reader.GetOrdinal("total_wages")),
                            TotalDeductions = reader.GetDecimal(reader.GetOrdinal("total_deductions")),
                            TotalNetPay = reader.GetDecimal(reader.GetOrdinal("total_net_pay")),
                            PayDate = reader.IsDBNull(reader.GetOrdinal("pay_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("pay_date")),
                            Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                            IsReversal = reader.GetBoolean(reader.GetOrdinal("is_reversal")),
                            OriginalPayrollId = reader.IsDBNull(reader.GetOrdinal("original_payroll_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("original_payroll_id")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        });
                }
                return list;
            }
        }

        public async Task<bool> DeletePayrollAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("SELECT fn_delete_payroll(@id);", conn);
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
