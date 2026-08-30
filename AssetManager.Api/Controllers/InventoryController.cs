using AssetManager.Api.Data;
using AssetManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAll()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetById(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItem>> Create(InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InventoryItem updated)
        {
            if (id != updated.Id) return BadRequest("The ID does not match.");

            var existing = await _context.InventoryItems.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = updated.Name;
            existing.Sku = updated.Sku;
            existing.Category = updated.Category;
            existing.Location = updated.Location;
            existing.Quantity = updated.Quantity;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class AdjustRequest
        {
            public int Amount { get; set; } // positive = stock in, negative = stock out
        }

        [HttpPost("{id}/adjust")]
        public async Task<IActionResult> AdjustQuantity(int id, AdjustRequest request)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null) return NotFound();

            var newQuantity = item.Quantity + request.Amount;
            if (newQuantity < 0)
                return BadRequest("Not enough quantity in stock.");

            item.Quantity = newQuantity;
            await _context.SaveChangesAsync();
            return Ok(item);
        }
    }
}