using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
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
    public class GetEmployeeIdByAddressesQueryHandler : IRequestHandler<GetAllAddressesByEmployeeIdQuery, List<EmployeeAddresses>>
    {
        private readonly IEmployeeAddressesRepository _repo;

        public GetEmployeeIdByAddressesQueryHandler(IEmployeeAddressesRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeAddresses>> Handle(GetAllAddressesByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByEmployeeIdAsync(request.Id);
        }
    }
}