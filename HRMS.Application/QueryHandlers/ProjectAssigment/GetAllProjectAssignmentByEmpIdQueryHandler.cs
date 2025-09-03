using HRMS.Application.Queries.ProjectAssigment;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.ProjectAssigment
{
    public class GetAllProjectAssignmentByEmpIdQueryHandler : IRequestHandler<GetAllProjectAssignmentByEmpIdQuery, List<ProjectAssignmentModel>>
    {
        private readonly IProjectAssignmentRepository _repo;
        public GetAllProjectAssignmentByEmpIdQueryHandler(IProjectAssignmentRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<ProjectAssignmentModel>> Handle(GetAllProjectAssignmentByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllByEmployeeIdAsync(request.EmployeeId);
            return models
                .Select(model => new ProjectAssignmentModel
                {
                    Id = model.Id,
                    ProjectId = model.ProjectId,
                    EmployeeId = model.EmployeeId,
                    Role = model.Role,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    BillingType = model.BillingType,
                    AllocationPercent = model.AllocationPercent,
                    ReportingManagerId = model.ReportingManagerId,
                    Status = model.Status,
                    Notes = model.Notes,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt
                })
                .ToList();
        }
    }
}
