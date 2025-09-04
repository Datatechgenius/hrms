using HRMS.Domain.Entities.Departments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Departments
{
    public class GetDepartmentByCompanyIdQuery : IRequest<List<DepartmentModel>>
    {
        public Guid Id { get; set; }

        public GetDepartmentByCompanyIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
