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
    public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, Guid>
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public CreateAttendanceCommandHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<Guid> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
        {
            var attendance = new Domain.Entities.Attendance.AttendanceModel
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                OrganizationId = request.OrganizationId,
                AttendanceDate = request.AttendanceDate,
                CheckInTime = request.CheckInTime,
                CheckOutTime = request.CheckOutTime,
                TotalWorkedHours = request.TotalWorkedHours,
                AttendanceStatus = request.AttendanceStatus,
                Remarks = request.Remarks,
                Source = request.Source,
                GeoLatitude = request.GeoLatitude,
                GeoLongitude = request.GeoLongitude,
                ApprovalStatus = request.ApprovalStatus,
                ApprovedBy = request.ApprovedBy,
                ApprovalRemarks = request.ApprovalRemarks,
                CreatedAt = DateTime.UtcNow
            };
            await _attendanceRepository.CreateAttendanceAsync(attendance);
            return attendance.Id;
        }
    }
}
