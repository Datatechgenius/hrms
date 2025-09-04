using HRMS.Application.Queries.Payroll;
using HRMS.Domain.Entities.Payroll;
using HRMS.Infrastructure.Interfaces.Payroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Payroll
{
    public class GetPayrollByIdQueryHandler : IRequestHandler<GetPayrollByIdQuery, PayrollModel>
    {
        private readonly IPayrollRepository _payrollRepository;

        public GetPayrollByIdQueryHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }
        public async Task<PayrollModel> Handle(GetPayrollByIdQuery request, CancellationToken cancellationToken)
        {
            var payroll = await _payrollRepository.GetPayrollByIdAsync(request.Id);

            if (payroll == null) {
                throw new Exception($"Payroll with ID {request.Id} not found.");
            }
            return payroll;
        }
    }
}
