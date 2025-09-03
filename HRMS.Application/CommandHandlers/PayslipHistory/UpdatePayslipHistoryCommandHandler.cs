using HRMS.Application.Commands.PayslipHistory;
using HRMS.Infrastructure.Interfaces.PayslipHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.PayslipHistory
{
    public class UpdatePayslipHistoryCommandHandler
    {
        private readonly IPayslipHistoryRepository _payslipHistoryRepository;
        public UpdatePayslipHistoryCommandHandler(IPayslipHistoryRepository payslipHistoryRepository)
        {
            _payslipHistoryRepository = payslipHistoryRepository;
        }

        public async Task Handle(UpdatePayslipHistoryCommand command , CancellationToken cancellationToken)
        {
            var existingPayslipHistory = await _payslipHistoryRepository.GetPayslipHistoryByIdAsync(command.Id);
            if (existingPayslipHistory == null)
            {
                throw new Exception("Payslip history not found");
            }
     
            await _payslipHistoryRepository.UpdatePayslipHistoryAsync(existingPayslipHistory);
        }
    }
}
