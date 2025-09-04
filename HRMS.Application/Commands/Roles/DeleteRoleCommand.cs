using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Roles
{
    public class DeleteRoleCommand : IRequest
    {
        public Guid Id { get; set; }
        public DeleteRoleCommand(Guid id)
        {
            Id = id;
        }
    }
}
