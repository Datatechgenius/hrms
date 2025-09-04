using HRMS.Application.Commands.Organization;
using HRMS.Infrastructure.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Organization
{
       public class DeleteOrganizationHandler 
        : IRequestHandler<DeleteOrganizationCommand, bool>
    {
        private readonly IOrganizationRepository _repo;

        public DeleteOrganizationHandler(IOrganizationRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteOrganizationCommand cmd, 
                                       CancellationToken ct)
        {
            return await _repo.DeleteAsync(cmd.Id);
        }
    }

}
