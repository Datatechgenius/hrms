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
    public class GetPayrollsByComIdQueryHandler : IRequestHandler<GetPayrollsByComIdQuery, List<PayrollModel>>
    {
        private readonly IPayrollRepository _payrollRepository;
        public GetPayrollsByComIdQueryHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }
        public async Task<List<PayrollModel>> Handle(GetPayrollsByComIdQuery request, CancellationToken cancellationToken)
        {
            var payrolls = await _payrollRepository.GetAllPayrollByComId(request.CompanyId);
            return payrolls.ToList();
        }
    }
}
