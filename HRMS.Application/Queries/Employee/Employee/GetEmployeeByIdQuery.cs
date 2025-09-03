using HRMS.Domain.Entities;
using MediatR;
using System;

namespace HRMS.Application.Queries.Employee
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeModel>
    {
        public Guid Id { get; set; }

        public GetEmployeeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}