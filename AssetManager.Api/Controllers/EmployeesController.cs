using AssetManager.Api.Data;
using AssetManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return await _context.Employees
                .Include(e => e.AssignedAssets)
                .ToListAsync();
        }

        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.AssignedAssets)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();
            return employee;
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee updated)
        {
            if (id != updated.Id) return BadRequest("Az ID nem egyezik.");

            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = updated.FullName;
            existing.Email = updated.Email;
            existing.Department = updated.Department;
            existing.Position = updated.Position;
            existing.HireDate = updated.HireDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
