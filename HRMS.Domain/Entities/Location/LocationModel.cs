using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.Location
{
    public class LocationModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? CompanyId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
        public bool? IsPrimary { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public decimal? GpsLatitude { get; set; }
        public decimal? GpsLongitude { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
