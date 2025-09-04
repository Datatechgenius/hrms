using HRMS.Application.Queries.Projects;
using HRMS.Domain.Entities.Projects;
using HRMS.Infrastructure.Interfaces.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Projects
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectsModel>
    {
        private readonly IProjectRepository _projectRepository;
        public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<ProjectsModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (projects == null) return null;
            return new ProjectsModel
            {
                Id = projects.Id,
                OrganizationId = projects.OrganizationId,
                DivisionId = projects.DivisionId,
                CompanyId = projects.CompanyId,
                Name = projects.Name,
                ClientName = projects.ClientName,
                Description = projects.Description,
                ProjectCode = projects.ProjectCode,
                StartDate = projects.StartDate,
                EndDate = projects.EndDate,
                Status = projects.Status,
                CreatedAt = projects.CreatedAt,
                UpdatedAt = projects.UpdatedAt
            };
        }
    }
    
}
