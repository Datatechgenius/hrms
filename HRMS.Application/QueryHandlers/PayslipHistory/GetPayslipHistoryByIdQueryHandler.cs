using HRMS.Application.Queries.PayslipHistory;
using HRMS.Domain.Entities.PayslipHistory;
using HRMS.Infrastructure.Interfaces.PayslipHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.PayslipHistory
{
    public class GetPayslipHistoryByIdQueryHandler : IRequestHandler<GetPayslipHistoryByIdQuery, PaySlipHistoryModel>
    {
        private readonly IPayslipHistoryRepository _payslipHistoryRepository;
        public GetPayslipHistoryByIdQueryHandler(IPayslipHistoryRepository payslipHistoryRepository)
        {
            _payslipHistoryRepository = payslipHistoryRepository;
        }
        public async Task<PaySlipHistoryModel> Handle(GetPayslipHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _payslipHistoryRepository.GetPayslipHistoryByIdAsync(request.Id);
        }
    }
}
