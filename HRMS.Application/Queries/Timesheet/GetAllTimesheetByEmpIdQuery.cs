using HRMS.Domain.Entities.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Timesheet
{
    public class GetAllTimesheetByEmpIdQuery : IRequest<List<TimesheetModel>>
    {
        public Guid EmployeeId { get; set; }
        public GetAllTimesheetByEmpIdQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }   
}
