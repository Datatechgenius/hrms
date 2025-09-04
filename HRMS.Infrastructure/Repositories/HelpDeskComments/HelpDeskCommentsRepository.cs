using HRMS.Domain.Entities.HelpDesk;
using HRMS.Domain.Entities.HelpDeskComments;
using HRMS.Infrastructure.Interfaces.HelpDeskComments;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.HelpDeskComments
{
    public class HelpDeskCommentsRepository : IHelpDeskCommentsRepository
    {
        private readonly string _connectionString;
        public HelpDeskCommentsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task CreateHelpdeskCommentAsync(HelpDeskCommentsModel commentsModel)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("CALL sp_insert_helpdesk_comment(@p_id, @p_ticket_id, @p_employee_id, @p_comment, @p_created_at)", conn);

            cmd.Parameters.AddWithValue("p_id", commentsModel.Id);
            cmd.Parameters.AddWithValue("p_ticket_id", commentsModel.TicketId);
            cmd.Parameters.AddWithValue("p_employee_id", commentsModel.EmployeeId);
            cmd.Parameters.AddWithValue("p_comment", commentsModel.Comment);
            cmd.Parameters.AddWithValue("p_created_at", commentsModel.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
            conn.Close();
        }
        public async Task<HelpDeskCommentsModel> GetHelpdeskCommentByIdAsync(Guid id)
        {
            HelpDeskCommentsModel commentsModel = null;

            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_comment_by_id(@id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "ticketComment_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }

                using (var cmd = new NpgsqlCommand("FETCH ALL IN ticketComment_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            commentsModel = new HelpDeskCommentsModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                TicketId = reader.GetGuid(reader.GetOrdinal("ticket_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Comment = reader.GetString(reader.GetOrdinal("comment")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                            };
                        }
                    }
                }
                return commentsModel;
            }
        }
        public async Task<List<HelpDeskCommentsModel>> GetAllHelpdeskCommentsByTicketIdAsync(Guid ticketId)
        {
            List<HelpDeskCommentsModel> commentsList = new List<HelpDeskCommentsModel>();
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_all_comments_by_ticket_id(@ticket_id, @ref);", conn))
                {
                    cmd.Parameters.AddWithValue("ticket_id", ticketId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "comments_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new NpgsqlCommand("FETCH ALL IN comments_ref;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var commentsModel = new HelpDeskCommentsModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                TicketId = reader.GetGuid(reader.GetOrdinal("ticket_id")),
                                EmployeeId = reader.GetGuid(reader.GetOrdinal("employee_id")),
                                Comment = reader.GetString(reader.GetOrdinal("comment")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                            };
                            commentsList.Add(commentsModel);
                        }
                    }
                }
                return commentsList;
            }
        }

        public async Task<bool> DeleteHelpdeskCommentAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT fn_delete_helpdesk_comment(@p_id)", conn);
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
