using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee
{
    public class CreateEmployeeFamilyMemberCommand : IRequest<Guid>
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayNmae { get; set; }
        public int RelationType { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Profession { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public bool IsDependent { get; set; }
        public bool IsEmergency { get; set; }
        public bool IsNominee { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
