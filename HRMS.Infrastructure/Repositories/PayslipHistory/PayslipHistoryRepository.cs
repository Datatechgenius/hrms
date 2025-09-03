using HRMS.Domain.Entities;
using HRMS.Domain.Entities.PayrollWages;
using HRMS.Domain.Entities.PayslipHistory;
using HRMS.Infrastructure.Interfaces.PayslipHistory;
using Npgsql;
using Npgsql.Internal.TypeHandlers.LTreeHandlers;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.PayslipHistory
{
    public class PayslipHistoryRepository : IPayslipHistoryRepository
    {
        private readonly string _connectionString;

        public PayslipHistoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreatePayslipHistoryAsync(PaySlipHistoryModel payslipHistory)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_insert_payslip_history(@p_id, @p_organization_id, @p_employee_id, @p_payroll_id, @p_version, @p_pay_period_start, @p_pay_period_end, @p_generated_at, @p_generated_by, @p_payslip_url, @p_delivery_status, @p_delivered_at, @p_remarks, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", payslipHistory.Id);
            cmd.Parameters.AddWithValue("p_organization_id", payslipHistory.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", payslipHistory.EmployeeId);
            cmd.Parameters.AddWithValue("p_payroll_id", payslipHistory.PayrollId);
            cmd.Parameters.AddWithValue("p_version", payslipHistory.Version);
            cmd.Parameters.AddWithValue("p_pay_period_start", payslipHistory.PayPeriodStart);
            cmd.Parameters.AddWithValue("p_pay_period_end", payslipHistory.PayPeriodEnd);
            cmd.Parameters.AddWithValue("p_generated_at", payslipHistory.GeneratedAt);
            cmd.Parameters.AddWithValue("p_generated_by", payslipHistory.GeneratedBy.HasValue ? (object)payslipHistory.GeneratedBy.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_payslip_url", payslipHistory.PayslipUrl);
            cmd.Parameters.AddWithValue("p_delivery_status", payslipHistory.DeliveryStatus);
            cmd.Parameters.AddWithValue("p_delivered_at", payslipHistory.DeliveredAt.HasValue ? (object)payslipHistory.DeliveredAt.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_remarks", payslipHistory.Remarks);
            cmd.Parameters.AddWithValue("p_created_at", payslipHistory.CreatedAt.HasValue ? (object)payslipHistory.CreatedAt.Value : DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        static TEnum? ReadNullableEnum<TEnum>(NpgsqlDataReader reader, string columnName) where TEnum : struct
        {
            var idx = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(idx)) return null;

            return reader.GetFieldValue<TEnum>(idx);
        }

        public async Task<PaySlipHistoryModel> GetPayslipHistoryByIdAsync(Guid guid)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_paysliphistory_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", guid);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "paysliphistory_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                PaySlipHistoryModel paySlipHistoryModel = null;

                using (var cmd = new NpgsqlCommand("FETCH ALL IN paysliphistory_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new PaySlipHistoryModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                Version = reader.GetInt32(reader.GetOrdinal("version")),
                                PayPeriodStart = reader.GetDateTime(reader.GetOrdinal("pay_period_start")),
                                PayPeriodEnd = reader.GetDateTime(reader.GetOrdinal("pay_period_end")),
                                GeneratedAt = reader.GetDateTime(reader.GetOrdinal("generated_at")),
                                GeneratedBy = reader.GetGuid(reader.GetOrdinal("generated_by")),
                                PayslipUrl = reader.GetString(reader.GetOrdinal("payslip_url")),
                                DeliveryStatus = ReadNullableEnum<PaymentStatusEnum>(reader, "delivery_status"),
                                DeliveredAt = reader.IsDBNull(reader.GetOrdinal("delivered_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("delivered_at")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            };
                        }
                    }
                }
                return paySlipHistoryModel;
            }
        }
        public async Task<List<PaySlipHistoryModel>> GetAllPayslipHistoryByEmpIdAsync(Guid empId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_paysliphistory_by_empid(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", empId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "payslip_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var payslipHistory = new List<PaySlipHistoryModel>();
                using (var cmd = new NpgsqlCommand("FETCH ALL IN payslip_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payslipHistory.Add(new PaySlipHistoryModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                PayrollId = reader.GetGuid(reader.GetOrdinal("payroll_id")),
                                Version = reader.GetInt32(reader.GetOrdinal("version")),
                                PayPeriodStart = reader.GetDateTime(reader.GetOrdinal("pay_period_start")),
                                PayPeriodEnd = reader.GetDateTime(reader.GetOrdinal("pay_period_end")),
                                GeneratedAt = reader.GetDateTime(reader.GetOrdinal("generated_at")),
                                GeneratedBy = reader.GetGuid(reader.GetOrdinal("generated_by")),
                                PayslipUrl = reader.GetString(reader.GetOrdinal("payslip_url")),
                                DeliveryStatus = ReadNullableEnum<PaymentStatusEnum>(reader, "delivery_status"),
                                DeliveredAt = reader.IsDBNull(reader.GetOrdinal("delivered_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("delivered_at")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            });
                        }
                    }
                }
                return payslipHistory;
            }
        }

        public async Task UpdatePayslipHistoryAsync(PaySlipHistoryModel payslipHistory)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("CALL sp_update_payslip_history(@p_id, @p_organization_id, @p_employee_id, @p_payroll_id, @p_version, @p_pay_period_start, @p_pay_period_end, @p_generated_at, @p_generated_by, @p_payslip_url, @p_delivery_status, @p_delivered_at, @p_remarks, @p_updated_at)", conn);
            
            cmd.Parameters.AddWithValue("p_id", payslipHistory.Id);
            cmd.Parameters.AddWithValue("p_organization_id", payslipHistory.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", payslipHistory.EmployeeId);
            cmd.Parameters.AddWithValue("p_payroll_id", payslipHistory.PayrollId);
            cmd.Parameters.AddWithValue("p_version", payslipHistory.Version);
            cmd.Parameters.AddWithValue("p_pay_period_start", payslipHistory.PayPeriodStart);
            cmd.Parameters.AddWithValue("p_pay_period_end", payslipHistory.PayPeriodEnd);
            cmd.Parameters.AddWithValue("p_generated_at", payslipHistory.GeneratedAt);
            cmd.Parameters.AddWithValue("p_generated_by", payslipHistory.GeneratedBy.HasValue ? (object)payslipHistory.GeneratedBy.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_payslip_url", payslipHistory.PayslipUrl);
            cmd.Parameters.AddWithValue("p_delivery_status", payslipHistory.DeliveryStatus.HasValue ? (object)payslipHistory.DeliveryStatus.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_delivered_at", payslipHistory.DeliveredAt.HasValue ? (object)payslipHistory.DeliveredAt.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("p_remarks", payslipHistory.Remarks ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("p_updated_at", payslipHistory.UpdatedAt.HasValue ? (object)payslipHistory.UpdatedAt.Value : DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        public async Task<bool> DeletePayslipHistoryAsync(Guid payslipHistoryId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT fn_delete_payslip_history(@p_id);", conn);
            cmd.Parameters.AddWithValue("p_id", payslipHistoryId);
            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetBoolean(0); 
            }

            return false;
        }
    }
}
