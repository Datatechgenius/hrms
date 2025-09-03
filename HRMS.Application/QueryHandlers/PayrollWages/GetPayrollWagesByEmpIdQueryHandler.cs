using HRMS.Application.Queries.PayrollWages;
using HRMS.Domain.Entities.PayrollWages;
using HRMS.Infrastructure.Interfaces.PayrollWages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.PayrollWages
{
    public class GetPayrollWagesByEmpIdQueryHandler : IRequestHandler<GetPayrollWagesByEmpIdQuery, List<PayrollWagesModel>>
    {
        private readonly IPayrollWagesRepository _payrollWagesRepository;
        public GetPayrollWagesByEmpIdQueryHandler(IPayrollWagesRepository payrollWagesRepository)
        {
            _payrollWagesRepository = payrollWagesRepository;
        }
        public async Task<List<PayrollWagesModel>> Handle(GetPayrollWagesByEmpIdQuery request, CancellationToken cancellationToken)
        {
            return await _payrollWagesRepository.GetPayrollWagesByEmpIdAsync(request.EmployeeId);
        }
    }
}
