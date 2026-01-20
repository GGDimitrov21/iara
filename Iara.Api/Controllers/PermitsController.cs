using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Permits;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing permits
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PermitsController : ControllerBase
{
    private readonly IPermitService _permitService;
    private readonly ILogger<PermitsController> _logger;

    public PermitsController(
        IPermitService permitService,
        ILogger<PermitsController> logger)
    {
        _permitService = permitService;
        _logger = logger;
    }

    /// <summary>
    /// Get all active permits
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _permitService.GetAllActiveAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get permit by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _permitService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Get all permits for a specific vessel
    /// </summary>
    [HttpGet("vessel/{vesselId}")]
    public async Task<IActionResult> GetByVesselId(int vesselId, CancellationToken cancellationToken)
    {
        var result = await _permitService.GetByVesselAsync(vesselId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new permit
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermitRequest request, CancellationToken cancellationToken)
    {
        var result = await _permitService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.PermitId }, result.Data);
    }

    /// <summary>
    /// Revoke a permit
    /// </summary>
    [HttpPost("{id}/revoke")]
    public async Task<IActionResult> Revoke(int id, CancellationToken cancellationToken)
    {
        var result = await _permitService.RevokePermitAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.ErrorMessage });
    }
}
