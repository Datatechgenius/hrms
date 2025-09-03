using HRMS.Application.Commands.Divisions;
using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Divisions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Divisions
{
    public class UpdateDivisionCommandHandler : IRequestHandler<UpdateDivisionCommand>
    {
        private readonly IDivisionRepository _repository;

        public UpdateDivisionCommandHandler(IDivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateDivisionCommand request, CancellationToken cancellationToken)
        {
            var division = new Division
            {
                Id            = request.Id,
                Name          = request.Name,
                DivisionCode  = request.DivisionCode,
                ShortName     = request.ShortName,
                Active        = request.Active,
                Description   = request.Description,
                //HeadId        = request.HeadId,
                Email         = request.Email,
                Phone         = request.Phone,
                //LocationId    = request.LocationId,
                Timezone      = request.Timezone,
                IsActive      = request.IsActive,
                UpdatedAt     = DateTime.UtcNow
            };

            await _repository.UpdateAsync(division);
            //return true;
        }
    }
}
