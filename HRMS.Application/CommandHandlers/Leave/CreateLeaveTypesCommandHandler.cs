using HRMS.Application.Commands.Leave;
using HRMS.Domain.Entities.Leave;
using HRMS.Infrastructure.Interfaces.Leave;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Leave
{
    public class CreateLeaveTypesCommandHandler : IRequestHandler<CreateLeaveTypesCommand, Guid>
    {
        private readonly ILeaveTypesRepository _repo;

        public CreateLeaveTypesCommandHandler(ILeaveTypesRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateLeaveTypesCommand command, CancellationToken cancellationToken)
        {
            var entity  = new LeaveTypesModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = command.OrganizationId,
                Title = command.Title,
                Code = command.Code,
                Description = command.Description,
                IsPaid = command.IsPaid,
                CarryForward = command.CarryForward,
                MaxDaysPerYear = command.MaxDaysPerYear,
                MaxDaysPerMonth = command.MaxDaysPerMonth,
                RequiresApproval = command.RequiresApproval,
                IsEncashable = command.IsEncashable,
                GenderRestriction = command.GenderRestriction,
                MaritalRestriction = command.MaritalRestriction,
                StartMonth = command.StartMonth,
                IsActive = command.IsActive,
                CreatedAt = command.CreatedAt
            };
            await _repo.InsertAsync(entity);
            return entity.Id;
        }
    }
}
