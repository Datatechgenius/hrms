using HRMS.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Employee
{
    public interface IEmployeeBankAccountsRepository
    {
        Task AddEmployeeBankAccountsAsync(EmployeeBankAccounts entity);
        Task<EmployeeBankAccounts> GetEmployeeBankAccountsByIdAsync(Guid id);
        Task<List<EmployeeBankAccounts>> GetAllByEmployeeIdAsync(Guid employeeId);
        Task UpdateEmployeeBankAccountsAsync(EmployeeBankAccounts entity);
        Task<bool> DeleteEmployeeBankAccountsAsync(Guid id);
    }
}
