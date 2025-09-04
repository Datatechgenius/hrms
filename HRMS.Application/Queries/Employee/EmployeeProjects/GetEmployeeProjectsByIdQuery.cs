using HRMS.Domain.Entities.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee.EmployeeProject
{
    public class GetEmployeeProjectsByIdQuery : IRequest<EmployeeProjects>
    {
        public Guid Id { get; set; }

        public GetEmployeeProjectsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}