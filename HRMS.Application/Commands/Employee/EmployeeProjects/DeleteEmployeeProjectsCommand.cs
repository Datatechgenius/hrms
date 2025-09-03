using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Employee.EmployeeProject
{
    public class DeleteEmployeeProjectsCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public DeleteEmployeeProjectsCommand(Guid id)
        {
            Id = id;
        }
    }
}