using HRMS.Application.Commands.PayslipHistory;
using HRMS.Infrastructure.Interfaces.PayrollDeduction;
using HRMS.Infrastructure.Interfaces.PayslipHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.PayslipHistory
{
    public class DeletePayslipHistoryCommandHandler : IRequestHandler<DeletePayslipHistoryCommand, bool>
    {
        private readonly IPayslipHistoryRepository _payslipHistoryRepository;

        public DeletePayslipHistoryCommandHandler(IPayslipHistoryRepository payslipHistoryRepository)
        {
            _payslipHistoryRepository = payslipHistoryRepository;
        }

        public async Task<bool> Handle(DeletePayslipHistoryCommand request, CancellationToken cancellationToken)
        {
            return await _payslipHistoryRepository.DeletePayslipHistoryAsync(request.Id);
        }
    }
}
