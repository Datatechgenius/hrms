using HRMS.Domain.Entities.Attendance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Attendance
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceModel>
    {
        public Guid AttendanceId { get; set; }
        public GetAttendanceByIdQuery(Guid attendanceId)
        {
            AttendanceId = attendanceId;
        }
    }
}
