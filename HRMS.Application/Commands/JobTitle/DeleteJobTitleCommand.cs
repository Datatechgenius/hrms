using MediatR;
using System;

namespace HRMS.Application.Commands.JobTitle
{
    public class DeleteJobTitleCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteJobTitleCommand(Guid id)
        {
            Id = id;
        }
    }
}
