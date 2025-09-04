using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee
{
    public class CreateEmployeeContactsCommand : IRequest<Guid>
    {
        public Guid EmployeeId { get; set; }
        public int ContactType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relation { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsEmergency { get; set; }
    }
}
