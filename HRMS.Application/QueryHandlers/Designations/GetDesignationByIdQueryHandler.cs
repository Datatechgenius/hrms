using HRMS.Application.Queries.Departments;
using HRMS.Application.Queries.Designations;
using HRMS.Domain.Entities.Departments;
using HRMS.Domain.Entities.Designations;
using HRMS.Infrastructure.Interfaces.Departments;
using HRMS.Infrastructure.Interfaces.Designations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Designations
{
    public class GetDesignationByIdQueryHandler : IRequestHandler<GetDesignationByIdQuery, GetDesignationModel>
    {
         private readonly IDesignationRepository _repo;

        public GetDesignationByIdQueryHandler(IDesignationRepository repo)
        {
            _repo = repo ;
        }
        public async Task<GetDesignationModel> Handle(GetDesignationByIdQuery request, CancellationToken cancellationToken)
        {
            var designation = await _repo.GetDesignationByIdAsync(request.Id);
            if (designation == null) return null;
            return new GetDesignationModel
            {
                Id = designation.Id,
                OrganizationId = designation.OrganizationId,
                Title = designation.Title,
                Code = designation.Code,
                Description = designation.Description,
                DepartmentId = designation.DepartmentId,
                Level = designation.Level,
                IsBillable = designation.IsBillable,
                Status = designation.Status,
                CreatedAt = designation.CreatedAt,
                UpdatedAt = designation.UpdatedAt
            };
        }
    }
}
