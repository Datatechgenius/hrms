using MediatR;
using System;
using HRMS.Domain.Entities;

namespace HRMS.Application.Commands.Employee
{
    public class UpdateEmployeeCommand : IRequest
    {
        public Guid Id { get; set; }

        public string EmployeeNumber { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PersonalEmail { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }

        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public DateTime? MarriageDate { get; set; }

        public DateTime? JoiningDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }

        public EmployeeStatus? EmploymentStatus { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public InvitationStatus? InvitationStatus { get; set; }

        public bool IsProfileComplete { get; set; }
        public bool IsPrivate { get; set; }

        public Guid? OrganizationId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? DepartmentId { get; set; }

        public Guid? JobTitleId { get; set; }
        public Guid? SecondaryJobTitleId { get; set; }
        public Guid? ContingentTypeId { get; set; }

        public Guid? ReportsToId { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? DottedLineManagerId { get; set; }

        public string AttendanceNumber { get; set; }
        public DateTime? ResignationSubmittedDate { get; set; }
        public string ProfessionalSummary { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}