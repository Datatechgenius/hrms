using HRMS.Domain.Entities.JobTitle;
using HRMS.Domain.Entities.Location;
using HRMS.Infrastructure.Interfaces.Location;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories.Location
{
    public class LocationRepository : ILocationRepository
    {
        private readonly string _connectionString;
        public LocationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddLocationAsync(LocationModel loc)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            if (loc == null)
            {
                throw new ArgumentNullException(nameof(loc), "Location model cannot be null");
            }

            var cmd = new NpgsqlCommand("CALL sp_insert_location(@id, @org_id, @div_id, @comp_id, @loc_code, @loc_name, @addr1, @addr2, @city, @state, @zip, @country, @tz, @primary, @contact, @email, @lat, @long, @created)", conn);

            cmd.Parameters.AddWithValue("id", loc.Id);
            cmd.Parameters.AddWithValue("org_id", loc.OrganizationId);
            cmd.Parameters.AddWithValue("div_id", loc.DivisionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("comp_id", loc.CompanyId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("loc_code", (object)loc.LocationCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("loc_name", loc.LocationName);
            cmd.Parameters.AddWithValue("addr1", loc.AddressLine1);
            cmd.Parameters.AddWithValue("addr2", (object)loc.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("city", loc.City);
            cmd.Parameters.AddWithValue("state", (object)loc.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("zip", (object)loc.ZipCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("country", loc.CountryCode);
            cmd.Parameters.AddWithValue("tz", (object)loc.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("primary", loc.IsPrimary ?? false);
            cmd.Parameters.AddWithValue("contact", (object)loc.ContactNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("email", (object)loc.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("lat", (object)loc.GpsLatitude ?? DBNull.Value);
            cmd.Parameters.AddWithValue("long", (object)loc.GpsLongitude ?? DBNull.Value);
            cmd.Parameters.AddWithValue("created", loc.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<LocationModel> GetLocationByIdAsync(Guid Id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            try
            {
                using (var tx = conn.BeginTransaction())
                {

                    using (var cmd = new NpgsqlCommand("CALL sp_get_location_by_id(@id, @ref)", conn))
                    {
                        cmd.Parameters.AddWithValue("id", Id);
                        cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value = "location_ref"
                        });
                        await cmd.ExecuteNonQueryAsync();
                    }
                    LocationModel location = null;

                    var fetchCmd = new NpgsqlCommand("FETCH ALL FROM location_ref;", conn);
                    using (var reader = await fetchCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            location = new LocationModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                                DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                                CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                                LocationCode = reader.IsDBNull(reader.GetOrdinal("location_code")) ? null : reader.GetString(reader.GetOrdinal("location_code")),
                                LocationName = reader.GetString(reader.GetOrdinal("location_name")),
                                AddressLine1 = reader.GetString(reader.GetOrdinal("address_line1")),
                                AddressLine2 = reader.IsDBNull(reader.GetOrdinal("address_line2")) ? null : reader.GetString(reader.GetOrdinal("address_line2")),
                                City = reader.GetString(reader.GetOrdinal("city")),
                                State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                                ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
                                CountryCode = reader.GetString(reader.GetOrdinal("country_code")),
                                Timezone = reader.IsDBNull(reader.GetOrdinal("timezone")) ? null : reader.GetString(reader.GetOrdinal("timezone")),
                                IsPrimary = reader.GetBoolean(reader.GetOrdinal("is_primary")),
                                ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? null : reader.GetString(reader.GetOrdinal("contact_number")),
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                                GpsLatitude = reader.IsDBNull(reader.GetOrdinal("gps_latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("gps_latitude")),
                                GpsLongitude = reader.IsDBNull(reader.GetOrdinal("gps_longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("gps_longitude")),
                                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            };
                        }
                    }
                    await tx.CommitAsync();
                    return location;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<List<LocationModel>> GetAllLocationsByOrgIdAsync(Guid orgId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_alllocations_by_org_id(@orgId, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("orgId", orgId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "locations_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var locations = new List<LocationModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM locations_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var location = new LocationModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            LocationCode = reader.IsDBNull(reader.GetOrdinal("location_code")) ? null : reader.GetString(reader.GetOrdinal("location_code")),
                            LocationName = reader.GetString(reader.GetOrdinal("location_name")),
                            AddressLine1 = reader.GetString(reader.GetOrdinal("address_line1")),
                            AddressLine2 = reader.IsDBNull(reader.GetOrdinal("address_line2")) ? null : reader.GetString(reader.GetOrdinal("address_line2")),
                            City = reader.GetString(reader.GetOrdinal("city")),
                            State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                            ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
                            CountryCode = reader.GetString(reader.GetOrdinal("country_code")),
                            Timezone = reader.IsDBNull(reader.GetOrdinal("timezone")) ? null : reader.GetString(reader.GetOrdinal("timezone")),
                            IsPrimary = reader.GetBoolean(reader.GetOrdinal("is_primary")),
                            ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? null : reader.GetString(reader.GetOrdinal("contact_number")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            GpsLatitude = reader.IsDBNull(reader.GetOrdinal("gps_latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("gps_latitude")),
                            GpsLongitude = reader.IsDBNull(reader.GetOrdinal("gps_longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("gps_longitude")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        locations.Add(location);
                    }
                }
                await tx.CommitAsync();
                return locations;
            }
        }

        public async Task<List<LocationModel>> GetAllLocationsByDivIdAsync(Guid divId)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var tx = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL sp_get_alllocations_by_div_id(@divId, @ref)", conn))
                {
                    cmd.Parameters.AddWithValue("divId", divId);
                    cmd.Parameters.Add(new NpgsqlParameter("ref", NpgsqlDbType.Refcursor)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "locations_ref"
                    });
                    await cmd.ExecuteNonQueryAsync();
                }
                var locations = new List<LocationModel>();
                var fetchCmd = new NpgsqlCommand("FETCH ALL FROM locations_ref;", conn);
                using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var location = new LocationModel
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            OrganizationId = reader.GetGuid(reader.GetOrdinal("organization_id")),
                            DivisionId = reader.IsDBNull(reader.GetOrdinal("division_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("division_id")),
                            CompanyId = reader.IsDBNull(reader.GetOrdinal("company_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("company_id")),
                            LocationCode = reader.IsDBNull(reader.GetOrdinal("location_code")) ? null : reader.GetString(reader.GetOrdinal("location_code")),
                            LocationName = reader.GetString(reader.GetOrdinal("location_name")),
                            AddressLine1 = reader.GetString(reader.GetOrdinal("address_line1")),
                            AddressLine2 = reader.IsDBNull(reader.GetOrdinal("address_line2")) ? null : reader.GetString(reader.GetOrdinal("address_line2")),
                            City = reader.GetString(reader.GetOrdinal("city")),
                            State = reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                            ZipCode = reader.IsDBNull(reader.GetOrdinal("zip_code")) ? null : reader.GetString(reader.GetOrdinal("zip_code")),
                            CountryCode = reader.GetString(reader.GetOrdinal("country_code")),
                            Timezone = reader.IsDBNull(reader.GetOrdinal("timezone")) ? null : reader.GetString(reader.GetOrdinal("timezone")),
                            IsPrimary = reader.GetBoolean(reader.GetOrdinal("is_primary")),
                            ContactNumber = reader.IsDBNull(reader.GetOrdinal("contact_number")) ? null : reader.GetString(reader.GetOrdinal("contact_number")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            GpsLatitude = reader.IsDBNull(reader.GetOrdinal("gps_latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("gps_latitude")),
                            GpsLongitude = reader.IsDBNull(reader.GetOrdinal("gps_longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("gps_longitude")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_at")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                        };
                        locations.Add(location);
                    }
                }
                await tx.CommitAsync();
                return locations;
            }
        }

        public async Task UpdateLocationAsync(LocationModel loc)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            if (loc == null)
            {
                throw new ArgumentNullException(nameof(loc), "Location model cannot be null");
            }
            var cmd = new NpgsqlCommand("CALL sp_update_location(@id, @org_id, @div_id, @comp_id, @loc_code, @loc_name, @addr1, @addr2, @city, @state, @zip, @country, @tz, @primary, @contact, @email, @lat, @long, @created)", conn);
            cmd.Parameters.AddWithValue("id", loc.Id);
            cmd.Parameters.AddWithValue("org_id", loc.OrganizationId);
            cmd.Parameters.AddWithValue("div_id", loc.DivisionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("comp_id", loc.CompanyId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("loc_code", (object)loc.LocationCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("loc_name", loc.LocationName);
            cmd.Parameters.AddWithValue("addr1", loc.AddressLine1);
            cmd.Parameters.AddWithValue("addr2", (object)loc.AddressLine2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("city", loc.City);
            cmd.Parameters.AddWithValue("state", (object)loc.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("zip", (object)loc.ZipCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("country", loc.CountryCode);
            cmd.Parameters.AddWithValue("tz", (object)loc.Timezone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("primary", loc.IsPrimary ?? false);
            cmd.Parameters.AddWithValue("contact", (object)loc.ContactNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("email", (object)loc.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("lat", (object)loc.GpsLatitude ?? DBNull.Value);
            cmd.Parameters.AddWithValue("long", (object)loc.GpsLongitude ?? DBNull.Value);
            cmd.Parameters.AddWithValue("created", loc.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<bool> DeleteLocationAsync(Guid id)
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT public.fn_delete_location(@id)", conn);
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