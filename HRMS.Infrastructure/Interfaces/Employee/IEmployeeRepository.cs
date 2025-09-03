using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(EmployeeModel emp);
        Task<EmployeeModel> GetEmployeeByIdAsync(Guid id);
        Task UpdateEmployeeAsync(EmployeeModel emp);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<List<EmployeeModel>> GetAllEmployeesByOrgAsync(Guid orgId);
        Task<List<EmployeeModel>> GetAllEmployeesByDivAsync(Guid orgId);
        Task<List<EmployeeModel>> GetAllEmployeesByCompAsync(Guid orgId);
    }
}
