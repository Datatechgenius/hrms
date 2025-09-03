using HRMS.Domain.Entities.PayrollDeduction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.PayrollDeduction
{
    public interface IPayrollDeductionRepository
    {
        Task CreatePayrollDeductionAsync(PayrollDeductionModel model);
        Task<PayrollDeductionModel> GetPayrollDeductionByIdAsync(Guid id);
        Task<List<PayrollDeductionModel>> GetAllPayrollDeductionByPayrollIdAsync(Guid payrollId);
        Task UpdatePayrollDeductionAsync(PayrollDeductionModel model);
        Task<bool> DeletePayrollDeductionAsync(Guid id);
    }
}
