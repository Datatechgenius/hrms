using HRMS.Domain.Entities.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Projects
{
    public class GetAllProjectByOrganizationIdQuery : IRequest<List<ProjectsModel>>
    {
        public Guid OrganizationId { get; set; }
        public GetAllProjectByOrganizationIdQuery(Guid organizationId)
        {
            OrganizationId = organizationId;
        }
    }
}
