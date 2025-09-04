using HRMS.Domain.Entities.Designations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Designations
{
    public class GetDesignationByIdQuery : IRequest<GetDesignationModel>
    {
        public Guid Id { get; }
        public GetDesignationByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
