using AssetManager.Api.Data;
using AssetManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssetsController(AppDbContext context)
        {
            _context = context;
        }

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
            asset.Status = AssetStatus.InStock; // a new asset always starts in stock
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
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
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound();

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- This part shows real business logic, not just CRUD ---

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

            // Business logic: status change + history entry in one transaction
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
            return Ok(asset);
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Asset not found.");

            if (asset.Status != AssetStatus.Assigned)
                return BadRequest("This asset is not currently assigned to anyone.");

            // Close the open (not yet returned) history entry
            var openHistory = await _context.AssignmentHistories
                .Where(h => h.AssetId == asset.Id && h.ReturnedDate == null)
                .OrderByDescending(h => h.AssignedDate)
                .FirstOrDefaultAsync();

            if (openHistory != null)
                openHistory.ReturnedDate = DateTime.UtcNow;

            asset.Status = AssetStatus.InStock;
            asset.AssignedToEmployeeId = null;

            await _context.SaveChangesAsync();
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