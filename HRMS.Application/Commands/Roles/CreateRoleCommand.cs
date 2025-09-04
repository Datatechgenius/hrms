using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Roles
{
    public class CreateRoleCommand : IRequest<Guid>
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
    }
}
