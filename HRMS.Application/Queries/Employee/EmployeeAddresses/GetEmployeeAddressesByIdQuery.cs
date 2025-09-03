using HRMS.Domain.Entities.Employee;
using MediatR;
using System;

namespace HRMS.Application.Queries.Employee
{
    public class GetEmployeeAddressesByIdQuery : IRequest<EmployeeAddresses>
    {
        public Guid Id { get; set; }

        public GetEmployeeAddressesByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}