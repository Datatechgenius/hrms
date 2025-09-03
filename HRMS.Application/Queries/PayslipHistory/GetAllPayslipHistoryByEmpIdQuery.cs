using HRMS.Domain.Entities.PayslipHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayslipHistory
{
    public class GetAllPayslipHistoryByEmpIdQuery : IRequest<List<PaySlipHistoryModel>>
    {
        public Guid EmployeeId { get; set; }
        public GetAllPayslipHistoryByEmpIdQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
