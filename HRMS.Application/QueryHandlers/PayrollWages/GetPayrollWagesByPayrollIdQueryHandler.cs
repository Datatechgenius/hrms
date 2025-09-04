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
    public class GetPayrollWagesByPayrollIdQueryHandler : IRequestHandler<GetPayrollWagesByPayrollIdQuery, List<PayrollWagesModel>>
    {
        private readonly IPayrollWagesRepository _repository;
        public GetPayrollWagesByPayrollIdQueryHandler(IPayrollWagesRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<PayrollWagesModel>> Handle(GetPayrollWagesByPayrollIdQuery request, CancellationToken cancellationToken)
        {
            var payrollWages = await _repository.GetPayrollWagesByPayrollIdAsync(request.PayrollId);
            return payrollWages;
        }
    }
}
