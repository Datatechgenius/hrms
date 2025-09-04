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
    public class CreateRolePermissionCommandHandler : IRequestHandler<CreateRolePermissionCommand, Guid>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public CreateRolePermissionCommandHandler(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<Guid> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            if (request.RoleId == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty", nameof(request.RoleId));
            }
            var rolePermission = new RolePermissionModel
            {
                Id = Guid.NewGuid(), 
                RoleId = request.RoleId,
                Module = request.Module,
                Permission = request.Permission,
                CreatedAt = DateTime.UtcNow,
            };
            await _rolePermissionRepository.CreateRolePermissionAsync(rolePermission);
            return rolePermission.Id; 
        }
    }
}
