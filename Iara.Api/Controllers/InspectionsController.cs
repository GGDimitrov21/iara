using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Inspections;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing inspections
/// </summary>
[ApiController]
[Route("api/[controller]")]
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
    /// Get all inspections
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _inspectionService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
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
    /// Get all inspections for a specific vessel
    /// </summary>
    [HttpGet("vessel/{vesselId}")]
    public async Task<IActionResult> GetByVesselId(int vesselId, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.GetByVesselAsync(vesselId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new inspection record
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInspectionRequest request, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.InspectionId }, result.Data);
    }

    /// <summary>
    /// Update an inspection
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInspectionRequest request, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.ErrorMessage });
    }

    /// <summary>
    /// Delete an inspection
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _inspectionService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.ErrorMessage });
    }
}
