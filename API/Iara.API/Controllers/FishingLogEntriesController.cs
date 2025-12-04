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
    public class FishingLogEntriesController : ControllerBase
    {
        private readonly IFishingLogEntryService _service;

        public FishingLogEntriesController(IFishingLogEntryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FishingLogEntryFilter filter)
        {
            try
            {
                var logEntries = filter != null && (filter.ShipId != null || filter.LogDateFrom != null || filter.LogDateTo != null || filter.FishingZone != null || filter.IsSigned != null)
                    ? await _service.GetFilteredAsync(filter)
                    : await _service.GetAllAsync();
                return Ok(logEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving fishing log entries", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var logEntry = await _service.GetByIdAsync(id);
                return Ok(logEntry);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the fishing log entry", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFishingLogEntryRequestDTO request)
        {
            try
            {
                var logEntry = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = logEntry.LogEntryId }, logEntry);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the fishing log entry", error = ex.Message });
            }
        }

        [HttpPost("{id}/sign")]
        public async Task<IActionResult> Sign(long id)
        {
            try
            {
                var result = await _service.SignLogEntryAsync(id);
                return Ok(new { message = "Log entry signed successfully", signed = result });
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
                return StatusCode(500, new { message = "An error occurred while signing the log entry", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the fishing log entry", error = ex.Message });
            }
        }
    }
}