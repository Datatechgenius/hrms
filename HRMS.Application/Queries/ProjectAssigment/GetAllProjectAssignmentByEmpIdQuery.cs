using HRMS.Domain.Entities.ProjectAssigment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.ProjectAssigment
{
    public class GetAllProjectAssignmentByEmpIdQuery : IRequest<List<ProjectAssignmentModel>>
    {
        public Guid EmployeeId { get; set; }
        public GetAllProjectAssignmentByEmpIdQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
