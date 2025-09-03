using HRMS.Application.Queries.Departments;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Departments;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Departments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Departments
{
    public class GetDepartmantsByCompanyIdQueryHandler : IRequestHandler<GetDepartmentByCompanyIdQuery, List<DepartmentModel>>
    {
        private readonly IDepartmentRepository _repository;

        public GetDepartmantsByCompanyIdQueryHandler(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepartmentModel>> Handle(GetDepartmentByCompanyIdQuery request,CancellationToken cancellationToken)
        {
            var departments = await _repository.GetDepartmantsByCompanyIdAsync(request.Id);

            if (departments == null || departments.Count == 0)
                return new List<DepartmentModel>();

            return departments
                .Select(d => new DepartmentModel
                {
                     Id             = d.Id,
                     Name           = d.Name,
                     Code           = d.Code,
                     OrganizationId = d.OrganizationId,
                     DivisionId     = d.DivisionId,
                     CompanyId      = d.CompanyId,
                     Email          = d.Email,
                     Phone          = d.Phone,
                     IsActive       = d.IsActive,
                     CreatedAt      = d.CreatedAt,
                     UpdatedAt      = d.UpdatedAt
                })
            .ToList();
        }
    }
}

