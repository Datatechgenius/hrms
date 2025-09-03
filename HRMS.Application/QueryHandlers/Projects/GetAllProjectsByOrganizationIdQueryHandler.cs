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
    public class GetAllProjectsByOrganizationIdQueryHandler : IRequestHandler<GetAllProjectByOrganizationIdQuery, List<ProjectsModel>>
    {
        private readonly IProjectRepository _projectRepository;
        public GetAllProjectsByOrganizationIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<List<ProjectsModel>> Handle(GetAllProjectByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllProjectsByOrganizationIdAsync(request.OrganizationId);
            return projects.Select(p => new ProjectsModel
            {
                Id = p.Id,
                OrganizationId = p.OrganizationId,
                DivisionId = p.DivisionId,
                CompanyId = p.CompanyId,
                Name = p.Name,
                ClientName = p.ClientName,
                Description = p.Description,
                ProjectCode = p.ProjectCode,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }
    }
}
