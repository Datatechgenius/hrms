using HRMS.Application.Commands.Payroll;
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
    public class DeletePayrollCommandHandler : IRequestHandler<DeletePayrollCommand, bool>
    {
        private readonly IPayrollRepository _payrollRepository;
        public DeletePayrollCommandHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }
        public async Task<bool> Handle(DeletePayrollCommand request, CancellationToken cancellationToken)
        {
            return await _payrollRepository.DeletePayrollAsync(request.Id);
        }
    }
}
