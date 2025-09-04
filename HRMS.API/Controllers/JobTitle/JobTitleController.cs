using HRMS.API.Responses;
using HRMS.Application.Commands.JobTitle;
using HRMS.Application.Queries.Departments;
using HRMS.Application.Queries.JobTitle;
using HRMS.Application.Queries.Organization;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Departments;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Infrastructure.Interfaces.JobTitle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.JobTitle
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobTitlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobTitlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _mediator.Send(new GetJobTitleByIdQuery(id));
             if (data == null)
                return NotFound(new StatusResponse<JobTitleModel>(false, "JobTitle not found"));

            return Ok(new StatusResponse<JobTitleModel>(true, "JobTitle fetched successfully", data));
        }

        [HttpPost("AllJobTitleByCompanyId/{comId}")]
        public async Task<ActionResult<List<JobTitleModel>>> GetAllJobTitleByCompanyId(Guid comId)
        {
            var result = await _mediator.Send(new GetJobTitleByCompanyIdQuery(comId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<JobTitleModel>>(false, "No JobTitle found for the Company"));

            return Ok(new StatusResponse<List<JobTitleModel>>(true, "JobTitle fetched successfully", result));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobTitleCommand cmd)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));

            try
            {
                var id = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Create), new { id }, new StatusResponse<Guid>(true, "Job title created successfully", id));
            }
            catch (Exception ex)
            {
                return BadRequest(new StatusResponse<object>(false, "An error occurred while creating the job title", ex.Message));
            }
            
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateJobTitleCommand cmd)
        {
           
            if (id != cmd.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched JobTitle ID"));

            await _mediator.Send(cmd);
            return Ok(new StatusResponse<object>(true, "JobTitle updated successfully"));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _mediator.Send(new DeleteJobTitleCommand(id));
             if (!ok)
                return NotFound(new StatusResponse<object>(false, "JobTitle not found"));

            return Ok(new StatusResponse<object>(true, "JobTitle deleted successfully"));
        }
    }

}
