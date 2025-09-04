using HRMS.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Employee
{
    public interface IEmployeeProjectsRepository
    {
        Task AddEmployeeProjectsAsync(EmployeeProjects entity);
        Task<EmployeeProjects> GetEmployeeProjectsByIdAsync(Guid id);
        Task<List<EmployeeProjects>> GetAllByEmployeeIdAsync(Guid employeeId);
        Task UpdateEmployeeProjectsAsync(EmployeeProjects entity);
        Task<bool> DeleteEmployeeProjectsAsync(Guid id);
    }

}