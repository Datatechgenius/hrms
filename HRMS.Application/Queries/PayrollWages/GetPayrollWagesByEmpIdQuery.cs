using HRMS.Domain.Entities.PayrollWages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayrollWages
{
    public class GetPayrollWagesByEmpIdQuery : IRequest<List<PayrollWagesModel>>
    {
        public Guid EmployeeId { get; set; }
        public GetPayrollWagesByEmpIdQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
