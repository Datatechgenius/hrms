using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.RolePermission
{
    public class UpdateRolePermissionCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Module { get; set; }
        public string Permission { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
