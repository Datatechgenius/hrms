using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Company
{
    public class DeleteCompanyCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteCompanyCommand(Guid id)
        {
            Id = id;
        }
    }
}
