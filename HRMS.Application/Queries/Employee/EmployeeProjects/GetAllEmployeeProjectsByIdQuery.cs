using HRMS.Domain.Entities.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee.EmployeeProject
{
    public class GetAllEmployeeProjectsByIdQuery : IRequest<List<EmployeeProjects>>
    {
        public Guid Id { get; set; }
        public GetAllEmployeeProjectsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}