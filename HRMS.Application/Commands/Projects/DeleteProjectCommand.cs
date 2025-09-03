using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Projects
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public Guid ProjectId { get; set; }
        public DeleteProjectCommand(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
