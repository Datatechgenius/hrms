using HRMS.Application.Queries.Roles;
using HRMS.Domain.Entities.Roles;
using HRMS.Infrastructure.Interfaces.Roles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Roles
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleModel>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleModel> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleByIdAsync(request.RoleId);
            return role;
        }
    }
}
