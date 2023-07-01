using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using EmployeeApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EmployeeApp.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmployeeAppController : ControllerBase
    {
        private readonly EmployeeMgrContext _context;
        private readonly ILogger<EmployeeAppController> _logger;
        public EmployeeAppController(EmployeeMgrContext context, ILogger<EmployeeAppController> logger)
        {
            _context = context;
            _logger = logger;
        }
    
        [HttpGet]
        public async Task<IEnumerable<EmployeeVM>> Get()
        {
            var employees = await _context.Employees.Include(e => e.Employee).ToListAsync();

            return employees.Select(e => new EmployeeVM
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                FullName = e.FullName,
                Email = e.Email
            });
        }
    
        [HttpGet("{id}")]
        public ActionResult<EmployeeVM> GetById(int id)
        {
                var employee = _context.Employees.SingleOrDefault(e => e.EmployeesId == id);

                if (employee == null)
                {
                    return NotFound();
                }
                else
                {
                    return new EmployeeVM
                    {
                        EmployeeId = employee.EmployeeId,
                        Name = employee.Name,
                        FullName = employee.FullName,
                        Email = employee.Email
                    };
                }
        }
    
        [HttpPost]
        public async Task<ActionResult<int>> Create(EmployeeVM vm)
        {
            try
            {
                var newEmployee = new Employee
                {
                        Name = vm.Name,
                        FullName = vm.FullName,
                        Email = vm.Email
                };

                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Employee created successfully.");

                return Ok(newEmployee.EmployeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create employee.");
                return BadRequest("Failed to create employee.");
            }
    }

    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}