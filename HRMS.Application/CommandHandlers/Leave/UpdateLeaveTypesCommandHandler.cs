using HRMS.Application.Commands.Leave;
using HRMS.Infrastructure.Interfaces.Leave;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Leave
{
    public class UpdateLeaveTypesCommandHandler : IRequestHandler<UpdateLeaveTypesCommand>
    {
        private readonly ILeaveTypesRepository _repo;

        public UpdateLeaveTypesCommandHandler(ILeaveTypesRepository repo)
        {
            _repo = repo;
        }
         public async Task Handle(UpdateLeaveTypesCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id);
            
             entity.OrganizationId = command.OrganizationId;
            entity.Title = command.Title;
            entity.Code = command.Code;
            entity.Description = command.Description;
            entity.IsPaid = command.IsPaid;
            entity.CarryForward = command.CarryForward;
            entity.MaxDaysPerYear = command.MaxDaysPerYear;
            entity.MaxDaysPerMonth = command.MaxDaysPerMonth;
            entity.RequiresApproval = command.RequiresApproval;
            entity.IsEncashable = command.IsEncashable;
            entity.GenderRestriction = command.GenderRestriction;
            entity.MaritalRestriction = command.MaritalRestriction;
            entity.StartMonth = command.StartMonth;
            entity.IsActive = command.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
        }
    }
}
