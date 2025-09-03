using HRMS.Application.Commands.Employee;
using HRMS.Application.Queries.Employee;
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
    public class GetEmployeeBankAccountsByIdQueryHandler : IRequestHandler<GetEmployeeBankAccountsByIdQuery, EmployeeBankAccounts>
    {
        private readonly IEmployeeBankAccountsRepository _repo;

        public GetEmployeeBankAccountsByIdQueryHandler(IEmployeeBankAccountsRepository repo)
        {
            _repo = repo;
        }
        public async Task<EmployeeBankAccounts> Handle(GetEmployeeBankAccountsByIdQuery request, CancellationToken cancellationToken)
        {
            var modelList = await _repo.GetEmployeeBankAccountsByIdAsync(request.Id);
            if (modelList == null) return new EmployeeBankAccounts();

            return modelList;   
        }
    }
}