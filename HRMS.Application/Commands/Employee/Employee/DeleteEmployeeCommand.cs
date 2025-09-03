using MediatR;
using System;

namespace HRMS.Application.Commands.Employee
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteEmployeeCommand(Guid id)
        {
            Id = id;
        }
    }
}