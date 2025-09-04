using HRMS.Application.Commands.Timesheet;
using HRMS.Infrastructure.Interfaces.Timesheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Timesheet
{
    public class DeleteTimesheetCommandHandler : IRequestHandler<DeleteTimesheetCommand, bool>
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public DeleteTimesheetCommandHandler(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        public async Task<bool> Handle(DeleteTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = await _timesheetRepository.GetTimesheetByIdAsync(request.Id);
            if (timesheet == null)
            {
                throw new KeyNotFoundException($"Timesheet with ID {request.Id} not found.");
            }
            return await _timesheetRepository.DeleteTimesheetAsync(request.Id);
        }
    }
}