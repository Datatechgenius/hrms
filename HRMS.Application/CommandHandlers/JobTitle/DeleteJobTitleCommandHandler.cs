using HRMS.Application.Commands.JobTitle;
using HRMS.Infrastructure.Interfaces.JobTitle;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.JobTitle
{
    public class DeleteJobTitleCommandHandler : IRequestHandler<DeleteJobTitleCommand, bool>
    {
        private readonly IJobTitleRepository _repo;

        public DeleteJobTitleCommandHandler(IJobTitleRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteJobTitleCommand cmd, CancellationToken ct)
        {
            return await _repo.DeleteAsync(cmd.Id);
        }
    }

}
