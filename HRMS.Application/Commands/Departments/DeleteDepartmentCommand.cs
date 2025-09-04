using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Departments
{
    public class DeleteDepartmentCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteDepartmentCommand(Guid id)
        {
            Id = id;
        }
    }
}
