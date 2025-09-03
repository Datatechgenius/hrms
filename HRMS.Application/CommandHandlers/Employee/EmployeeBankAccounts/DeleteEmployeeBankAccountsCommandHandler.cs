using HRMS.Application.Commands.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class DeleteEmployeeBankAccountsCommandHandler : IRequestHandler<DeleteEmployeeBankAccountsCommand, bool>
    {
        private readonly IEmployeeBankAccountsRepository _repo;

        public DeleteEmployeeBankAccountsCommandHandler(IEmployeeBankAccountsRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteEmployeeBankAccountsCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteEmployeeBankAccountsAsync(request.Id);
        }
    }
}