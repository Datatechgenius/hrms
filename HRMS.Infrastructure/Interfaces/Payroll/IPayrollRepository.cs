using HRMS.Domain.Entities.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Payroll
{
    public interface IPayrollRepository
    {
        Task CreatePayrollAsync(PayrollModel payroll);
        Task<PayrollModel> GetPayrollByIdAsync(Guid id);
        Task<List<PayrollModel>> GetAllPayrollByOrgId(Guid organizationId);
        Task<List<PayrollModel>> GetAllPayrollByComId(Guid companyId);
        Task UpdatePayrollAsync(PayrollModel payroll);
        Task<bool> DeletePayrollAsync(Guid id);
        //Task<bool> IsPayrollExistsAsync(Guid id);
        //Task<IEnumerable<PayrollModel>> GetPayrollsByMonthAsync(DateTime month, Guid organizationId, Guid companyId);
    }
}
