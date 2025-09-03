using HRMS.Application.Commands.Timesheet;
using HRMS.Domain.Entities.Timesheet;
using HRMS.Infrastructure.Interfaces.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers
{
    public class CreateTimesheetCommandHandler : IRequestHandler<CreateTimesheetCommand, Guid>
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public CreateTimesheetCommandHandler(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        public async Task<Guid> Handle(CreateTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = new TimesheetModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                EmployeeId = request.EmployeeId,
                ProjectId = request.ProjectId,
                TaskTitle = request.TaskTitle,
                WorkDate = request.WorkDate,
                HoursWorked = request.HoursWorked,  
                WorkType = request.WorkType,
                Description = request.Description,
                ApprovalStatus = request.ApprovalStatus,
                ApprovedBy = request.ApprovedBy,
                ApprovedAt = request.ApprovedAt,
                RejectionReason = request.RejectionReason,
                CreatedAt = DateTime.UtcNow
            };
            await _timesheetRepository.CreateTimesheetAsync(timesheet);
            return timesheet.Id;
        }
    }
}
