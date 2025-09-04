using MediatR;
using System;

namespace HRMS.Application.Commands.Leave
{
    public class DeleteLeaveTypesCommand  : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteLeaveTypesCommand(Guid id)
        {
            Id = id;
        }
    }
}
