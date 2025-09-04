using HRMS.Application.Queries.Contingent;
using HRMS.Domain.Entities.Contingent;
using HRMS.Infrastructure.Interfaces.Contingent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Contingent
{
    public class GetContingentByIdQueryHandler : IRequestHandler<GetContingentByIdQuery, ContingentDto>
    {
        private readonly IContingentRepository _repository;

        public GetContingentByIdQueryHandler(IContingentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContingentDto> Handle(GetContingentByIdQuery request, CancellationToken cancellationToken)
        {
            var contingent = await _repository.GetContingentByIdAsync(request.Id);
            if (contingent == null) return null;

            return new ContingentDto
            {
                Id = contingent.Id,
                OrganizationId = contingent.OrganizationId,
                Name = contingent.Name,
                Code = contingent.Code,
                Description = contingent.Description,
                isBillable = contingent.isBillable,
                isActive = contingent.isActive,
                CreatedAt = contingent.CreatedAt,
                UpdatedAt = contingent.UpdatedAt
            };
        }
    }
}
