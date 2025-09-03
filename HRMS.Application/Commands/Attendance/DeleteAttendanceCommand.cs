using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Attendance
{
    public class DeleteAttendanceCommand : IRequest<bool>   
    {
        public Guid AttendanceId { get; set; }
        public DeleteAttendanceCommand(Guid attendanceId)
        {
            AttendanceId = attendanceId;
        }
    }
}
