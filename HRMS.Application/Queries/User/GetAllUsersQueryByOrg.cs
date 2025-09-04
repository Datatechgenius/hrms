using HRMS.Domain.Entities.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Query.User
{
    public class GetAllUsersQueryByOrg : IRequest<List<UsersModel>>
    {
        public Guid OrganizationId { get; set; }
        public GetAllUsersQueryByOrg(Guid organizationId)
        {
            OrganizationId = organizationId;
        }
    }
}
