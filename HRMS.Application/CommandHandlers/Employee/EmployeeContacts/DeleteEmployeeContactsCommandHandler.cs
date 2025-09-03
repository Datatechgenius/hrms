using HRMS.Application.Commands.Employee;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class DeleteEmployeeContactsCommandHandler : IRequestHandler<DeleteEmployeeContactsCommand, bool>
    {
        private readonly IEmployeeContactsRepository _repo;

        public DeleteEmployeeContactsCommandHandler(IEmployeeContactsRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteEmployeeContactsCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteEmployeeContactsAsync(request.Id);
        }
    }
}