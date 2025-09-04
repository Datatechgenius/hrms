using HRMS.Domain.Entities.Leave;
using MediatR;
using System;
using System.Collections.Generic;

namespace HRMS.Application.Queries.Leave
{
     public class GetLeaveTypeByIdQuery : IRequest<LeaveTypesModel>
    {
        public Guid Id { get; set; }
        public GetLeaveTypeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
