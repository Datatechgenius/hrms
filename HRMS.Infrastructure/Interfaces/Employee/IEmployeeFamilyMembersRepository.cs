using HRMS.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Employee
{
    public interface IEmployeeFamilyMembersRepository
    {
        Task AddAsync(EmployeeFamilyMembers entity);
        Task<EmployeeFamilyMembers> GetByIdAsync(Guid id);
        Task<List<EmployeeFamilyMembers>> GetAllByEmployeeIdAsync(Guid employeeId);
        Task UpdateAsync(EmployeeFamilyMembers entity);
        Task<bool> DeleteAsync(Guid id);
    }
}