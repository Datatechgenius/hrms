using HRMS.Domain.Entities.Roles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Roles
{
    public class GetRoleByIdQuery : IRequest<RoleModel>
    {
        public Guid RoleId { get; set; }
        public GetRoleByIdQuery(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}
