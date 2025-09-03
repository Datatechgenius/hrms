using HRMS.Application.Commands.PayrollWages;
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
    public class DeletePayrollWagesCommandHandler : IRequestHandler<DeletePayrollWagesCommand, bool>
    {
        private readonly IPayrollWagesRepository _repository;
        public DeletePayrollWagesCommandHandler(IPayrollWagesRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeletePayrollWagesCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeletePayrollWagesAsync(request.Id);
        }
    }
}
