using HRMS.Application.Queries.Employee;
using HRMS.Application.Queries.Employee.EmployeeProject;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Employee.EmployeeProject
{
    public class GetAllEmployeeProjectsByIdQueryHandler : IRequestHandler<GetAllEmployeeProjectsByIdQuery, List<EmployeeProjects>>
    {
        private readonly IEmployeeProjectsRepository _repo;

        public GetAllEmployeeProjectsByIdQueryHandler(IEmployeeProjectsRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeProjects>> Handle(GetAllEmployeeProjectsByIdQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllByEmployeeIdAsync(request.Id);
            return models
                .Select(model => new EmployeeProjects
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    ProjectId = model.ProjectId,
                    Role = model.Role,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsBillable = model.IsBillable,
                    AllocationPercent = model.AllocationPercent,
                    ReportingManagerId = model.ReportingManagerId,
                    Status = model.Status,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt
                })
                .ToList();
        }
    }
}