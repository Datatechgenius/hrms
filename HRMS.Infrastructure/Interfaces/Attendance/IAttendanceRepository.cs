using HRMS.Domain.Entities.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Attendance
{
    public interface IAttendanceRepository
    {
        Task CreateAttendanceAsync(AttendanceModel attendance);

        Task UpdateAttendanceAsync(AttendanceModel attendanceModel);

        Task<bool> DeleteAttendanceAsync(Guid attendanceId);

        Task<AttendanceModel> GetAttendanceByIdAsync(Guid attendanceId);

        Task<List<AttendanceModel>> GetAllAttendanceByEmployeeIdAsync(Guid employeeId);
    }
}
