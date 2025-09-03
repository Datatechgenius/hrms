using HRMS.Domain.Entities.ProjectAssigment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.ProjectAssigment
{
    public class UpdateAssignmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Role { get; set; }
        public Guid? ReportingManagerId { get; set; }
        public int? AllocationPercent { get; set; }
        public TaskBillingTypeEnum? BillingType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
    }
}
