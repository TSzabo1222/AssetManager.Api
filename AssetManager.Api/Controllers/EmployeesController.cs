using AssetManager.Api.Data;
using AssetManager.Api.Models;
using AssetManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IActivityLogger _activityLogger;

        public EmployeesController(AppDbContext context, IActivityLogger activityLogger)
        {
            _context = context;
            _activityLogger = activityLogger;
        }

        private string CurrentUserName =>
            User.Identity?.Name ?? User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Unknown user";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return await _context.Employees.Include(e => e.AssignedAssets).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _context.Employees.Include(e => e.AssignedAssets).FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            await _activityLogger.LogAsync(CurrentUserName, "created", "Employee", employee.FullName);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee updated)
        {
            if (id != updated.Id) return BadRequest("The ID does not match.");

            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = updated.FullName;
            existing.Email = updated.Email;
            existing.Department = updated.Department;
            existing.Position = updated.Position;
            existing.HireDate = updated.HireDate;

            await _context.SaveChangesAsync();
            await _activityLogger.LogAsync(CurrentUserName, "updated", "Employee", existing.FullName);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            await _activityLogger.LogAsync(CurrentUserName, "deleted", "Employee", employee.FullName);

            return NoContent();
        }
    }
}