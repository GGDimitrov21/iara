using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.FishingPermits;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing fishing permits
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FishingPermitsController : ControllerBase
{
    private readonly IFishingPermitService _permitService;
    private readonly ILogger<FishingPermitsController> _logger;

    public FishingPermitsController(
        IFishingPermitService permitService,
        ILogger<FishingPermitsController> logger)
    {
        _permitService = permitService;
        _logger = logger;
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
    /// Get all permits for a specific ship
    /// </summary>
    [HttpGet("ship/{shipId}")]
    public async Task<IActionResult> GetByShipId(int shipId, CancellationToken cancellationToken)
    {
        var result = await _permitService.GetPermitsByShipAsync(shipId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new fishing permit
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFishingPermitRequest request, CancellationToken cancellationToken)
    {
        var result = await _permitService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.PermitId }, result.Data);
    }

    /// <summary>
    /// Revoke a fishing permit
    /// </summary>
    [HttpPost("{id}/revoke")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Revoke(int id, CancellationToken cancellationToken)
    {
        var result = await _permitService.RevokePermitAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}
