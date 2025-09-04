using HRMS.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Employee
{
    public interface IEmployeeAddressesRepository
    {
        Task AddEmployeeAddressesAsync(EmployeeAddresses entity);
        Task<EmployeeAddresses> GetEmployeeAddressesByIdAsync(Guid id);
        //Task<EmployeeAddresses> GetAllByEmployeeIdAsync(Guid id);
        Task UpdateEmployeeAddressesAsync(EmployeeAddresses entity);
        Task<bool> DeleteEmployeeAddressesAsync(Guid id);
        Task<List<EmployeeAddresses>> GetByEmployeeIdAsync(Guid employeeId);
    }
}
