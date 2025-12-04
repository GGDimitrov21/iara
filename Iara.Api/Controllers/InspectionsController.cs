using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Inspections;
using Iara.Application.Services;
using System.Security.Claims;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing fishing inspections
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Inspector,Admin")]
public class InspectionsController : ControllerBase
{
    private readonly IInspectionService _inspectionService;
    private readonly ILogger<InspectionsController> _logger;

    public InspectionsController(
        IInspectionService inspectionService,
        ILogger<InspectionsController> logger)
    {
        _inspectionService = inspectionService;
        _logger = logger;
    }

    /// <summary>
    /// Get inspection by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Get all inspections for a specific ship
    /// </summary>
    [HttpGet("ship/{shipId}")]
    public async Task<IActionResult> GetByShipId(int shipId, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.GetInspectionsByShipAsync(shipId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new inspection record
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInspectionRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var inspectorId))
        {
            return Unauthorized("Invalid inspector ID");
        }

        var result = await _inspectionService.CreateAsync(request, inspectorId, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.InspectionId }, result.Data);
    }

    /// <summary>
    /// Mark inspection as processed
    /// </summary>
    [HttpPost("{id}/process")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProcessInspection(int id, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.ProcessInspectionAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}
