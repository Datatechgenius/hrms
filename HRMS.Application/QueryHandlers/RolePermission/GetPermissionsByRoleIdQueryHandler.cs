using HRMS.Application.Queries.RolePermission;
using HRMS.Domain.Entities.RolePermission;
using HRMS.Infrastructure.Interfaces.RolePermission;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.RolePermission
{
    public class GetPermissionsByRoleIdQueryHandler : IRequestHandler<GetPermissionsByRoleIdQuery, List<RolePermissionModel>>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public GetPermissionsByRoleIdQueryHandler(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<List<RolePermissionModel>> Handle(GetPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            return await _rolePermissionRepository.GetPermissionsByRoleIdAsync(request.RoleId);
        }
    }
}
