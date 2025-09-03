using HRMS.Application.Commands.RolePermission;
using HRMS.Domain.Entities.RolePermission;
using HRMS.Infrastructure.Interfaces.RolePermission;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.RolePermission
{
    public class UpdateRolePermissionCommandHandler : IRequestHandler<UpdateRolePermissionCommand, Guid>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public UpdateRolePermissionCommandHandler(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<Guid> Handle(UpdateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rolePermission = new RolePermissionModel
            {
                Id = request.Id,
                RoleId = request.RoleId,
                Module = request.Module,
                Permission = request.Permission,
                UpdatedAt = DateTime.UtcNow
            };
            await _rolePermissionRepository.UpdateRolePermissionAsync(rolePermission);
            return rolePermission.Id;
        }
    }
}
