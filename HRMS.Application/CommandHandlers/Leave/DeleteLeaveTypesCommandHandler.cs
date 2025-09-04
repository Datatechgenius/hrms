using HRMS.Application.Commands.Leave;
using HRMS.Infrastructure.Interfaces.Leave;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Leave
{
    public class DeleteLeaveTypesCommandHandler : IRequestHandler<DeleteLeaveTypesCommand, bool>
    {
        private readonly ILeaveTypesRepository _repo;
        public DeleteLeaveTypesCommandHandler(ILeaveTypesRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> Handle(DeleteLeaveTypesCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id);
        }
    }
}
