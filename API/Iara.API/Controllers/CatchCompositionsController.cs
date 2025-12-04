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
    public class CatchCompositionsController : ControllerBase
    {
        private readonly ICatchCompositionService _service;

        public CatchCompositionsController(ICatchCompositionService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var catchComp = await _service.GetByIdAsync(id);
                return Ok(catchComp);
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

        [HttpGet("log-entry/{logEntryId}")]
        public async Task<IActionResult> GetByLogEntryId(long logEntryId)
        {
            try
            {
                var catches = await _service.GetByLogEntryIdAsync(logEntryId);
                return Ok(catches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatchCompositionRequestDTO request)
        {
            try
            {
                var catchComp = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = catchComp.CatchId }, catchComp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}