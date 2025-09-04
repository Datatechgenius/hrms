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
    public class GetAllPayslipHistoryByEmpIdQueryHandler : IRequestHandler<GetAllPayslipHistoryByEmpIdQuery, List<PaySlipHistoryModel>>
    {
        private readonly IPayslipHistoryRepository _payslipHistoryRepository;
        public GetAllPayslipHistoryByEmpIdQueryHandler(IPayslipHistoryRepository payslipHistoryRepository)
        {
            _payslipHistoryRepository = payslipHistoryRepository;
        }
        public async Task<List<PaySlipHistoryModel>> Handle(GetAllPayslipHistoryByEmpIdQuery request, CancellationToken cancellationToken)
        {
            return await _payslipHistoryRepository.GetAllPayslipHistoryByEmpIdAsync(request.EmployeeId);
        }
    }
}
