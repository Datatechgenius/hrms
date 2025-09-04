using HRMS.Application.Query.User;
using HRMS.Domain.Entities.User;
using HRMS.Infrastructure.Interfaces.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.User
{
    public class GetAllUsersQueryByOrgHandler : IRequestHandler<GetAllUsersQueryByOrg, List<UsersModel>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUsersQueryByOrgHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<UsersModel>> Handle(GetAllUsersQueryByOrg request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersByOrganizationAsync(request.OrganizationId);
            return users;
        }
    }
}
