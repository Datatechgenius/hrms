using HRMS.Application.Commands.RolePermission;
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
    public class DeleteRolePermissionCommandHandler : IRequestHandler<DeleteRolePermissionCommand>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public DeleteRolePermissionCommandHandler(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            await _rolePermissionRepository.DeleteAsync(request.Id);
        }
    }
}
