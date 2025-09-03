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
   public class CreateDivisionCommandHandler : IRequestHandler<CreateDivisionCommand, Guid>
{
    private readonly IDivisionRepository _repository;

    public CreateDivisionCommandHandler(IDivisionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateDivisionCommand request, CancellationToken cancellationToken)
    {
        var division = new Division
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OrganizationId = request.OrganizationId,
            IsActive = request.IsActive,
            DivisionCode = request.DivisionCode,
            ShortName = request.ShortName,
            Description = request.Description,
            Email = request.Email,
            Phone = request.Phone,
            Timezone = request.Timezone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(division);
        return division.Id;
    }
}
}
