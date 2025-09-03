using HRMS.Domain.Entities.Payroll;
using HRMS.Domain.Entities.PayrollWages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayrollWages
{
    public class GetPayrollWagesByIdQuery : IRequest<PayrollWagesModel>
    {
        public Guid Id { get; set; }
        public GetPayrollWagesByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
