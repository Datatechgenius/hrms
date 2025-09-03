using HRMS.Domain.Entities.PayrollWages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.PayrollWages
{
    public interface IPayrollWagesRepository
    {
        Task CreatePayrollWagesAsync(PayrollWagesModel payrollWages);
        Task<PayrollWagesModel> GetPayrollWagesByIdAsync(Guid id);
        Task<List<PayrollWagesModel>> GetPayrollWagesByPayrollIdAsync(Guid id);
        Task<List<PayrollWagesModel>> GetPayrollWagesByEmpIdAsync(Guid id);
        Task UpdatePayrollWagesAsync(PayrollWagesModel payrollWages);
        Task<bool> DeletePayrollWagesAsync(Guid id);

    }
}
