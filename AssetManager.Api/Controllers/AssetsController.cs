using AssetManager.Api.Data;
using AssetManager.Api.Models;
using AssetManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IActivityLogger _activityLogger;

        public AssetsController(AppDbContext context, IActivityLogger activityLogger)
        {
            _context = context;
            _activityLogger = activityLogger;
        }

        private string CurrentUserName =>
            User.Identity?.Name ?? User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Unknown user";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAll([FromQuery] AssetStatus? status, [FromQuery] string? type)
        {
            var query = _context.Assets.Include(a => a.AssignedToEmployee).AsQueryable();

            if (status.HasValue)
                query = query.Where(a => a.Status == status);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(a => a.Type == type);

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetById(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.AssignedToEmployee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null) return NotFound();
            return asset;
        }

        [HttpPost]
        public async Task<ActionResult<Asset>> Create(Asset asset)
        {
            asset.Status = AssetStatus.InStock;
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            await _activityLogger.LogAsync(CurrentUserName, "created", "Asset", asset.Name);

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Asset updated)
        {
            if (id != updated.Id) return BadRequest("The ID does not match.");

            var existing = await _context.Assets.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = updated.Name;
            existing.Type = updated.Type;
            existing.SerialNumber = updated.SerialNumber;
            existing.PurchaseDate = updated.PurchaseDate;

            await _context.SaveChangesAsync();
            await _activityLogger.LogAsync(CurrentUserName, "updated", "Asset", existing.Name);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound();

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();

            await _activityLogger.LogAsync(CurrentUserName, "deleted", "Asset", asset.Name);

            return NoContent();
        }

        public class AssignRequest
        {
            public int EmployeeId { get; set; }
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(int id, AssignRequest request)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Asset not found.");

            if (asset.Status != AssetStatus.InStock)
                return BadRequest("This asset is not currently available for assignment (not 'InStock').");

            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null) return NotFound("Employee not found.");

            asset.Status = AssetStatus.Assigned;
            asset.AssignedToEmployeeId = employee.Id;

            _context.AssignmentHistories.Add(new AssignmentHistory
            {
                AssetId = asset.Id,
                EmployeeId = employee.Id,
                AssignedDate = DateTime.UtcNow,
                ReturnedDate = null
            });

            await _context.SaveChangesAsync();
            await _activityLogger.LogAsync(CurrentUserName, "assigned", "Asset", $"{asset.Name} to {employee.FullName}");

            return Ok(asset);
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Asset not found.");

            if (asset.Status != AssetStatus.Assigned)
                return BadRequest("This asset is not currently assigned to anyone.");

            var openHistory = await _context.AssignmentHistories
                .Where(h => h.AssetId == asset.Id && h.ReturnedDate == null)
                .OrderByDescending(h => h.AssignedDate)
                .FirstOrDefaultAsync();

            if (openHistory != null)
                openHistory.ReturnedDate = DateTime.UtcNow;

            asset.Status = AssetStatus.InStock;
            asset.AssignedToEmployeeId = null;

            await _context.SaveChangesAsync();
            await _activityLogger.LogAsync(CurrentUserName, "returned", "Asset", asset.Name);

            return Ok(asset);
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<AssignmentHistory>>> GetHistory(int id)
        {
            return await _context.AssignmentHistories
                .Where(h => h.AssetId == id)
                .Include(h => h.Employee)
                .OrderByDescending(h => h.AssignedDate)
                .ToListAsync();
        }
    }
}