using HRMS.Application.Commands.Projects;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Projects
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectRepository _repository;

        public CreateProjectCommandHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Domain.Entities.Projects.ProjectsModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                DivisionId = request.DivisionId,
                CompanyId = request.CompanyId,
                Name = request.Name,
                ClientName = request.ClientName,
                Description = request.Description,
                ProjectCode = request.ProjectCode,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.CreateProjectAsync(project);
            return project.Id;
        }
    }
}
