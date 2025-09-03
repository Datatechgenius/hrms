using HRMS.Application.Queries.Attendance;
using HRMS.Domain.Entities.Attendance;
using HRMS.Infrastructure.Interfaces.Attendance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Attendance
{
    public class GetAttendanceByIdQueryHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceModel>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public GetAttendanceByIdQueryHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<AttendanceModel> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(request.AttendanceId);
            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {request.AttendanceId} not found.");
            }
            return attendance;
        }
    }
}
