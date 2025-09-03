using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee
{
    public class GetAllEmployeeByDivisionQuery : IRequest<List<EmployeeModel>>
    {
        public Guid Id { get; set; }

        public GetAllEmployeeByDivisionQuery(Guid id)
        {
            Id = id;
        }
    }
}
