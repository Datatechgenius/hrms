using HRMS.Application.Commands.PayrollDeduction;
using HRMS.Domain.Entities.PayrollDeduction;
using HRMS.Infrastructure.Interfaces.PayrollDeduction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.PayrollDeduction
{
    public class CreatePayrollDeductionCommandHandler : IRequestHandler<CreatePayrollDeductionCommand ,Guid>
    {
        private readonly IPayrollDeductionRepository _payrollDeduction;

        public CreatePayrollDeductionCommandHandler(IPayrollDeductionRepository payrollDeduction)
        {
             _payrollDeduction = payrollDeduction;
        }
        public async Task<Guid> Handle(CreatePayrollDeductionCommand request , CancellationToken cancellation)
        {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            var payroll = new PayrollDeductionModel
            {
                Id = Guid.NewGuid(),
                PayrollId = request.PayrollId,
                ComponentName = request.ComponentName,
                Amount = request.Amount
            };
            await _payrollDeduction.CreatePayrollDeductionAsync(payroll);
            return payroll.Id;
        }
    }
}
    