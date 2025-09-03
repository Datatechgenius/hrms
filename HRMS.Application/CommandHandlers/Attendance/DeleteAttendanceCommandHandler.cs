using HRMS.Application.Commands.Attendance;
using HRMS.Infrastructure.Interfaces.Attendance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Attendance
{
    public class DeleteAttendanceCommandHandler : IRequestHandler<DeleteAttendanceCommand, bool>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public DeleteAttendanceCommandHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<bool> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
        {
           return   await _attendanceRepository.DeleteAttendanceAsync(request.AttendanceId);
        }
    }
}
