using MediatR;
using System;

namespace HRMS.Application.Commands.Employee
{
    public class DeleteEmployeeAddressesCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteEmployeeAddressesCommand(Guid id)
        {
            Id = id;
        }
    }
}