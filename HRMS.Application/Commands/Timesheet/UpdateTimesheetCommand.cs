using HRMS.Domain.Entities.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands
{
    public class UpdateTimesheetCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? ProjectId { get; set; }
        public string TaskTitle { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string WorkType { get; set; }
        public string Description { get; set; }
        public TimeEntryStatus ApprovalStatus { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }
    }
}
