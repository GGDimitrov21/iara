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
    public class FishingShipsController : ControllerBase
    {
        private readonly IFishingShipService _service;

        public FishingShipsController(IFishingShipService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FishingShipFilter filter)
        {
            try
            {
                var ships = filter != null && (filter.ShipName != null || filter.IaraIdNumber != null || filter.MaritimeNumber != null || filter.OwnerName != null || filter.IsActive != null)
                    ? await _service.GetFilteredAsync(filter)
                    : await _service.GetAllAsync();
                return Ok(ships);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving fishing ships", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ship = await _service.GetByIdAsync(id);
                return Ok(ship);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the fishing ship", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateFishingShipRequestDTO request)
        {
            try
            {
                var ship = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = ship.ShipId }, ship);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the fishing ship", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFishingShipRequestDTO request)
        {
            try
            {
                var ship = await _service.UpdateAsync(id, request);
                return Ok(ship);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the fishing ship", error = ex.Message });
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the fishing ship", error = ex.Message });
            }
        }
    }
}