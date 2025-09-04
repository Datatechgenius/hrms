using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.HelpDesk
{
    public class DeleteHelpdeskTicketCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteHelpdeskTicketCommand(Guid id)
        {
            Id = id;
        }
    }
}
