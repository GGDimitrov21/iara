using Iara.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iara.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VesselsController : ControllerBase
{
    private readonly AppDbContext _db;
    public VesselsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        => Ok(await _db.Vessels.AsNoTracking().OrderBy(v => v.VesselId).ToListAsync(ct));
}
