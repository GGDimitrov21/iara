using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Iara.BusinessLogic.Services;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.Filters;

namespace Iara.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionsController : ControllerBase
    {
        private readonly IInspectionService _service;

        public InspectionsController(IInspectionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] InspectionFilter filter)
        {
            try
            {
                var inspections = filter != null && (filter.ShipId != null || filter.InspectorId != null || filter.HasViolation != null || filter.IsProcessed != null || filter.InspectionDateFrom != null || filter.InspectionDateTo != null)
                    ? await _service.GetFilteredAsync(filter)
                    : await _service.GetAllAsync();
                return Ok(inspections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var inspection = await _service.GetByIdAsync(id);
                return Ok(inspection);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Inspector,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateInspectionRequestDTO request)
        {
            try
            {
                var inspection = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = inspection.InspectionId }, inspection);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpPost("{id}/process")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Process(int id)
        {
            try
            {
                var result = await _service.ProcessInspectionAsync(id);
                return Ok(new { message = "Inspection processed successfully", processed = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}