using HRMS.Domain.Entities.Designations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Designations
{
    public class GetDesignationByDepartmentIdQuery : IRequest<List<DesignationModel>>
    {
         public Guid Id { get; set; }

        public GetDesignationByDepartmentIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
