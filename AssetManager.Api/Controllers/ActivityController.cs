using AssetManager.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActivityController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/activity?take=30
        [HttpGet]
        public async Task<IActionResult> GetRecent([FromQuery] int take = 30)
        {
            var logs = await _context.ActivityLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(take)
                .ToListAsync();

            return Ok(logs);
        }
    }
}