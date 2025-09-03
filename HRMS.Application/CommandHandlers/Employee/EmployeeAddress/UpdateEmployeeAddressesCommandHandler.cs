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

namespace HRMS.Application.CommandHandlers.Employee.EmployeeAddress
{
    public class UpdateEmployeeAddressesCommandHandler : IRequestHandler<UpdateEmployeeAddressesCommand, Guid>
    {
        private readonly IEmployeeAddressesRepository _repo;

        public UpdateEmployeeAddressesCommandHandler(
            IEmployeeAddressesRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(UpdateEmployeeAddressesCommand request, CancellationToken cancellationToken)
        {
            var entity = new EmployeeAddresses
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                AddressType = request.AddressType,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                Landmark = request.Landmark,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                CountryCode = request.CountryCode,
                IsPrimary = request.IsPrimary,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo,
                Phone = request.Phone,
                Mobile = request.Mobile,
                Email = request.Email,
                UpdateAt = request.UpdateAt
            };

            await _repo.UpdateEmployeeAddressesAsync(entity);
            return entity.Id;
        }
    }
}
