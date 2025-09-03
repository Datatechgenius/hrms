using HRMS.Application.Commands.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.ProjectAssigment
{
    public class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand, bool>
    {
        private readonly IProjectAssignmentRepository _repo;
        public DeleteAssignmentCommandHandler(IProjectAssignmentRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteProjectAssignmentAsync(request.Id);
        }
    }
}
