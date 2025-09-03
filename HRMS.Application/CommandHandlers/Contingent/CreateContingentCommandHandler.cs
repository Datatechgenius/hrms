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
    public class CreateContingentCommandHandler : IRequestHandler<CreateContingentCommand, Guid>
    {
        private readonly IContingentRepository _repository;

        public CreateContingentCommandHandler(IContingentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateContingentCommand request, CancellationToken cancellationToken)
        {
            var contingent = new ContingentDto
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                isBillable = request.isBillable,
                isActive = request.isActive,
                CreatedAt = DateTime.UtcNow,
            };

            await _repository.AddContingentAsync(contingent);
            return contingent.Id;
        }
    }
}
