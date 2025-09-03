using HRMS.Application.Commands.Company;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Divisions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Company
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand , bool>
    {
        private readonly ICompanyRepository _repository;

        public DeleteCompanyCommandHandler(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
           return await _repository.DeleteAsync(request.Id);
        }
    }
}
