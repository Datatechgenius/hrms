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
    public class GetPayrollsByOrgIdQueryHandler : IRequestHandler<GetPayrollsByOrgIdQuery, List<PayrollModel>>
    {
        private readonly IPayrollRepository _payrollRepository;
        public GetPayrollsByOrgIdQueryHandler(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public async Task<List<PayrollModel>> Handle(GetPayrollsByOrgIdQuery request, CancellationToken cancellationToken)
        {
            var payrolls = await _payrollRepository.GetAllPayrollByOrgId(request.OrganizationId);
            return payrolls.ToList();
        }
    }
}
