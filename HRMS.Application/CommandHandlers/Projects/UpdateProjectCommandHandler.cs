using HRMS.Application.Commands.Projects;
using HRMS.Domain.Entities.Projects;
using HRMS.Infrastructure.Interfaces.Projects;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Projects
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {

        private readonly IProjectRepository _repository;
        public UpdateProjectCommandHandler(IProjectRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new ProjectsModel
            {
                Id = request.Id,
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
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.UpdateProjectAsync(project);

        }
    }
}

