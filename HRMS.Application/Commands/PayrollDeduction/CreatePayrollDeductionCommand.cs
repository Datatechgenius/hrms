using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.PayrollDeduction
{
    public class CreatePayrollDeductionCommand : IRequest<Guid>
    {
        public Guid PayrollId { get; set; }
        public string ComponentName { get; set; }
        public decimal Amount { get; set; }

    }
}
