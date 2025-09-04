using HRMS.Domain.Entities.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee
{
    public class GetEmployeeFamilyMembersByIdQuery : IRequest<EmployeeFamilyMembers>
    {
        public Guid Id { get; set; }

        public GetEmployeeFamilyMembersByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}