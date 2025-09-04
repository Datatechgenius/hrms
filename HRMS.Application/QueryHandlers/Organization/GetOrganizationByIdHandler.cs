using HRMS.Application.Queries.Organization;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Organization
{
   public class GetOrganizationByIdHandler 
        : IRequestHandler<GetOrganizationByIdQuery, OrganizationModel>
    {
        private readonly IOrganizationRepository _repo;
        public GetOrganizationByIdHandler(IOrganizationRepository repo)
            => _repo = repo;

        public Task<OrganizationModel> Handle(
            GetOrganizationByIdQuery request, 
            CancellationToken cancellationToken)
            => _repo.GetByIdAsync(request.Id);
    }

}
