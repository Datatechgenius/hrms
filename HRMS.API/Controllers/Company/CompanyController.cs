using HRMS.API.Responses;
using HRMS.Application.Commands;
using HRMS.Application.Commands.Company;
using HRMS.Application.Commands.Divisions;
using HRMS.Application.Queries.Company;
using HRMS.Application.Queries.Divisions;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Divisions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Company
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommand command)
        {
            var id = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Company created successfully", new { id }));
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<StatusResponse<GetCompanyModel>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCompanyByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<GetCompanyModel>(false, "Company not found"));

            return Ok(new StatusResponse<GetCompanyModel>(true, "Company fetched successfully", result));
        }

        [HttpPost("AllCompaniesByOrg/{orgid}")]
        public async Task<ActionResult<List<GetCompanyModel>>> GetAllCompaniesByOrgId(Guid orgid)
        {
            var result = await _mediator.Send(new GetCompaniesByOrgIdQuery(orgid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<GetCompanyModel>>(false, "No Companies found for the organization"));

            return Ok(new StatusResponse<List<GetCompanyModel>>(true, "Companies fetched successfully", result));
        }


        [HttpPost("AllCompaniesByDiv/{divid}")]
        public async Task<ActionResult<List<GetCompanyModel>>> GetAllCompaniesByDivId(Guid divid)
        {
            var result = await _mediator.Send(new GetCompaniesByDivIdQuery(divid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<GetCompanyModel>>(false, "No Companies found for the division"));

            return Ok(new StatusResponse<List<GetCompanyModel>>(true, "Companies fetched successfully", result));
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCompanyCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched Company ID"));

            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Company updated successfully"));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteCompanyCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Company not found"));

            return Ok(new StatusResponse<object>(true, "Company deleted successfully"));
        }
    }
}
