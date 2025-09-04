using HRMS.Domain.Entities.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Timesheet
{
    public class GetTimesheetByIdQuery : IRequest<TimesheetModel>
    {
        public Guid Id { get; set; }
        public GetTimesheetByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
