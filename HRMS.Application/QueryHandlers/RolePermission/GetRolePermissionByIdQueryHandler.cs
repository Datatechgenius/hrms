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

namespace HRMS.Application.QueryHandlers
{
    public class GetRolePermissionByIdQueryHandler : IRequestHandler<GetRolePermissionByIdQuery, RolePermissionModel>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public GetRolePermissionByIdQueryHandler(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<RolePermissionModel> Handle(GetRolePermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var rolePermission = await _rolePermissionRepository.GetPermissionsByIdAsync(request.Id);
            if (rolePermission == null)
            {
                throw new Exception($"RolePermission with ID {request.Id} not found.");
            }
            return new RolePermissionModel
            {
                Id = rolePermission.Id,
                RoleId = rolePermission.RoleId,
                Module = rolePermission.Module,
                Permission = rolePermission.Permission,
                CreatedAt = rolePermission.CreatedAt,
                UpdatedAt = rolePermission.UpdatedAt
            };
        }
    }
}
    