using HRMS.Domain.Entities.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Company
{
    public class UpdateCompanyCommand : IRequest
    {
        public Guid   Id                 { get; set; }
        public string Name               { get; set; }
        public string Code               { get; set; }
        public Guid   OrganizationId     { get; set; }
        public Guid?  DivisionId         { get; set; }
        public string LegalName          { get; set; }
        public string TaxId              { get; set; }
        public string DunsNumber         { get; set; }
        public DateTime? IncorporationDate { get; set; }
        public string CountryCode        { get; set; }
        public string CurrencyCode       { get; set; }
        public string Timezone           { get; set; }
        public string AddressLine1       { get; set; }
        public string AddressLine2       { get; set; }
        public string City               { get; set; }
        public string State              { get; set; }
        public string ZipCode            { get; set; }
        public string Phone              { get; set; }
        public string Email              { get; set; }
        public string Website            { get; set; }
        public bool   IsActive           { get; set; }
    }

}
