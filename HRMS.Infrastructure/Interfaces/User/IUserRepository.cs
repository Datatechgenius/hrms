using HRMS.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.User
{
    public interface IUserRepository
    {
        Task AddUserAsync(UsersModel user);
        Task<UsersModel> GetUserByIdAsync(Guid userId);
        Task<List<UsersModel>> GetAllUsersByOrganizationAsync(Guid organizationId);
        Task UpdateUserAsync(UsersModel user);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
