using HRMS.Application.Commands;
using HRMS.Domain.Entities.Timesheet;
using HRMS.Infrastructure.Interfaces.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Timesheet
{
    public class UpdateTimesheetCommandHandler : IRequestHandler<UpdateTimesheetCommand>
    {
        private readonly ITimesheetRepository _timesheetRepository;
        public UpdateTimesheetCommandHandler(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        public async Task Handle(UpdateTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = new TimesheetModel
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                OrganizationId = request.OrganizationId,
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
                UpdatedAt = DateTime.UtcNow

            };
            await _timesheetRepository.UpdateTimesheetAsync(timesheet);
        }
    }
}
