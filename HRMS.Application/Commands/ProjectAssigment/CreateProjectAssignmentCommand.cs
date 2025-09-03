using HRMS.Domain.Entities.ProjectAssigment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.ProjectAssigment
{
    public class CreateProjectAssignmentCommand : IRequest<Guid>
    {
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
