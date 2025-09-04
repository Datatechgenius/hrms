using HRMS.Application.Commands.PayrollWages;
using HRMS.Domain.Entities.PayrollWages;
using HRMS.Infrastructure.Interfaces.PayrollWages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.PayrollWages
{
    public class CreatePayrollWagesCommandHandler : IRequestHandler<CreatePayrollWagesCommand, Guid>
    {
        private readonly IPayrollWagesRepository _repository;
        public CreatePayrollWagesCommandHandler(IPayrollWagesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> Handle(CreatePayrollWagesCommand request, CancellationToken cancellationToken)
        {
            var newPayrollWages = new PayrollWagesModel
            {
                Id = Guid.NewGuid(),
                PayrollId = request.PayrollId,
                EmployeeId = request.EmployeeId,
                WageType = request.WageType,
                WageAmount = request.WageAmount,
                HoursWorked = request.HoursWorked,
                RatePerHour = request.RatePerHour,
                Taxable = request.Taxable,
                Remarks = request.Remarks,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.CreatePayrollWagesAsync(newPayrollWages);
            return newPayrollWages.Id;
        }
    }
}
