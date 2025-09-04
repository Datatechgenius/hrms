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
    public class CreateEmployeeBankAccountsCommandHandler : IRequestHandler<CreateEmployeeBankAccountsCommand, Guid>
    {
        private readonly IEmployeeBankAccountsRepository _repo;

        public CreateEmployeeBankAccountsCommandHandler(IEmployeeBankAccountsRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateEmployeeBankAccountsCommand request, CancellationToken cancellationToken)
        {
            var entity = new EmployeeBankAccounts
            {
                Id = Guid.NewGuid(),
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
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddEmployeeBankAccountsAsync(entity);
            return entity.Id;
        }
    }
}