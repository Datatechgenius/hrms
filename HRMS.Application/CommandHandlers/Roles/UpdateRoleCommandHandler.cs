using HRMS.Application.Commands.Roles;
using HRMS.Domain.Entities.Roles;
using HRMS.Infrastructure.Interfaces.Roles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Roles
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
           var role = new RoleModel
           {
               Id = request.Id,
               OrganizationId = request.OrganizationId,
               Name = request.Name,
               Description = request.Description,
               IsSystemRole = request.IsSystemRole,
               UpdatedAt = DateTime.UtcNow
           };
            await _roleRepository.UpdateRoleAsync(role);
        }
    }
}
