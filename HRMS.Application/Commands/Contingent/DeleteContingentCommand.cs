using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Contingent
{
    public class DeleteContingentCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteContingentCommand(Guid id)
        {
            Id = id;
        }
    }
}
