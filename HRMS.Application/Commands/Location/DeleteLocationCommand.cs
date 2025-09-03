using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Location
{
    public class DeleteLocationCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteLocationCommand(Guid id)
        {
            Id = id;
        }   
    }
}
