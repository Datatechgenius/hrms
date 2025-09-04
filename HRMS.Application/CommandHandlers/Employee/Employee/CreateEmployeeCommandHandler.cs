using HRMS.Application.Commands.Employee;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand , Guid>
    {
        private readonly IEmployeeRepository _repo;

        public CreateEmployeeCommandHandler(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var model = new EmployeeModel
            {
                Id = Guid.NewGuid(),
                EmployeeNumber = request.EmployeeNumber,
                DisplayName = request.DisplayName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Email = request.Email,
                PersonalEmail = request.PersonalEmail,
                MobilePhone = request.MobilePhone,
                WorkPhone = request.WorkPhone,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                MaritalStatus = request.MaritalStatus,
                MarriageDate = request.MarriageDate,
                JoiningDate = request.JoiningDate,
                ExitDate = request.ExitDate,
                ProbationEndDate = request.ProbationEndDate,
                EmploymentStatus = request.EmploymentStatus,
                AccountStatus = request.AccountStatus,
                InvitationStatus = request.InvitationStatus,
                IsProfileComplete = request.IsProfileComplete,
                IsPrivate = request.IsPrivate,
                OrganizationId = request.OrganizationId,
                DivisionId = request.DivisionId,
                CompanyId = request.CompanyId,
                DepartmentId = request.DepartmentId,
                JobTitleId = request.JobTitleId,
                SecondaryJobTitleId = request.SecondaryJobTitleId,
                ContingentTypeId = request.ContingentTypeId,
                ReportsToId = request.ReportsToId,
                ManagerId = request.ManagerId,
                DottedLineManagerId = request.DottedLineManagerId,
                AttendanceNumber = request.AttendanceNumber,
                ResignationSubmittedDate = request.ResignationSubmittedDate,
                ProfessionalSummary = request.ProfessionalSummary,
                CreatedAt = DateTime.UtcNow      
            };
            await _repo.AddEmployeeAsync(model);
            return model.Id;
        }
    }
}