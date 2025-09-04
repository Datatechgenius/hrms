using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Payroll
{
    public class DeletePayrollCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeletePayrollCommand(Guid id)
        {
            Id = id;
        }
    }
}
