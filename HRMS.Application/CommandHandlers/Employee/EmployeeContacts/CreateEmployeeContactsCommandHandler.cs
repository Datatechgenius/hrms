    using HRMS.Application.Commands.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class CreateEmployeeContactsCommandHandler : IRequestHandler<CreateEmployeeContactsCommand , Guid>
    {
        private readonly IEmployeeContactsRepository _repo;

        public CreateEmployeeContactsCommandHandler(IEmployeeContactsRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateEmployeeContactsCommand request, CancellationToken cancellationToken)
        {
            var entity = new EmployeeContacts
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                ContactType = request.ContactType,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Relation = request.Relation,
                Mobile = request.Mobile,
                Phone = request.Phone,
                Email = request.Email,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                CountryCode = request.CountryCode,
                ZipCode = request.ZipCode,
                IsPrimary = request.IsPrimary,
                IsEmergency = request.IsEmergency,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddEmployeeContactsAsync(entity);
            return entity.Id;
        }
    }
}