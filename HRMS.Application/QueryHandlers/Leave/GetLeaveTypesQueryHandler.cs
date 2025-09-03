using HRMS.Application.Queries.Leave;
using HRMS.Domain.Entities.Leave;
using HRMS.Infrastructure.Interfaces.Leave;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Leave
{
    public class GetLeaveTypesQueryHandler : IRequestHandler<GetLeaveTypeByIdQuery, LeaveTypesModel>
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;

        public GetLeaveTypesQueryHandler(ILeaveTypesRepository leaveTypesRepository)
        {
            _leaveTypesRepository = leaveTypesRepository;
        }

        public async Task<LeaveTypesModel> Handle(GetLeaveTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await _leaveTypesRepository.GetByIdAsync(request.Id);

            if (model == null) return null;

            return new LeaveTypesModel
            {
                Id = model.Id,
                OrganizationId = model.OrganizationId,
                Title = model.Title,
                Code = model.Code,
                Description = model.Description,
                IsPaid = model.IsPaid,
                CarryForward = model.CarryForward,
                MaxDaysPerYear = model.MaxDaysPerYear,
                MaxDaysPerMonth = model.MaxDaysPerMonth,
                RequiresApproval = model.RequiresApproval,
                IsEncashable = model.IsEncashable,
                GenderRestriction = model.GenderRestriction,
                MaritalRestriction = model.MaritalRestriction,
                StartMonth = model.StartMonth,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }
    }
}
