using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee
{
    public class GetEmployeeContactByIdQuery : IRequest<EmployeeContacts>
    {
        public Guid Id { get; set; }

        public GetEmployeeContactByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}