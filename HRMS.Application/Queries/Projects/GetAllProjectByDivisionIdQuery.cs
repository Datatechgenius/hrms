using HRMS.Domain.Entities.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Projects
{
    public class GetAllProjectByDivisionIdQuery : IRequest<List<ProjectsModel>>
    {
        public Guid DivisionId { get; set; }
        public GetAllProjectByDivisionIdQuery(Guid divisionId)
        {
            DivisionId = divisionId;
        }
    }
   
}
