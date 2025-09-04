using HRMS.Application.Commands.Employee;
using HRMS.Application.Commands.Employee.EmployeeProject;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee.EmployeeProject
{
    public class DeleteEmployeeProjectsCommandHandler : IRequestHandler<DeleteEmployeeProjectsCommand, bool>
    {
        private readonly IEmployeeProjectsRepository _repo;

        public DeleteEmployeeProjectsCommandHandler(IEmployeeProjectsRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteEmployeeProjectsCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteEmployeeProjectsAsync(request.Id);
        }
    }
}
