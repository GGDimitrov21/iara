using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.CatchQuotas;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing catch quotas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class QuotasController : ControllerBase
{
    private readonly ICatchQuotaService _quotaService;
    private readonly ILogger<QuotasController> _logger;

    public QuotasController(
        ICatchQuotaService quotaService,
        ILogger<QuotasController> logger)
    {
        _quotaService = quotaService;
        _logger = logger;
    }

    /// <summary>
    /// Get all quotas
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _quotaService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get quota by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _quotaService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Get all quotas for a specific permit
    /// </summary>
    [HttpGet("permit/{permitId}")]
    public async Task<IActionResult> GetByPermitId(int permitId, CancellationToken cancellationToken)
    {
        var result = await _quotaService.GetByPermitAsync(permitId, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new catch quota
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatchQuotaRequest request, CancellationToken cancellationToken)
    {
        var result = await _quotaService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.QuotaId }, result.Data);
    }

    /// <summary>
    /// Update a catch quota
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCatchQuotaRequest request, CancellationToken cancellationToken)
    {
        var result = await _quotaService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { message = result.ErrorMessage });
    }

    /// <summary>
    /// Delete a catch quota
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _quotaService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.ErrorMessage });
    }
}
