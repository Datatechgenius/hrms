using HRMS.Domain.Entities.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Projects
{
    public class GetProjectByIdQuery : IRequest<ProjectsModel>
    {
        public Guid ProjectId { get; set; }
        public GetProjectByIdQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
