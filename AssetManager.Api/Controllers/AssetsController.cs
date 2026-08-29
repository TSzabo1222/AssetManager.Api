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

        // GET: api/assets?status=InStock&type=Laptop
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

        // GET: api/assets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetById(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.AssignedToEmployee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null) return NotFound();
            return asset;
        }

        // POST: api/assets
        [HttpPost]
        public async Task<ActionResult<Asset>> Create(Asset asset)
        {
            asset.Status = AssetStatus.InStock; // új eszköz mindig raktáron kezdi
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        // PUT: api/assets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Asset updated)
        {
            if (id != updated.Id) return BadRequest("Az ID nem egyezik.");

            var existing = await _context.Assets.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = updated.Name;
            existing.Type = updated.Type;
            existing.SerialNumber = updated.SerialNumber;
            existing.PurchaseDate = updated.PurchaseDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/assets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound();

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- Ez a rész mutatja, hogy nem csak CRUD-ot tudsz ---

        public class AssignRequest
        {
            public int EmployeeId { get; set; }
        }

        // POST: api/assets/5/assign
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(int id, AssignRequest request)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Az eszköz nem található.");

            if (asset.Status != AssetStatus.InStock)
                return BadRequest("Az eszköz jelenleg nem elérhető kiadásra (nincs 'InStock' állapotban).");

            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if (employee == null) return NotFound("Az alkalmazott nem található.");

            // Üzleti logika: állapotváltás + napló bejegyzés egy tranzakcióban
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

        // POST: api/assets/5/return
        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound("Az eszköz nem található.");

            if (asset.Status != AssetStatus.Assigned)
                return BadRequest("Az eszköz nincs jelenleg kiadva senkinek.");

            // A nyitott (még vissza nem adott) history bejegyzés lezárása
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

        // GET: api/assets/5/history
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
