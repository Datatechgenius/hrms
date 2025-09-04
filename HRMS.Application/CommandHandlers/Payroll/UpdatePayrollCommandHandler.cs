using HRMS.Application.Commands.Payroll;
using HRMS.Domain.Entities.Payroll;
using HRMS.Infrastructure.Interfaces.Payroll;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Payroll
{
    public class UpdatePayrollCommandHandler : IRequestHandler<UpdatePayrollCommand>
    {
        private readonly IPayrollRepository _payrollRepository;

        public UpdatePayrollCommandHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }
        public async Task Handle(UpdatePayrollCommand request, CancellationToken cancellationToken)
        {
            var payroll = new PayrollModel
            {
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                CompanyId = request.CompanyId,
                PayrollMonth = request.PayrollMonth,
                PayrollName = request.PayrollName,
                Status = request.Status,
                ProcessedBy = request.ProcessedBy,
                ProcessedDate = request.ProcessedDate,
                TotalEmployees = request.TotalEmployees,
                TotalWages = request.TotalWages,
                TotalDeductions = request.TotalDeductions,
                TotalNetPay = request.TotalNetPay,
                PayDate = request.PayDate,
                Remarks = request.Remarks,
                IsReversal = request.IsReversal,
                OriginalPayrollId = request.OriginalPayrollId,
                UpdatedAt = DateTime.UtcNow
            };
            await _payrollRepository.UpdatePayrollAsync(payroll);
        }
    }
}
