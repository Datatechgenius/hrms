using HRMS.Application.Queries.Employee;
using HRMS.Application.Queries.Employee.EmployeeProject;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces;
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
    public class GetEmployeeProjectsByIdQueryHandler : IRequestHandler<GetEmployeeProjectsByIdQuery, EmployeeProjects>
    {
        private readonly IEmployeeProjectsRepository _repo;

        public GetEmployeeProjectsByIdQueryHandler(IEmployeeProjectsRepository repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeProjects> Handle(GetEmployeeProjectsByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await _repo.GetEmployeeProjectsByIdAsync(request.Id);
            if (model == null) return null;

            return new EmployeeProjects
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
            };
        }
    }
}