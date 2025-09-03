using HRMS.Domain.Entities.Employee;
using MediatR;
using System;
using System.Collections.Generic;

namespace HRMS.Application.Queries.Employee
{
    public class GetAllAddressesByEmployeeIdQuery : IRequest<List<EmployeeAddresses>> 
    {
        public Guid Id { get; set; }

        public GetAllAddressesByEmployeeIdQuery(Guid id)
        {
            Id = id;
        }
    }
}