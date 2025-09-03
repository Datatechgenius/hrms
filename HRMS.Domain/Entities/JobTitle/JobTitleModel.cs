using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.JobTitle
{
    public class JobTitleModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public int? Level { get; set; }
        public string JobCode { get; set; }
        public Guid? PayGradeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
