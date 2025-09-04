using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Organization
{
   public class UpdateOrganizationCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public string TaxId { get; set; }
        public string Phone { get; set; }
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
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
        public string CurrencyCode { get; set; }
        public string LogoUrl { get; set; }
        public string Status { get; set; }
    }

}
