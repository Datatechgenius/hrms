using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Divisions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRMS.Infrastructure.Repositories.Divisions
{
   public class DivisionRepository : IDivisionRepository
    {
        private readonly string _connectionString;

        public DivisionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(Division division)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("CALL sp_insert_division(@id, @name, @orgId, @code, @short, @active, " +
                                              "@desc, @email, @phone, @tz, @isActive, @created, @updated)", conn);

            cmd.Parameters.AddWithValue("id", division.Id);
            cmd.Parameters.AddWithValue("name", division.Name);
            cmd.Parameters.AddWithValue("orgId", division.OrganizationId);
            cmd.Parameters.AddWithValue("code", (object)division.DivisionCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("short", (object)division.ShortName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("active", (object)division.Active ?? DBNull.Value);
            cmd.Parameters.AddWithValue("desc", (object)division.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("email", (object)division.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("phone", (object)division.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("tz", (object)division.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("isActive", division.IsActive);
            cmd.Parameters.AddWithValue("created", division.CreatedAt);
            cmd.Parameters.AddWithValue("updated", division.UpdatedAt);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<Division> GetByIdAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
               
                using (var cmd = new NpgsqlCommand("CALL sp_get_division_by_id(@id, @ref)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value     = "division_cursor"
                    });

                    await cmd.ExecuteNonQueryAsync();
                }
                Division division = null;

           
                using (var fetch = new NpgsqlCommand("FETCH ALL FROM division_cursor;", conn, tx))
                using (var reader = await fetch.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        division = new Division
                            {
 
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionCode = reader.IsDBNull(reader.GetOrdinal("division_code")) ? null : reader.GetString(reader.GetOrdinal("division_code")),
                            ShortName = reader.IsDBNull(reader.GetOrdinal("short_name")) ? null : reader.GetString(reader.GetOrdinal("short_name")),
                            Active = reader.IsDBNull(reader.GetOrdinal("active")) ? false : reader.GetBoolean(reader.GetOrdinal("active")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                            //HeadId = reader.GetGuid(reader.GetOrdinal("head_id")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                            //LocationId = reader.GetGuid(reader.GetOrdinal("location_id")),
                            Timezone = reader.IsDBNull(reader.GetOrdinal("timezone")) ? null : reader.GetString(reader.GetOrdinal("timezone")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                    }
                }
            await tx.CommitAsync();
            return division;

            } 
        }

        public async Task<List<Division>> GetAllByIdAsync(Guid orgId)
        {
            var divisions = new List<Division>();

            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Transaction needed for REF_CURSOR
            using (var tx = conn.BeginTransaction())
            {
                // 1) Call the proc to open the cursor
                using (var cmd = new NpgsqlCommand("CALL sp_get_alldivision_by_orgid(@id, @ref)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("id", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value     = "division_cursor"
                    });

                    await cmd.ExecuteNonQueryAsync();
                }

                // 2) Fetch all rows from the named cursor
                using (var fetch = new NpgsqlCommand("FETCH ALL FROM division_cursor;", conn, tx))
                using (var reader = await fetch.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var division = new Division
                        {
                            Id             = reader.GetGuid(reader.GetOrdinal("id")),
                            Name           = reader.GetString(reader.GetOrdinal("name")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionCode   = reader.IsDBNull(reader.GetOrdinal("division_code"))
                                               ? null : reader.GetString(reader.GetOrdinal("division_code")),
                            ShortName      = reader.IsDBNull(reader.GetOrdinal("short_name"))
                                               ? null : reader.GetString(reader.GetOrdinal("short_name")),
                            Active         = reader.IsDBNull(reader.GetOrdinal("active"))
                                               ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("active")),
                            Description    = reader.IsDBNull(reader.GetOrdinal("description"))
                                               ? null : reader.GetString(reader.GetOrdinal("description")),
                            Email          = reader.IsDBNull(reader.GetOrdinal("email"))
                                               ? null : reader.GetString(reader.GetOrdinal("email")),
                            Phone          = reader.IsDBNull(reader.GetOrdinal("phone"))
                                               ? null : reader.GetString(reader.GetOrdinal("phone")),
                            Timezone       = reader.IsDBNull(reader.GetOrdinal("timezone"))
                                               ? null : reader.GetString(reader.GetOrdinal("timezone")),
                            IsActive       = reader.GetBoolean(reader.GetOrdinal("is_active")),
                            CreatedAt      = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt      = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };

                        divisions.Add(division);
                    }
                }

                // 3) Commit to close the cursor
                await tx.CommitAsync();
            }

            return divisions;
        }

        public async Task UpdateAsync(Division division)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "CALL sp_update_division(@id, @name, @code, @short, @active, @desc, @email, @phone, @tz, @isActive, @updated)", 
                conn);  

            cmd.Parameters.AddWithValue("id", division.Id);
            cmd.Parameters.AddWithValue("name", division.Name);
            cmd.Parameters.AddWithValue("code", (object)division.DivisionCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("short", (object)division.ShortName  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("active", division.Active ?? false);
            cmd.Parameters.AddWithValue("desc", (object)division.Description  ?? DBNull.Value);
            //cmd.Parameters.AddWithValue("head", division.HeadId);
            cmd.Parameters.AddWithValue("email", (object)division.Email        ?? DBNull.Value);
            cmd.Parameters.AddWithValue("phone", (object)division.Phone        ?? DBNull.Value);
            //cmd.Parameters.AddWithValue("loc", division.LocationId);
            cmd.Parameters.AddWithValue("tz", (object)division.Timezone       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("isActive", division.IsActive);
            cmd.Parameters.AddWithValue("updated", DateTime.UtcNow);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_division(@id)", conn);
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
