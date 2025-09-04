using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Employee
{
    public interface IEmployeeContactsRepository
    {
        Task AddEmployeeContactsAsync(EmployeeContacts entity);
        Task<EmployeeContacts> GetContactByIdAsync(Guid id);
        Task<List<EmployeeContacts>> GetAllContactsByEmployeeIdAsync(Guid employeeId);
        Task UpdateEmployeeContactsAsync(EmployeeContacts entity);
        Task<bool> DeleteEmployeeContactsAsync(Guid id);
    }
}