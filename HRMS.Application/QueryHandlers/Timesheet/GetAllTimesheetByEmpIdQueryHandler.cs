using HRMS.Application.Queries.Timesheet;
using HRMS.Domain.Entities.Timesheet;
using HRMS.Infrastructure.Interfaces.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers
{
    public class GetAllTimesheetByEmpIdQueryHandler : IRequestHandler<GetAllTimesheetByEmpIdQuery, List<TimesheetModel>>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        public GetAllTimesheetByEmpIdQueryHandler(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        public async Task<List<TimesheetModel>> Handle(GetAllTimesheetByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var timesheets = await _timesheetRepository.GetAllTimesheetsByEmployeeIdAsync(request.EmployeeId);
            if (timesheets == null || !timesheets.Any())
            {
                throw new KeyNotFoundException($"No timesheets found for employee ID {request.EmployeeId}.");
            }
            return timesheets;
        }
    }
}
