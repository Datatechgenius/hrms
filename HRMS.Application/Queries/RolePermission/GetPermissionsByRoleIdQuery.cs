using HRMS.Domain.Entities.RolePermission;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.RolePermission
{
    public class GetPermissionsByRoleIdQuery : IRequest<List<RolePermissionModel>>
    {
        public Guid RoleId { get; }
        public GetPermissionsByRoleIdQuery(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty", nameof(roleId));
            }
            RoleId = roleId;
        }
    }
}
