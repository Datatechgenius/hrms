using HRMS.Application.Queries.PayrollDeduction;
using HRMS.Domain.Entities.PayrollDeduction;
using HRMS.Infrastructure.Interfaces.PayrollDeduction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.PayrollDeduction
{
    public class GetAllPayrollDeductionByPayrollIdQueryHandler : IRequestHandler<GetAllPayrollDeductionByPayrollIdQuery, List<PayrollDeductionModel>>
    {
        private readonly IPayrollDeductionRepository _payrollDeductionRepository;
        public GetAllPayrollDeductionByPayrollIdQueryHandler(IPayrollDeductionRepository payrollDeductionRepository)
        {
            _payrollDeductionRepository = payrollDeductionRepository;
        }
        public async Task<List<PayrollDeductionModel>> Handle(GetAllPayrollDeductionByPayrollIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var payrollDeductions = await _payrollDeductionRepository.GetAllPayrollDeductionByPayrollIdAsync(request.PayrollId);
            return payrollDeductions;
        }
    }
}
