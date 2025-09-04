using HRMS.Domain.Entities.PayrollDeduction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayrollDeduction
{
    public class GetAllPayrollDeductionByPayrollIdQuery : IRequest<List<PayrollDeductionModel>>
    {
        public Guid PayrollId { get; set; }
        public GetAllPayrollDeductionByPayrollIdQuery(Guid payrollId)
        {
            PayrollId = payrollId;
        }
    }
}
