using HRMS.Domain.Entities.Attendance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Attendance
{
    public class GetAllAttendanceByEmpIdQuery : IRequest<List<AttendanceModel>>
    {
        public Guid EmployeeId { get; set; }
        public GetAllAttendanceByEmpIdQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
