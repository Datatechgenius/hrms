using HRMS.Application.Commands.Designations;
using HRMS.Infrastructure.Interfaces.Designations;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Designations
{
    public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand , bool>
    {
        private readonly IDesignationRepository _repository;

        public DeleteDesignationCommandHandler(IDesignationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
        {
           return await _repository.DeleteDesignationAsync(request.Id);
        }
    }
}
