using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee.EmployeeProject
{
    public class CreateEmployeeProjectsCommand : IRequest<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid ProjectId { get; set; }
        public string Role { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsBillable { get; set; }
        public int AllocationPercent { get; set; }
        public Guid ReportingManagerId { get; set; }
        public int Status { get; set; }
    }
}


