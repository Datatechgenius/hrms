using HRMS.Application.Commands.Contingent;
using HRMS.Domain.Entities.Contingent;
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
    public class UpdateContingentCommandHandler : IRequestHandler<UpdateContingentCommand>
    {
        private readonly IContingentRepository _repository;

        public UpdateContingentCommandHandler(IContingentRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateContingentCommand request, CancellationToken cancellationToken)
        {
            var contingent = new ContingentDto
            {
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                isBillable = request.isBillable,
                isActive = request.isActive,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.UpdateContingentAsync(contingent);
        }
    }
}
