using HRMS.Application.Commands.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee.EmployeeAddress
{
    public class DeleteEmployeeAddressesCommandHandler : IRequestHandler<DeleteEmployeeAddressesCommand, bool>
    {
        private readonly IEmployeeAddressesRepository _repo;

        public DeleteEmployeeAddressesCommandHandler(IEmployeeAddressesRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteEmployeeAddressesCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteEmployeeAddressesAsync(request.Id);
        }
    }
}
