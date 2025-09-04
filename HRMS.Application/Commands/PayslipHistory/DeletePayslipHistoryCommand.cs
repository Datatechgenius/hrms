using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.PayslipHistory
{
    public class DeletePayslipHistoryCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeletePayslipHistoryCommand(Guid id)
        {
            Id = id;
        }
    }
}
