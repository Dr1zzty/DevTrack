
using DevTrack.Data;
using DevTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Where(task => !task.IsDeleted)
                .ToListAsync();

            return Ok(tasks);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == id && !task.IsDeleted);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.Id },
                task);
            
        }
    }
}