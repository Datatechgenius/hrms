using HRMS.Application.Commands.Contingent;
using HRMS.Infrastructure.Interfaces.Contingent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Contingent
{
    public class DeleteContingentCommandHandler : IRequestHandler<DeleteContingentCommand, bool>
    {
        private readonly IContingentRepository _repository;

        public DeleteContingentCommandHandler(IContingentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteContingentCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteContingentAsync(request.Id);
        }
    }
}
