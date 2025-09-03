using HRMS.Domain.Entities.Roles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Roles
{
    public class GetAllRolesByOrganizationQuery : IRequest<List<RoleModel>>
    {
        public Guid OrganizationId { get; }
        public GetAllRolesByOrganizationQuery(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
            {
                throw new ArgumentException("Organization ID cannot be empty", nameof(organizationId));
            }
            OrganizationId = organizationId;
        }
    }
}
