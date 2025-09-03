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
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
    {
        private readonly IRoleRepository _roleRepository;
        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = new RoleModel
            {
                Id = Guid.NewGuid(), 
                OrganizationId = request.OrganizationId,
                Name = request.Name,
                Description = request.Description,
                IsSystemRole = request.IsSystemRole,
                CreatedAt = DateTime.UtcNow
            };
            await _roleRepository.CreateRoleAsync(role);
            return role.Id;
        }
    }
}
