using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.FishingShips;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing fishing ships
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FishingShipsController : ControllerBase
{
    private readonly IFishingShipService _fishingShipService;
    private readonly ILogger<FishingShipsController> _logger;

    public FishingShipsController(
        IFishingShipService fishingShipService,
        ILogger<FishingShipsController> logger)
    {
        _fishingShipService = fishingShipService;
        _logger = logger;
    }

    /// <summary>
    /// Get all fishing ships
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Inspector,Viewer")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _fishingShipService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get fishing ship by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _fishingShipService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new fishing ship
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFishingShipRequest request, CancellationToken cancellationToken)
    {
        var result = await _fishingShipService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ShipId }, result.Data);
    }

    /// <summary>
    /// Update fishing ship details
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFishingShipRequest request, CancellationToken cancellationToken)
    {
        var result = await _fishingShipService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Delete fishing ship
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _fishingShipService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}
