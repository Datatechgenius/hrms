using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.HelpDeskComments
{
    public class CreateHelpDeskCommentCommand : IRequest<Guid>
    {
        public Guid TicketId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Comment { get; set; }
    }
}
