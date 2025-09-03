using HRMS.Domain.Entities.JobTitle;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.JobTitle
{
    public class UpdateJobTitleCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Level { get; set; }
        public string JobCode { get; set; } = string.Empty;
        public Guid? PayGradeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
