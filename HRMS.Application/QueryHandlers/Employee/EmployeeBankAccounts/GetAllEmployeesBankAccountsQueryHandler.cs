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
    public class GetAllEmployeesBankAccountsQueryHandler : IRequestHandler<GetAllEmployeeBankAccountsByIdQuery, List<EmployeeBankAccounts>>
    {
        private readonly IEmployeeBankAccountsRepository _repo;

        public GetAllEmployeesBankAccountsQueryHandler(IEmployeeBankAccountsRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeBankAccounts>> Handle(GetAllEmployeeBankAccountsByIdQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllByEmployeeIdAsync(request.Id);
            return models
                .Select(model => new EmployeeBankAccounts
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    AccountType = model.AccountType,
                    IsPrimary = model.IsPrimary,
                    BankName = model.BankName,
                    AccountNumber = model.AccountNumber,
                    AccountHolderName = model.AccountHolderName,
                    BranchName = model.BranchName,
                    IfscCode = model.IfscCode,
                    SwiftCode = model.SwiftCode,
                    RoutingNumber = model.RoutingNumber,
                    MicrCode = model.MicrCode,
                    CountryCode = model.CountryCode,
                    CurrencyCode = model.CurrencyCode,
                    EffectiveFrom = model.EffectiveFrom,
                    EffectiveTo = model.EffectiveTo,
                    IsVerified = model.IsVerified,
                    Notes = model.Notes,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt

                })
                .ToList();
        }
    }
}