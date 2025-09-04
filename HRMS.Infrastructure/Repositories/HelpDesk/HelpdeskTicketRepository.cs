using HRMS.Domain.Entities.HelpDesk;
using HRMS.Domain.Entities.PayrollDeduction;
using HRMS.Domain.Entities.PayrollWages;
using HRMS.Infrastructure.Interfaces.HelpDesk;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.HelpDesk
{
    public class HelpdeskTicketRepository : IHelpdeskTicketRepository
    {
        private readonly string _connectionString;
        public HelpdeskTicketRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateHelpdeskTicketAsync(HelpdeskTicketModel model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_create_helpdesk_ticket(@p_id, @p_organization_id, @p_employee_id, @p_subject, @p_description, @p_priority, @p_status, @p_assigned_to, @p_resolution, @p_resolution_date, @p_sla_due_date, @p_channel, @p_attachments_url, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", model.Id);
            cmd.Parameters.AddWithValue("p_organization_id", model.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", model.EmployeeId);
            cmd.Parameters.AddWithValue("p_subject", model.Subject);
            cmd.Parameters.AddWithValue("p_description", model.Description);
            cmd.Parameters.AddWithValue("p_priority", model.Priority);
            cmd.Parameters.AddWithValue("p_status", model.Status);
            cmd.Parameters.AddWithValue("p_assigned_to", (object)model.AssignedTo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_resolution", (object)model.Resolution ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_resolution_date", (object)model.ResolutionDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_sla_due_date", (object)model.SlaDueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_channel", (object)model.Channel ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_attachments_url", (object)model.AttachmentsUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_created_at", model.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }

        public async Task<HelpdeskTicketModel> GetHelpdeskTicketByIdAsync(Guid id)
        {
            HelpdeskTicketModel ticketModel = null;

            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_ticket_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "helpdeskticket_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }

                using (var cmd = new NpgsqlCommand("FETCH ALL IN helpdeskticket_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ticketModel = new HelpdeskTicketModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Subject = reader.GetString(reader.GetOrdinal("subject")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                Priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                Status = reader.GetInt32(reader.GetOrdinal("status")),
                                AssignedTo = reader.IsDBNull(reader.GetOrdinal("assigned_to")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("assigned_to")),
                                Resolution = reader.IsDBNull(reader.GetOrdinal("resolution")) ? null : reader.GetString(reader.GetOrdinal("resolution")),
                                ResolutionDate = reader.IsDBNull(reader.GetOrdinal("resolution_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resolution_date")),
                                SlaDueDate = reader.IsDBNull(reader.GetOrdinal("sla_due_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("sla_due_date")),
                                Channel = reader.IsDBNull(reader.GetOrdinal("channel")) ? null : reader.GetString(reader.GetOrdinal("channel")),
                                AttachmentsUrl = reader.IsDBNull(reader.GetOrdinal("attachments_url")) ? null : reader.GetString(reader.GetOrdinal("attachments_url")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            };
                        }
                    }
                }
                return ticketModel;
            }
        }

        public async Task<List<HelpdeskTicketModel>> GetAllHelpdeskTicketsByEmpIdAsync(Guid empId)
        {
            List<HelpdeskTicketModel> helpdeskTickets = new List<HelpdeskTicketModel>();
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_helpdesktickets_by_empid(@emp_id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("emp_id", empId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "helpdeskTickets_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new NpgsqlCommand("FETCH ALL IN helpdeskTickets_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            helpdeskTickets.Add(new HelpdeskTicketModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Subject = reader.GetString(reader.GetOrdinal("subject")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                Priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                Status = reader.GetInt32(reader.GetOrdinal("status")),
                                AssignedTo = reader.IsDBNull(reader.GetOrdinal("assigned_to")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("assigned_to")),
                                Resolution = reader.IsDBNull(reader.GetOrdinal("resolution")) ? null : reader.GetString(reader.GetOrdinal("resolution")),
                                ResolutionDate = reader.IsDBNull(reader.GetOrdinal("resolution_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resolution_date")),
                                SlaDueDate = reader.IsDBNull(reader.GetOrdinal("sla_due_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("sla_due_date")),
                                Channel = reader.IsDBNull(reader.GetOrdinal("channel")) ? null : reader.GetString(reader.GetOrdinal("channel")),
                                AttachmentsUrl = reader.IsDBNull(reader.GetOrdinal("attachments_url")) ? null : reader.GetString(reader.GetOrdinal("attachments_url")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            });
                        }
                    }
                }
                return helpdeskTickets;
            }
        }

        public async Task<List<HelpdeskTicketModel>> GetAllHelpdeskTicketsByOrgIdAsync(Guid orgId)
        {
            List<HelpdeskTicketModel> helpdeskTickets = new List<HelpdeskTicketModel>();
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_helpdesktickets_by_orgid(@org_id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("org_id", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "helpdeskTickets_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new NpgsqlCommand("FETCH ALL IN helpdeskTickets_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            helpdeskTickets.Add(new HelpdeskTicketModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Subject = reader.GetString(reader.GetOrdinal("subject")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                Priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                Status = reader.GetInt32(reader.GetOrdinal("status")),
                                AssignedTo = reader.IsDBNull(reader.GetOrdinal("assigned_to")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("assigned_to")),
                                Resolution = reader.IsDBNull(reader.GetOrdinal("resolution")) ? null : reader.GetString(reader.GetOrdinal("resolution")),
                                ResolutionDate = reader.IsDBNull(reader.GetOrdinal("resolution_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("resolution_date")),
                                SlaDueDate = reader.IsDBNull(reader.GetOrdinal("sla_due_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("sla_due_date")),
                                Channel = reader.IsDBNull(reader.GetOrdinal("channel")) ? null : reader.GetString(reader.GetOrdinal("channel")),
                                AttachmentsUrl = reader.IsDBNull(reader.GetOrdinal("attachments_url")) ? null : reader.GetString(reader.GetOrdinal("attachments_url")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            });
                        }
                    }
                }
                return helpdeskTickets;
            }
        }
        public async Task UpdateHelpdeskTicketAsync(HelpdeskTicketModel model)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_update_helpdesk_ticket(@p_id, @p_organization_id, @p_employee_id, @p_subject, @p_description, @p_priority, @p_status, @p_assigned_to, @p_resolution, @p_resolution_date, @p_sla_due_date, @p_channel, @p_attachments_url, @p_updated_at)", conn);
            
            cmd.Parameters.AddWithValue("p_id", model.Id);
            cmd.Parameters.AddWithValue("p_organization_id", model.OrganizationId);
            cmd.Parameters.AddWithValue("p_employee_id", model.EmployeeId);
            cmd.Parameters.AddWithValue("p_subject", model.Subject);
            cmd.Parameters.AddWithValue("p_description", model.Description);
            cmd.Parameters.AddWithValue("p_priority", model.Priority);
            cmd.Parameters.AddWithValue("p_status", model.Status);
            cmd.Parameters.AddWithValue("p_assigned_to", (object)model.AssignedTo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_resolution", (object)model.Resolution ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_resolution_date", (object)model.ResolutionDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_sla_due_date", (object)model.SlaDueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_channel", (object)model.Channel ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_attachments_url", (object)model.AttachmentsUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("p_updated_at", model.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        public async Task<bool> DeleteHelpdeskTicketAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT fn_delete_helpdesk_ticket(@p_id)", conn);
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
