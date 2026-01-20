using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Vessels;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing vessels
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VesselsController : ControllerBase
{
    private readonly IVesselService _vesselService;
    private readonly ILogger<VesselsController> _logger;

    public VesselsController(
        IVesselService vesselService,
        ILogger<VesselsController> logger)
    {
        _vesselService = vesselService;
        _logger = logger;
    }

    /// <summary>
    /// Get all vessels
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _vesselService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get vessel by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _vesselService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new vessel
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVesselRequest request, CancellationToken cancellationToken)
    {
        var result = await _vesselService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.VesselId }, result.Data);
    }

    /// <summary>
    /// Update vessel details
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVesselRequest request, CancellationToken cancellationToken)
    {
        var result = await _vesselService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.ErrorMessage });
    }

    /// <summary>
    /// Delete vessel
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _vesselService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.ErrorMessage });
    }
}
