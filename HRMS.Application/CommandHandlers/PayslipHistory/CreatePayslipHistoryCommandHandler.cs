using HRMS.Application.Commands.PayslipHistory;
using HRMS.Domain.Entities.PayslipHistory;
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
    public class CreatePayslipHistoryCommandHandler : IRequestHandler<CreatePayslipHistoryCommand, Guid>
    {
        private readonly IPayslipHistoryRepository _payslipHistoryRepository;

        public CreatePayslipHistoryCommandHandler(IPayslipHistoryRepository payslipHistoryRepository)
        {
            _payslipHistoryRepository = payslipHistoryRepository;
        }

        public async Task<Guid> Handle(CreatePayslipHistoryCommand request, CancellationToken cancellationToken)
        {
            var payslipHistory = new PaySlipHistoryModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                EmployeeId = request.EmployeeId,
                PayrollId = request.PayrollId,
                Version = request.Version,
                PayPeriodStart = request.PayPeriodStart,
                PayPeriodEnd = request.PayPeriodEnd,
                GeneratedAt = request.GeneratedAt,
                GeneratedBy = request.GeneratedBy,
                PayslipUrl = request.PayslipUrl,
                DeliveryStatus = request.DeliveryStatus,
                DeliveredAt = request.DeliveredAt,
                Remarks = request.Remarks,
                CreatedAt = DateTime.UtcNow
            };
            await _payslipHistoryRepository.CreatePayslipHistoryAsync(payslipHistory);
            return payslipHistory.Id;
        }
    }
}
