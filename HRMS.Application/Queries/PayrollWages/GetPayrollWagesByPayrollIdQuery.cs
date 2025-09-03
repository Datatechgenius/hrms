using HRMS.Domain.Entities.PayrollWages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayrollWages
{
    public class GetPayrollWagesByPayrollIdQuery : IRequest<List<PayrollWagesModel>>
    {
        public Guid PayrollId { get; set; }
        public GetPayrollWagesByPayrollIdQuery(Guid payrollId)
        {
            PayrollId = payrollId;
        }
    }
}
