using HRMS.Domain.Entities.RolePermission;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.RolePermission
{
    public class GetRolePermissionByIdQuery : IRequest<RolePermissionModel>
    {
        public Guid Id { get; }
        public GetRolePermissionByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Role Permission ID cannot be empty", nameof(id));
            }
            Id = id;
        }
    }
}
