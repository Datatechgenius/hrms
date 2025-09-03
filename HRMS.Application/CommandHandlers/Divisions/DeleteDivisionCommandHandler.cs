using HRMS.Application.Commands.Company;
using HRMS.Infrastructure.Interfaces.Divisions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Divisions
{
    public class DeleteDivisionCommandHandler : IRequestHandler<DeleteCompanyCommand , bool>
    {
        private readonly IDivisionRepository _repository;

        public DeleteDivisionCommandHandler(IDivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
           return await _repository.DeleteAsync(request.Id);
        }
    }
}
