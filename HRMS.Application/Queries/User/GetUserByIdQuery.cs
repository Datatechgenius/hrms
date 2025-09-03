using HRMS.Domain.Entities.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.User
{
    public class GetUserByIdQuery : IRequest<UsersModel>
    {
        public Guid UserId { get; set; }
        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
