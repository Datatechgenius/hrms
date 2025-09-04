using HRMS.Application.Queries.Leave;
using HRMS.Domain.Entities.Leave;
using HRMS.Infrastructure.Interfaces.Leave;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Leave
{
    public class GetAllLeaveTypesQueryHandler : IRequestHandler<GetAllLeaveTypesQuery, List<LeaveTypesModel>>
    {
        private readonly ILeaveTypesRepository _repo;
        public GetAllLeaveTypesQueryHandler(ILeaveTypesRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<LeaveTypesModel>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllAsync(request.LeaveTypesId);
            return models
                .Select(model => new LeaveTypesModel
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
                })
                .ToList();
        }
    }
}
