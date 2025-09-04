using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class OrganizationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [MaxLength(50)]
        public string TaxId { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Website { get; set; }
        public string Industry { get; set; }
        public string OrgType { get; set; }
        public int? Size { get; set; }
        public DateTime? IncorporationDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        [StringLength(3)]
        public string CountryCode { get; set; }

        public string Timezone { get; set; }

        [StringLength(3)]
        public string CurrencyCode { get; set; }

        public string LogoUrl { get; set; }
        public string Status { get; set; }
    }
}