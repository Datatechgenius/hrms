using HRMS.Domain.Entities.PayslipHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.PayslipHistory
{
    public interface IPayslipHistoryRepository 
    {
        Task CreatePayslipHistoryAsync(PaySlipHistoryModel payslipHistory);
        Task<PaySlipHistoryModel> GetPayslipHistoryByIdAsync(Guid id);
        Task<List<PaySlipHistoryModel>> GetAllPayslipHistoryByEmpIdAsync(Guid employeeId);
        Task UpdatePayslipHistoryAsync(PaySlipHistoryModel payslipHistory);
        Task<bool> DeletePayslipHistoryAsync(Guid id);
    }
}
