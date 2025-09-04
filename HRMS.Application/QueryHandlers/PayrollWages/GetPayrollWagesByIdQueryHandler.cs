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
    public class GetPayrollWagesByIdQueryHandler : IRequestHandler<GetPayrollWagesByIdQuery , PayrollWagesModel>
    {
        private readonly IPayrollWagesRepository _repository;
        public GetPayrollWagesByIdQueryHandler(IPayrollWagesRepository repository)
        {
            _repository = repository;
        }

        public async Task<PayrollWagesModel> Handle(GetPayrollWagesByIdQuery request, CancellationToken cancellationToken)
        {
            var payrollWages = await _repository.GetPayrollWagesByIdAsync(request.Id);
            return payrollWages;
        }
    }
}
