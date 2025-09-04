using HRMS.Application.Queries.Company;
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
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentModel>
    {
         private readonly IDepartmentRepository _repo;

        public GetDepartmentByIdQueryHandler(IDepartmentRepository repo)
        {
            _repo = repo ;
        }

        public async Task<DepartmentModel> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _repo.GetDepartmentByIdAsync(request.Id);
            if (department == null) return null;
            return new DepartmentModel
            {
                 Id             = department.Id,
                 Name           = department.Name,
                 Code           = department.Code,
                 OrganizationId = department.OrganizationId,
                 DivisionId     = department.DivisionId,
                 CompanyId      = department.CompanyId,
                 Email          = department.Email,
                 Phone          = department.Phone,
                 IsActive       = department.IsActive,
                 CreatedAt      = department.CreatedAt,
                 UpdatedAt      = department.UpdatedAt
            };
        }
    }
}
