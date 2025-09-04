using HRMS.API.Responses;
using HRMS.Application.Commands.ProjectAssigment;
using HRMS.Application.Queries.Organization;
using HRMS.Application.Queries.ProjectAssigment;
using HRMS.Application.Queries.Projects;
using HRMS.Domain.Entities.ProjectAssigment;
using HRMS.Domain.Entities.Projects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.ProjectAssigment
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectAssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectAssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectAssignmentCommand cmd)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));

            if (cmd != null)
            {
                var id = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Create), new { id }, new StatusResponse<Guid>(true, "ProjectAssignment created successfully", id));
            }
            else
            {
                return NotFound(new StatusResponse<object>(false, "ProjectAssignment creation faild"));
            }
        }
        [HttpGet("project/{assignmentId}")]
        public async Task<IActionResult> GetById(Guid assignmentId)
        {
            var query = new GetProjectAssignmentByIdQuery(assignmentId);
            var assignment = await _mediator.Send(query);
            if (assignment == null)
            {
                return NotFound(new StatusResponse<object>(false, "Project not found"));
            }
            return Ok(new StatusResponse<ProjectAssignmentModel>(true, "Project retrieved successfully", assignment));
        }

        [HttpPost]
        [Route("AllProjectAssignmentsByEmployee/{employeeId}")]
        public async Task<ActionResult<List<ProjectAssignmentModel>>> GetAllProjectAssignmentsByEmployeeId(Guid employeeId)
        {
            var result = await _mediator.Send(new GetAllProjectAssignmentByEmpIdQuery(employeeId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<ProjectAssignmentModel>>(false, "No Project Assignments found for the employee"));
            return Ok(new StatusResponse<List<ProjectAssignmentModel>>(true, "Project Assignments fetched successfully", result));
        }

        [HttpPut]
        [Route("UpdateProjectAssignment/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssignmentCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched Project Assignment ID"));
            var existingAssignment = await _mediator.Send(new GetProjectAssignmentByIdQuery(id));
            if (existingAssignment == null)
            {
                return NotFound(new StatusResponse<object>(false, "Project Assignment not found"));
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Project Assignment updated successfully"));
        }

        [HttpDelete]
        [Route("DeleteProjectAssignment/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetProjectAssignmentByIdQuery(id);
            var assignment = await _mediator.Send(query);
            if (assignment == null)
            {
                return NotFound(new StatusResponse<object>(false, "Project Assignment not found"));
            }
            await _mediator.Send(new DeleteAssignmentCommand(id));
            return Ok(new StatusResponse<object>(true, "Project Assignment deleted successfully"));
        }
    }
}