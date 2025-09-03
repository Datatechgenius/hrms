using HRMS.Application.Commands.ProjectAssigment;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.ProjectAssigment
{
    public class CreateProjectAssignmentCommandHandler : IRequestHandler<CreateProjectAssignmentCommand, Guid>
    {
        private readonly IProjectAssignmentRepository _repo;

        public CreateProjectAssignmentCommandHandler(IProjectAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateProjectAssignmentCommand command, CancellationToken cancellationToken)
        {
            var entity  = new ProjectAssignmentModel
            {
                Id = Guid.NewGuid(),
                EmployeeId = command.EmployeeId,
                ProjectId = command.ProjectId,
                Role = command.Role ?? string.Empty,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
                BillingType = command.BillingType,
                AllocationPercent = command.AllocationPercent,
                ReportingManagerId = command.ReportingManagerId,
                Status = command.Status,
                Notes = command.Notes,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.ProjectAssignmentInsertAsync(entity);
            return entity.Id;
        }
    }
}
