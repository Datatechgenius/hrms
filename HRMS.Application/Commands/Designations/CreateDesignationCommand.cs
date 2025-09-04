using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Designations
{
    public class CreateDesignationCommand : IRequest<Guid>
    {
        public Guid OrganizationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public int Level { get; set; }
        public bool IsBillable { get; set; }
        public bool Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
