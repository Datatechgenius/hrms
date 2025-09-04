using HRMS.Domain.Entities.Departments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Departments
{
    public class GetDepartmentByIdQuery : IRequest<DepartmentModel>
    {
        public Guid Id { get; }
        public GetDepartmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
