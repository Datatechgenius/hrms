// File: HRMS.Application/QueryHandlers/Divisions/GetAllDivisionByIdQueryHandler.cs

using HRMS.Application.Queries.Divisions;
using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Divisions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Divisions
{
    public class GetAllDivisionByIdQueryHandler : IRequestHandler<GetAllDivisionByIdQuery, List<DivisionResponseDto>>
    {
        private readonly IDivisionRepository _repository;

        public GetAllDivisionByIdQueryHandler(IDivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DivisionResponseDto>> Handle(GetAllDivisionByIdQuery request,CancellationToken cancellationToken)
        {
            var divisions = await _repository.GetAllByIdAsync(request.Id);

            if (divisions == null || divisions.Count == 0)
                return new List<DivisionResponseDto>();

            return divisions
                .Select(d => new DivisionResponseDto
                {
                    Id           = d.Id,
                    Name         = d.Name,
                    IsActive     = d.IsActive,
                    DivisionCode = d.DivisionCode,
                    Email        = d.Email,
                    Phone        = d.Phone
                })
                .ToList();
        }
    }
}