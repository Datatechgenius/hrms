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
    public class UpdatePayrollDeductionCommandHandler : IRequestHandler<UpdatePayrollDeductionCommand>
    {
        private readonly IPayrollDeductionRepository _payrollDeductionRepository;
        public UpdatePayrollDeductionCommandHandler(IPayrollDeductionRepository payrollDeductionRepository)
        {
            _payrollDeductionRepository = payrollDeductionRepository;
        }

        public async Task Handle(UpdatePayrollDeductionCommand request, CancellationToken cancellationToken)
        {
            var payrollDeduction = new PayrollDeductionModel
            {
                Id = request.Id,
                PayrollId = request.PayrollId,
                ComponentName = request.ComponentName,
                Amount = request.Amount
            };
            await _payrollDeductionRepository.UpdatePayrollDeductionAsync(payrollDeduction);
        }
    }
}
