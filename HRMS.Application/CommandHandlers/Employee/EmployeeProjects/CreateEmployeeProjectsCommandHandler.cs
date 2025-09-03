using HRMS.Application.Commands.Employee;
using HRMS.Application.Commands.Employee.EmployeeProject;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee.EmployeeProject
{
    public class CreateEmployeeProjectsCommandHandler : IRequestHandler<CreateEmployeeProjectsCommand, Guid>
    {
        private readonly IEmployeeProjectsRepository _repo;

        public CreateEmployeeProjectsCommandHandler(IEmployeeProjectsRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateEmployeeProjectsCommand request, CancellationToken cancellationToken)
        {
            var entity = new EmployeeProjects
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                ProjectId = request.ProjectId,
                Role = request.Role ?? string.Empty,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsBillable = request.IsBillable,
                AllocationPercent = request.AllocationPercent,
                ReportingManagerId = request.ReportingManagerId,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddEmployeeProjectsAsync(entity);
            return entity.Id;
        }
    }
}
