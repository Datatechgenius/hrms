using HRMS.Application.Queries.ProjectAssigment;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.ProjectAssigment
{
    public class GetProjectAssignmentByIdQueryHandler : IRequestHandler<GetProjectAssignmentByIdQuery, ProjectAssignmentModel>
    {
        private readonly IProjectAssignmentRepository _assignmentRepository;
        public GetProjectAssignmentByIdQueryHandler(IProjectAssignmentRepository  projectRepository)
        {
            _assignmentRepository = projectRepository;
        }

        public async Task<ProjectAssignmentModel> Handle(GetProjectAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await _assignmentRepository.GetProjectAssignmentByIdAsync(request.Id);
            if (model == null) return null;
            return new ProjectAssignmentModel
            {
                Id = model.Id,
                EmployeeId = model.EmployeeId,
                ProjectId = model.ProjectId,
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
            };
        }
    }
}
