using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee
{
    public class DeleteEmployeeFamilyMembersCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public DeleteEmployeeFamilyMembersCommand(Guid id)
        {
            Id = id;
        }
    }
}