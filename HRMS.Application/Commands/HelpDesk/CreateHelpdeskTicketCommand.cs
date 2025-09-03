using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.HelpDesk
{
    public class CreateHelpdeskTicketCommand : IRequest<Guid>
    {
        public Guid OrganizationId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public Guid? AssignedTo { get; set; }
        public string Resolution { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime? SlaDueDate { get; set; }
        public string Channel { get; set; }
        public string AttachmentsUrl { get; set; }
    }
}
