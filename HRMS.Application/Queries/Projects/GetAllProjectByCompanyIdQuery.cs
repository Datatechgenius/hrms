using HRMS.Domain.Entities.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Projects
{
    public class GetAllProjectByCompanyIdQuery : IRequest<List<ProjectsModel>>
    {
        public Guid CompanyId { get; set; }
        public GetAllProjectByCompanyIdQuery(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}
