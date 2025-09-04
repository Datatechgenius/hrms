using HRMS.Application.Commands.Company;
using HRMS.Application.Commands.Location;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Location
{
    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
    {
        private readonly ILocationRepository _repository;

        public DeleteLocationCommandHandler(ILocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
           return await _repository.DeleteLocationAsync(request.Id);
        }
    }
}
