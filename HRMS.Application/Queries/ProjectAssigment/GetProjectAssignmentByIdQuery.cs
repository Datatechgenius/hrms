using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Domain.Entities.Projects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.ProjectAssigment
{
    public class GetProjectAssignmentByIdQuery : IRequest<ProjectAssignmentModel>
    {
        public Guid Id { get; set; }
        public GetProjectAssignmentByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
