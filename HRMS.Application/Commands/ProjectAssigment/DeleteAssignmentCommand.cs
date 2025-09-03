using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.ProjectAssigment
{
    public class DeleteAssignmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteAssignmentCommand(Guid id)
        {
            Id = id;
        }
    }
}
