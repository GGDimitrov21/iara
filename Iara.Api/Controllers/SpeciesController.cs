using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iara.Application.DTOs.Species;
using Iara.Application.Services;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing species
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SpeciesController : ControllerBase
{
    private readonly ISpeciesService _speciesService;
    private readonly ILogger<SpeciesController> _logger;

    public SpeciesController(
        ISpeciesService speciesService,
        ILogger<SpeciesController> logger)
    {
        _speciesService = speciesService;
        _logger = logger;
    }

    /// <summary>
    /// Get all species
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _speciesService.GetAllAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Get species by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _speciesService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Create a new species
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSpeciesRequest request, CancellationToken cancellationToken)
    {
        var result = await _speciesService.CreateAsync(request, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.SpeciesId }, result.Data);
    }

    /// <summary>
    /// Delete a species
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _speciesService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.ErrorMessage });
    }
}
