
using DevTrack.Data;
using DevTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevTrack.DTOs;

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
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> UpdateTask(int id, UpdateTaskDto updatedTask)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == id && !task.IsDeleted);

            if (task == null)
            {
                    return NotFound();
            }
           
            bool wasCompleted = task.Status == DevTrack.Models.TaskStatus.Completed;
            bool willBeCompleted = updatedTask.Status == DevTrack.Models.TaskStatus.Completed;
            
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Priority = updatedTask.Priority;
            task.DueDate = updatedTask.DueDate;
            task.Status = updatedTask.Status;

            if (!wasCompleted && willBeCompleted)
            {
                task.CompletedAt = DateTime.Now;
                task.WasCompletedLate = task.DueDate.HasValue && 
                    task.CompletedAt!.Value.Date > task.DueDate.Value.Date;
            }
            else if (wasCompleted && !willBeCompleted)
            {
                task.CompletedAt = null;
                task.WasCompletedLate = null;
            }
            await _context.SaveChangesAsync();

            return Ok(task);

        }
    }
}