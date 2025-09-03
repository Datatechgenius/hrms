using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.RolePermission
{
    public class CreateRolePermissionCommand : IRequest<Guid>
    {
        public Guid RoleId { get; set; }
        public string Module { get; set; }
        public string Permission { get; set; }
    }
}
