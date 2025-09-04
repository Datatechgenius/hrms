using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Employee
{
    public class GetEmployeeContactByIdQueryHandler : IRequestHandler<GetEmployeeContactByIdQuery, EmployeeContacts>
    {
        private readonly IEmployeeContactsRepository _repo;

        public GetEmployeeContactByIdQueryHandler(IEmployeeContactsRepository repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeContacts> Handle(GetEmployeeContactByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await _repo.GetContactByIdAsync(request.Id);
            if (model == null) return null;

            return new EmployeeContacts
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
            };
        }
    }
}