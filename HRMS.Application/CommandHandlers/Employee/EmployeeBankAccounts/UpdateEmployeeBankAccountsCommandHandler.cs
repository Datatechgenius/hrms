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
    public class UpdateEmployeeBankAccountsCommandHandler : IRequestHandler<UpdateEmployeeBankAccountsCommand>
    {
        private readonly IEmployeeBankAccountsRepository _repo;

        public UpdateEmployeeBankAccountsCommandHandler(IEmployeeBankAccountsRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(UpdateEmployeeBankAccountsCommand request, CancellationToken cancellationToken)
        {
            var existing = new EmployeeBankAccounts
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                AccountType = request.AccountType,
                IsPrimary = request.IsPrimary,
                BankName = request.BankName,
                AccountNumber = request.AccountNumber,
                AccountHolderName = request.AccountHolderName,
                BranchName = request.BranchName,
                IfscCode = request.IfscCode,
                SwiftCode = request.SwiftCode,
                RoutingNumber = request.RoutingNumber,
                MicrCode = request.MicrCode,
                CountryCode = request.CountryCode,
                CurrencyCode = request.CurrencyCode,
                EffectiveFrom = request.EffectiveFrom,
                EffectiveTo = request.EffectiveTo,
                IsVerified = request.IsVerified,
                Notes = request.Notes,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.UpdateEmployeeBankAccountsAsync(existing);
        }
    }
}