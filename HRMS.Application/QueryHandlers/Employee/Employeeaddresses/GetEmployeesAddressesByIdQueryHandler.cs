using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using HRMS.Infrastructure.Repositories.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.EmployeeAddress
{
    public class GetEmployeeAddressesByEmployeeIdQueryHandler : IRequestHandler<GetEmployeeAddressesByIdQuery, EmployeeAddresses>
    {
        private readonly IEmployeeAddressesRepository _repo;

        public GetEmployeeAddressesByEmployeeIdQueryHandler(IEmployeeAddressesRepository repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeAddresses> Handle(GetEmployeeAddressesByIdQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.GetEmployeeAddressesByIdAsync(request.Id);

            if (entities == null)
                return null;


            return new EmployeeAddresses
            {
                Id = entities.Id,
                EmployeeId = entities.EmployeeId,
                AddressType = entities.AddressType,
                AddressLine1 = entities.AddressLine1,
                AddressLine2 = entities.AddressLine2,
                Landmark = entities.Landmark,
                City = entities.City,
                State = entities.State,
                ZipCode = entities.ZipCode,
                CountryCode = entities.CountryCode,
                IsPrimary = entities.IsPrimary,
                ValidFrom = entities.ValidFrom,
                ValidTo = entities.ValidTo,
                Phone = entities.Phone,
                Mobile = entities.Mobile,
                Email = entities.Email,
                CreatedAt = entities.CreatedAt,
                UpdateAt = entities.UpdateAt
            };

        }
    }
}
