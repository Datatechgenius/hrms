using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.PayrollDeduction
{
    public class DeletePayrollDeductionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeletePayrollDeductionCommand(Guid id)
        {
            Id = id;
        }
    }
}
