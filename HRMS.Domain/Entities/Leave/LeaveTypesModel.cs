using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.Leave
{
    public class LeaveTypesModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsPaid { get; set; }
        public bool CarryForward { get; set; }
        public int MaxDaysPerYear { get; set; }
        public int MaxDaysPerMonth { get; set; }
        public bool RequiresApproval { get; set; }
        public bool IsEncashable { get; set; }
        public int GenderRestriction { get; set; }
        public int MaritalRestriction { get; set; }
        public int StartMonth { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
