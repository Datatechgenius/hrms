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
    public class GetAllContactsByEmployeeIdQuery : IRequest<List<EmployeeContacts>>
    {
        public Guid Id { get; set; }

        public GetAllContactsByEmployeeIdQuery(Guid id)
        {
            Id = id;
        }
    }
}