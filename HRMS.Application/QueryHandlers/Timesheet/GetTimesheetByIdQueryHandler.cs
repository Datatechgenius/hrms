using HRMS.Application.Queries.Timesheet;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Domain.Entities.Timesheet;
using HRMS.Infrastructure.Interfaces.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Timesheet
{
    public class GetTimesheetByIdQueryHandler : IRequestHandler<GetTimesheetByIdQuery, TimesheetModel>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        public GetTimesheetByIdQueryHandler(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        public async Task<TimesheetModel> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.GetTimesheetByIdAsync(request.Id);
            if (timesheet == null)
            {
                throw new KeyNotFoundException($"Timesheet with ID {request.Id} not found.");
            }
            return timesheet;
        }
    }
}
