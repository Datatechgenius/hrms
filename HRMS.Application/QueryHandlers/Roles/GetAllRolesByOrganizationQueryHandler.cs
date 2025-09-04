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
    public class GetAllRolesByOrganizationQueryHandler : IRequestHandler<GetAllRolesByOrganizationQuery, List<RoleModel>>
    {
        private readonly IRoleRepository _roleRepository;
        public GetAllRolesByOrganizationQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository), "Role repository cannot be null");
        }
        public async Task<List<RoleModel>> Handle(GetAllRolesByOrganizationQuery request, CancellationToken cancellationToken)
        {
            if (request.OrganizationId == Guid.Empty)
            {
                throw new ArgumentException("Organization ID cannot be empty", nameof(request.OrganizationId));
            }
            var roles = await _roleRepository.GetAllRolesByOrganizationAsync(request.OrganizationId);
            return roles;
        }
    }
}
