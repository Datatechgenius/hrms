using HRMS.Domain.Entities.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Roles
{
    public interface IRoleRepository
    {
        Task CreateRoleAsync(RoleModel role);
        Task<RoleModel> GetRoleByIdAsync(Guid roleId);
        Task<List<RoleModel>> GetAllRolesByOrganizationAsync(Guid orgId);
        Task UpdateRoleAsync(RoleModel role);

        Task<bool> DeleteRoleAsync(Guid roleId);
    }
}
