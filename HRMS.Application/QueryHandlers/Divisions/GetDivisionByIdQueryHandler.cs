using HRMS.Application.Queries.Divisions;
using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Divisions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Divisions
{
   public class GetDivisionByIdQueryHandler : IRequestHandler<GetDivisionByIdQuery, DivisionResponseDto>
    {
        private readonly IDivisionRepository _repository;

        public GetDivisionByIdQueryHandler(IDivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task<DivisionResponseDto> Handle(GetDivisionByIdQuery request, CancellationToken cancellationToken)
        {
            var division = await _repository.GetByIdAsync(request.Id);
            if (division == null) return null;

            return new DivisionResponseDto
            {
                Id = division.Id,
                Name = division.Name,
                IsActive = division.IsActive,
                DivisionCode = division.DivisionCode,
                Email = division.Email,
                Phone = division.Phone
            };
        }
    }
}
