using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.PayrollWages
{
    public class DeletePayrollWagesCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeletePayrollWagesCommand(Guid id)
        {
            Id = id;
        }
    }
}
