using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Divisions
{
    public class DeleteDivisionCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteDivisionCommand(Guid id)
        {
            Id = id;
        }
    }
}
