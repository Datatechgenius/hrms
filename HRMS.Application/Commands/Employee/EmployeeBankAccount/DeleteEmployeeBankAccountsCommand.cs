using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee
{
    public class DeleteEmployeeBankAccountsCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteEmployeeBankAccountsCommand(Guid id)
        {
            Id = id;
        }
    }
}