using HRMS.API.Responses;
using HRMS.Application.Commands.Asset;
using HRMS.Application.Queries.Asset;
using HRMS.Application.Queries.Attendance;
using HRMS.Domain.Entities.Asset;
using HRMS.Domain.Entities.Attendance;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Asset
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : Controller
    {
        private readonly IMediator _mediator;

        public AssetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateAsset")]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetCommand command)
        {  
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateAsset), new StatusResponse<object>(true, "Asset created successfully", new { id }));
        }

        [HttpGet("getbyid{assetId}")]
        public async Task<IActionResult> GetAssetById(Guid assetId)
        {
             var query = new GetAssetByIdQuery(assetId);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(new StatusResponse<object>(false, "Asset not found"));
            }
            return Ok(new StatusResponse<AssetModel>(true, "Asset retrieved successfully", result));
        }

        [HttpPost("getallby")]
        public async Task<ActionResult<List<AssetModel>>> GetAllAssetByOrgId(Guid empId)
        {
            var result = await _mediator.Send(new GetAllAssetByOrgIdQuery(empId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<AssetModel>>(false, "No Asset found for the Organization"));
            return Ok(new StatusResponse<List<AssetModel>>(true, "Asset fetched successfully", result));
        }

        [HttpPut("update/{assetId}")]
        public async Task<IActionResult> UpdateAsset(Guid assetId, [FromBody] UpdateAssetCommand cmd)
        {
            if (assetId != cmd.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "Asset ID mismatch"));
            }
            var result = await _mediator.Send(cmd);
            return Ok(new StatusResponse<object>(true, "Asset updated successfully"));
        }
        [HttpDelete("delete/{assetId}")]
        public async Task<IActionResult> DeleteAsset(Guid assetId)
        {
            var command = new DeleteAssetCommand(assetId);
            var result = await _mediator.Send(command);
             if (result)
            {
                return Ok(new StatusResponse<object>(true, "Asset deleted successfully"));
            }
            return NotFound(new StatusResponse<object>(false, "Asset not found"));
            
        }
    }
}
