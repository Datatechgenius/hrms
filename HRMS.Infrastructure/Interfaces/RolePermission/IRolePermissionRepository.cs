using HRMS.Domain.Entities.RolePermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.RolePermission
{
    public interface IRolePermissionRepository
    {
        Task CreateRolePermissionAsync(RolePermissionModel rolePermission);
        Task<RolePermissionModel> GetPermissionsByIdAsync(Guid Id);
        Task<List<RolePermissionModel>> GetPermissionsByRoleIdAsync(Guid orgId);
        Task UpdateRolePermissionAsync(RolePermissionModel rolePermission);
        Task<bool> DeleteAsync(Guid Id);
    }
}
