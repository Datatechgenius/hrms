using HRMS.Application.Commands.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class DeleteEmployeeFamilyMembersCommandHandler : IRequestHandler<DeleteEmployeeFamilyMembersCommand, bool>
    {
        private readonly IEmployeeFamilyMembersRepository _repo;

        public DeleteEmployeeFamilyMembersCommandHandler(IEmployeeFamilyMembersRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteEmployeeFamilyMembersCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id);
        }
    }
}