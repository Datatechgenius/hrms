using HRMS.Domain.Entities.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Departments
{
    public interface IDepartmentRepository
    {
        Task CreateDepartmentAsync(DepartmentModel department);

        Task<DepartmentModel> GetDepartmentByIdAsync(Guid id);

        Task<bool> DeleteAsync(Guid id);

        Task UpdateDepartmentAsync(DepartmentModel department);
        Task<List<DepartmentModel>> GetDepartmantsByCompanyIdAsync(Guid companyId);
    }
}
