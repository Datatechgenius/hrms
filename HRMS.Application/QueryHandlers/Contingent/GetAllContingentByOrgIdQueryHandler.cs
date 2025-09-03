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
    public class GetAllContingentByOrgIdQueryHandler : IRequestHandler<GetAllContingentByOrgIdQuery, List<ContingentDto>>
    {
        private readonly IContingentRepository _repository;

        public GetAllContingentByOrgIdQueryHandler(IContingentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContingentDto>> Handle(GetAllContingentByOrgIdQuery request, CancellationToken cancellationToken)
        {
            var contingent = await _repository.GetAllContingentsByOrgIdAsync(request.Id);

            if (contingent == null || contingent.Count == 0)
                return new List<ContingentDto>();

            return contingent
                .Select(d => new ContingentDto
                {
                    Id = d.Id,
                    OrganizationId = d.OrganizationId,
                    Name = d.Name,
                    Code = d.Code,
                    Description = d.Description,
                    isBillable = d.isBillable,
                    isActive = d.isActive,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                })
                .ToList();
        }
    }
}