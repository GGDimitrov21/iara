using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Logbook;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing logbook entries
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LogbookController : ControllerBase
{
    private readonly ILogbookService _logbookService;
    private readonly ILogger<LogbookController> _logger;

    public LogbookController(
        ILogbookService logbookService,
        ILogger<LogbookController> logger)
    {
        _logbookService = logbookService;
        _logger = logger;
    }

    /// <summary>
    /// Get all logbook entries
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _logbookService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get logbook entry by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _logbookService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Get all logbook entries for a specific vessel
    /// </summary>
    [HttpGet("vessel/{vesselId}")]
    public async Task<IActionResult> GetByVesselId(int vesselId, CancellationToken cancellationToken)
    {
        var result = await _logbookService.GetByVesselAsync(vesselId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new logbook entry
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLogbookEntryRequest request, CancellationToken cancellationToken)
    {
        var result = await _logbookService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.LogEntryId }, result.Data);
    }

    /// <summary>
    /// Update a logbook entry
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLogbookEntryRequest request, CancellationToken cancellationToken)
    {
        var result = await _logbookService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.ErrorMessage });
    }

    /// <summary>
    /// Delete a logbook entry
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _logbookService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.ErrorMessage });
    }
}
