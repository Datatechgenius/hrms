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
    public class GetAllAttendanceByEmpIdQueryHandler : IRequestHandler<GetAllAttendanceByEmpIdQuery, List<AttendanceModel>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public GetAllAttendanceByEmpIdQueryHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<List<AttendanceModel>> Handle(GetAllAttendanceByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var attendances = await _attendanceRepository.GetAllAttendanceByEmployeeIdAsync(request.EmployeeId);
            if (attendances == null || !attendances.Any())
            {
                throw new KeyNotFoundException($"No attendance records found for Employee ID {request.EmployeeId}.");
            }
            return attendances;
        }
    }
}
