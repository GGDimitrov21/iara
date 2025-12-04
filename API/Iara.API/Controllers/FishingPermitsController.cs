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
    public class FishingPermitsController : ControllerBase
    {
        private readonly IFishingPermitService _service;

        public FishingPermitsController(IFishingPermitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FishingPermitFilter filter)
        {
            try
            {
                var permits = filter != null && (filter.ShipId != null || filter.PermitYear != null || filter.Status != null)
                    ? await _service.GetFilteredAsync(filter)
                    : await _service.GetAllAsync();
                return Ok(permits);
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
                var permit = await _service.GetByIdAsync(id);
                return Ok(permit);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateFishingPermitRequestDTO request)
        {
            try
            {
                var permit = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = permit.PermitId }, permit);
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