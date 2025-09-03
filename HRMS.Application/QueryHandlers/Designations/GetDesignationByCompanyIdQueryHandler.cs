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
    public class GetDesignationByCompanyIdQueryHandler : IRequestHandler<GetDesignationByDepartmentIdQuery, List<DesignationModel>>
    {
         private readonly IDesignationRepository _repository;

        public GetDesignationByCompanyIdQueryHandler(IDesignationRepository repo)
        {
            _repository = repo ;
        }
        public async Task<List<DesignationModel>> Handle(GetDesignationByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            var designation = await _repository.GetDesignationByDepartmentIdAsync(request.Id);

            if (designation == null || designation.Count == 0)
                return new List<DesignationModel>();

            return designation
                .Select(d => new DesignationModel
                {
                    Id = d.Id,
                    OrganizationId = d.OrganizationId,
                    Title = d.Title,
                    Code = d.Code,
                    Description = d.Description,
                    DepartmentId = d.DepartmentId,
                    Level = d.Level,
                    IsBillable = d.IsBillable,
                    Status = d.Status,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
            .ToList();
        }
    }
}
