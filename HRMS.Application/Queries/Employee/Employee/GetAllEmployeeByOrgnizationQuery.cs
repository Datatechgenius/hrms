using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee
{
    public class GetAllEmployeeByOrgnizationQuery : IRequest<List<EmployeeModel>>
    {
        public Guid Id { get; set; }

        public GetAllEmployeeByOrgnizationQuery(Guid id)
        {
            Id = id;
        }
    }
}
