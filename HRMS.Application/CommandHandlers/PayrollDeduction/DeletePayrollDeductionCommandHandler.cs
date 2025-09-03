using HRMS.Application.Commands.PayrollDeduction;
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
    public class DeletePayrollDeductionCommandHandler : IRequestHandler<DeletePayrollDeductionCommand, bool>
    {
        private readonly IPayrollDeductionRepository _payrollDeductionRepository;
        public DeletePayrollDeductionCommandHandler(IPayrollDeductionRepository payrollDeductionRepository)
        {
            _payrollDeductionRepository = payrollDeductionRepository;
        }
        public async Task<bool> Handle(DeletePayrollDeductionCommand request, CancellationToken cancellationToken)
        {
            return await _payrollDeductionRepository.DeletePayrollDeductionAsync(request.Id);
        }
    }
}
