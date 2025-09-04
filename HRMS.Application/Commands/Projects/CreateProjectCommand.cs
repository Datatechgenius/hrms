using HRMS.Domain.Entities.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Projects
{
    public class CreateProjectCommand : IRequest<Guid>
    {
        public Guid OrganizationId { get; set; }
        public Guid? DivisionId { get; set; }
        public Guid? CompanyId { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatusEnum Status { get; set; }
    }
}
