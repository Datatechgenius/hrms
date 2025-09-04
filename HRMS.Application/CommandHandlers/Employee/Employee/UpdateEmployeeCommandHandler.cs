using HRMS.Application.Commands.Employee;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IEmployeeRepository _repo;

        public UpdateEmployeeCommandHandler(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new EmployeeModel
            {
                Id = request.Id,
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
                UpdatedAt = request.UpdatedAt,
            };

            await _repo.UpdateEmployeeAsync(employee);
            //return Unit.Value;
        }
    }
}