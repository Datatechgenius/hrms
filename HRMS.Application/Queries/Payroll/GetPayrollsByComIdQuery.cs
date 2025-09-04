using HRMS.Domain.Entities.Payroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Payroll
{
    public class GetPayrollsByComIdQuery : IRequest<List<PayrollModel>>
    {
        public Guid CompanyId { get; set; }
        public GetPayrollsByComIdQuery(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}
