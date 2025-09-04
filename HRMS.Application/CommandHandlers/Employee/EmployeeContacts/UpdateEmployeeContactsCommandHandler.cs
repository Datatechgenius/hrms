using HRMS.Application.Commands.Employee;
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

namespace HRMS.Application.CommandHandlers.Employee
{
    public class UpdateEmployeeContactsCommandHandler : IRequestHandler<UpdateEmployeeContactsCommand>
    {
        private readonly IEmployeeContactsRepository _repo;

        public UpdateEmployeeContactsCommandHandler(IEmployeeContactsRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(UpdateEmployeeContactsCommand request, CancellationToken cancellationToken)
        {
            var existing = new EmployeeContacts
            {
                Id = request.Id,
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
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.UpdateEmployeeContactsAsync(existing);
        }
    }
}