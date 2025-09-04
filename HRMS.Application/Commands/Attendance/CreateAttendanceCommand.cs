using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Attendance
{
    public class CreateAttendanceCommand : IRequest<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal? TotalWorkedHours { get; set; }
        public int AttendanceStatus { get; set; }
        public string Remarks { get; set; }
        public string Source { get; set; }
        public decimal? GeoLatitude { get; set; }
        public decimal? GeoLongitude { get; set; }
        public int? ApprovalStatus { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string ApprovalRemarks { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
