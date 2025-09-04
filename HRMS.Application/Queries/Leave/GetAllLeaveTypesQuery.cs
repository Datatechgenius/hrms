using HRMS.Domain.Entities.Leave;
using MediatR;
using System;
using System.Collections.Generic;

namespace HRMS.Application.Queries.Leave
{
    public class GetAllLeaveTypesQuery : IRequest<List<LeaveTypesModel>>
    {
        public Guid LeaveTypesId { get; set; }

        public GetAllLeaveTypesQuery(Guid leavetypesId)
        {
            LeaveTypesId = leavetypesId;
        }
    }
}
