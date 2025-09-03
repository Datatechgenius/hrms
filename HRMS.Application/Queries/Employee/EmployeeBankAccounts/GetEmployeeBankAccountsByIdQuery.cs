using HRMS.Domain.Entities.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Employee
{
    public class GetEmployeeBankAccountsByIdQuery : IRequest<EmployeeBankAccounts>
    {
        public Guid Id { get; set; }

        public GetEmployeeBankAccountsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}