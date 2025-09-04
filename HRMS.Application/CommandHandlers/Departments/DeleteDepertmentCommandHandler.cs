using HRMS.Application.Commands.Departments;
using HRMS.Infrastructure.Interfaces.Departments;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Departments
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand , bool>
    {
        private readonly IDepartmentRepository _repository;

        public DeleteDepartmentCommandHandler(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
           return await _repository.DeleteAsync(request.Id);
        }
    }
}
