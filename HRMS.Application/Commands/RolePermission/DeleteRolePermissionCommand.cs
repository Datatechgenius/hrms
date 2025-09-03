using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.RolePermission
{
    public class DeleteRolePermissionCommand : IRequest
    {
        public Guid Id { get; set; }
        public DeleteRolePermissionCommand(Guid id)
        {
            Id = id;
        }
    }
}
