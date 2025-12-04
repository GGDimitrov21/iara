using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.FishingLogs;
using Iara.Application.Services;
using System.Security.Claims;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing fishing log entries
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FishingLogsController : ControllerBase
{
    private readonly IFishingLogEntryService _logService;
    private readonly ILogger<FishingLogsController> _logger;

    public FishingLogsController(
        IFishingLogEntryService logService,
        ILogger<FishingLogsController> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    /// <summary>
    /// Get log entry by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _logService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Get all log entries for a specific ship
    /// </summary>
    [HttpGet("ship/{shipId}")]
    public async Task<IActionResult> GetByShipId(int shipId, CancellationToken cancellationToken)
    {
        var result = await _logService.GetLogsByShipAsync(shipId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new fishing log entry
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Fisherman,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFishingLogEntryRequest request, CancellationToken cancellationToken)
    {
        var result = await _logService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.LogEntryId }, result.Data);
    }

    /// <summary>
    /// Sign a log entry to confirm accuracy
    /// </summary>
    [HttpPost("{id}/sign")]
    [Authorize(Roles = "Fisherman,Admin")]
    public async Task<IActionResult> SignLogEntry(long id, CancellationToken cancellationToken)
    {
        var result = await _logService.SignLogEntryAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}
