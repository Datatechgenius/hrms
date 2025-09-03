using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Employee
{
    public class GetAllEmployeesByCompanyQueryHandler : IRequestHandler<GetAllEmployeeByCompanyQuery, List<EmployeeModel>>
    {
        private readonly IEmployeeRepository _repo;

        public GetAllEmployeesByCompanyQueryHandler(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeModel>> Handle(GetAllEmployeeByCompanyQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllEmployeesByCompAsync(request.Id);
            return models
                .Select(model => new EmployeeModel
                {
                    Id = model.Id,
                    EmployeeNumber = model.EmployeeNumber,
                    DisplayName = model.DisplayName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PersonalEmail = model.PersonalEmail,
                    MobilePhone = model.MobilePhone,
                    WorkPhone = model.WorkPhone,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    MaritalStatus = model.MaritalStatus,
                    MarriageDate = model.MarriageDate,
                    JoiningDate = model.JoiningDate,
                    ExitDate = model.ExitDate,
                    ProbationEndDate = model.ProbationEndDate,
                    EmploymentStatus = model.EmploymentStatus,
                    AccountStatus = model.AccountStatus,
                    InvitationStatus = model.InvitationStatus,
                    IsProfileComplete = model.IsProfileComplete,
                    IsPrivate = model.IsPrivate,
                    OrganizationId = model.OrganizationId,
                    DivisionId = model.DivisionId,
                    CompanyId = model.CompanyId,
                    DepartmentId = model.DepartmentId,
                    JobTitleId = model.JobTitleId,
                    SecondaryJobTitleId = model.SecondaryJobTitleId,
                    ContingentTypeId = model.ContingentTypeId,
                    ReportsToId = model.ReportsToId,
                    ManagerId = model.ManagerId,
                    DottedLineManagerId = model.DottedLineManagerId,
                    AttendanceNumber = model.AttendanceNumber,
                    ResignationSubmittedDate = model.ResignationSubmittedDate,
                    ProfessionalSummary = model.ProfessionalSummary,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt
                })
                .ToList();
        }
    }
}