using HRMS.Application.Commands.Payroll;
using HRMS.Domain.Entities.Payroll;
using HRMS.Infrastructure.Interfaces.Payroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Payroll
{
    public class CreatePayrollCommandHandler : IRequestHandler<CreatePayrollCommand, Guid>
    {
        private readonly IPayrollRepository _payrollRepository;

        public CreatePayrollCommandHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<Guid> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
        {
            var payroll = new PayrollModel
            {
                Id = Guid.NewGuid(),
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
                CreatedAt = DateTime.UtcNow
            };
            await _payrollRepository.CreatePayrollAsync(payroll);
            return payroll.Id;
        }
    }
}
