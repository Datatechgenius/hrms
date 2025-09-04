using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Employee
{
    public class GetAllEmployeesByContactsQueryHandler : IRequestHandler<GetAllContactsByEmployeeIdQuery, List<EmployeeContacts>>
    {
        private readonly IEmployeeContactsRepository _repo;

        public GetAllEmployeesByContactsQueryHandler(IEmployeeContactsRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeContacts>> Handle(GetAllContactsByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllContactsByEmployeeIdAsync(request.Id);
            return models
                .Select(model => new EmployeeContacts
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    ContactType = model.ContactType,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Relation = model.Relation,
                    Mobile = model.Mobile,
                    Phone = model.Phone,
                    Email = model.Email,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    State = model.State,
                    CountryCode = model.CountryCode,
                    ZipCode = model.ZipCode,
                    IsPrimary = model.IsPrimary,
                    IsEmergency = model.IsEmergency,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt
                })
                .ToList();
        }
    }
}