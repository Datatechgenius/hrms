using HRMS.API.Responses;
using HRMS.Application.Commands.Projects;
using HRMS.Application.Queries.Employee;
using HRMS.Application.Queries.Projects;
using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Projects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Projects
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProjectCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Project created successfully", new { id }));
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetById(Guid projectId)
        {
            var query = new GetProjectByIdQuery(projectId);
            var project = await _mediator.Send(query);
            if (project == null)
            {
                return NotFound(new StatusResponse<object>(false, "Project not found"));
            }
            return Ok(new StatusResponse<ProjectsModel>(true, "Project retrieved successfully", project));
        }
        [HttpPost("AllProjectsByComp/{id}")]
        public async Task<ActionResult<List<ProjectsModel>>> GetAllProjectsByCompanyId(Guid id)
        {
            var result = await _mediator.Send(new GetAllProjectByCompanyIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<List<ProjectsModel>>(false, "No Projects found for the company"));

            return Ok(new StatusResponse<List<ProjectsModel>>(true, "Projects fetched successfully", result));
        }
        [HttpPost("AllProjectsByDiv/{id}")]
        public async Task<ActionResult<List<ProjectsModel>>> GetAllProjectsByDivisionId(Guid id)
        {
            var result = await _mediator.Send(new GetAllProjectByDivisionIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<List<ProjectsModel>>(false, "No Projects found for the company"));

            return Ok(new StatusResponse<List<ProjectsModel>>(true, "Projects fetched successfully", result));
        }
        [HttpPost("AllProjectsByOrg/{id}")]
        public async Task<ActionResult<List<ProjectsModel>>> GetAllProjectsByOrganizationId(Guid id)
        {
            var result = await _mediator.Send(new GetAllProjectByOrganizationIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<List<ProjectsModel>>(false, "No Projects found for the company"));

            return Ok(new StatusResponse<List<ProjectsModel>>(true, "Projects fetched successfully", result));
        }

        [HttpDelete("delete/{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var command = new DeleteProjectCommand(projectId);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new StatusResponse<object>(true, "Project deleted successfully"));
            }
            return NotFound(new StatusResponse<object>(false, "Project not found"));
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched Project ID"));
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Project updated successfully"));
        }
    }
}