using HRMS.Application.Commands.Roles;
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
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty", nameof(request.Id));
            }
            await _roleRepository.DeleteRoleAsync(request.Id);
        }   
    }
}
