using HRMS.Application.Commands.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.ProjectAssigment
{
    public class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand , bool>
    {
        private readonly IProjectAssignmentRepository _repo;

        public UpdateAssignmentCommandHandler(IProjectAssignmentRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> Handle(UpdateAssignmentCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetProjectAssignmentByIdAsync(command.Id);
            
            entity.Id = command.Id;
            entity.EmployeeId = command.EmployeeId;
            entity.ProjectId = command.ProjectId;
            entity.Role = command.Role ?? string.Empty;
            entity.StartDate = command.StartDate;
            entity.EndDate = command.EndDate;
            entity.BillingType = command.BillingType;
            entity.AllocationPercent = command.AllocationPercent;
            entity.ReportingManagerId = command.ReportingManagerId;
            entity.Status = command.Status;
            entity.Notes = command.Notes;
            entity.UpdatedAt = DateTime.UtcNow;

           return await _repo.UpdateProjectAssignmentAsync(entity);
        }
    }
}
