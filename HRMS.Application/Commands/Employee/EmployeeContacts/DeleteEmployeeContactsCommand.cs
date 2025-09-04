using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee
{
    public class DeleteEmployeeContactsCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteEmployeeContactsCommand(Guid id)
        {
            Id = id;
        }
    }
}