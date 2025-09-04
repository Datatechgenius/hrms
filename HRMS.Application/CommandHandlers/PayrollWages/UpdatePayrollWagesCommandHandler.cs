using HRMS.Application.Commands.PayrollWages;
using HRMS.Domain.Entities.PayrollWages;
using HRMS.Infrastructure.Interfaces.PayrollWages;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.PayrollWages
{
    public class UpdatePayrollWagesCommandHandler : IRequestHandler<UpdatePayrollWagesCommand>
    {
        private readonly IPayrollWagesRepository _repository;
        public UpdatePayrollWagesCommandHandler(IPayrollWagesRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdatePayrollWagesCommand request, CancellationToken cancellationToken)
        {
            if (request == null) {
                throw new ArgumentNullException(nameof(request), "Request object cannot be null.");
            }

            var wages = new PayrollWagesModel
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                PayrollId = request.PayrollId,
                WageType = request.WageType,
                WageAmount = request.WageAmount,
                HoursWorked = request.HoursWorked,
                RatePerHour = request.RatePerHour,
                Taxable = request.Taxable,
                Remarks = request.Remarks,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.UpdatePayrollWagesAsync(wages);
        }
    }
}
