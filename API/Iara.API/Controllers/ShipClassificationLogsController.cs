using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Iara.BusinessLogic.Services;
using Iara.DomainModel.RequestDTOs;

namespace Iara.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShipClassificationLogsController : ControllerBase
    {
        private readonly IShipClassificationLogService _service;

        public ShipClassificationLogsController(IShipClassificationLogService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var log = await _service.GetByIdAsync(id);
                return Ok(log);
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

        [HttpGet("ship/{shipId}")]
        public async Task<IActionResult> GetByShipId(int shipId)
        {
            try
            {
                var logs = await _service.GetByShipIdAsync(shipId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateShipClassificationLogRequestDTO request)
        {
            try
            {
                var log = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = log.LogId }, log);
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