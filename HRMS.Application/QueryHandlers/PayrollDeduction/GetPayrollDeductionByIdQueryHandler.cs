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
    public class GetPayrollDeductionByIdQueryHandler : IRequestHandler<GetPayrollDeductionByIdQuery, PayrollDeductionModel>
    {
        private readonly IPayrollDeductionRepository _payrollDeductionRepository;
        public GetPayrollDeductionByIdQueryHandler(IPayrollDeductionRepository payrollDeductionRepository)
        {
            _payrollDeductionRepository = payrollDeductionRepository;
        }

        public async Task<PayrollDeductionModel> Handle(GetPayrollDeductionByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var payrollDeduction = await _payrollDeductionRepository.GetPayrollDeductionByIdAsync(request.Id);
            return payrollDeduction;
        }
    }
}
