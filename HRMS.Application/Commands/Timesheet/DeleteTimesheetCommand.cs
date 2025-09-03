using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Timesheet
{
    public class DeleteTimesheetCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteTimesheetCommand(Guid id)
        {
            Id = id;
        }
    }
}
