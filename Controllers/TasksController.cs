using DevTrack.Data;
using DevTrack.DTOs;
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
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks(
            [FromQuery] TaskItemStatus? status,
            [FromQuery] TaskPriority? priority)
        {
            var query = _context.Tasks.AsNoTracking();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(tasks.Select(TaskResponseDto.From));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return Ok(TaskResponseDto.From(task));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(
            CreateTaskDto newTask)
        {
            var task = new TaskItem
            {
                Title = newTask.Title.Trim(),
                Description = newTask.Description.Trim(),
                Priority = newTask.Priority,
                Status = TaskItemStatus.ToDo,
                DueDate = newTask.DueDate,
                OriginalDueDate = newTask.DueDate,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.Id },
                TaskResponseDto.From(task));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(
            int id,
            UpdateTaskDto updatedTask)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            bool wasCompleted = task.Status == TaskItemStatus.Completed;
            bool willBeCompleted = updatedTask.Status == TaskItemStatus.Completed;

            task.Title = updatedTask.Title.Trim();
            task.Description = updatedTask.Description.Trim();
            task.Priority = updatedTask.Priority!.Value;
            task.DueDate = updatedTask.DueDate;
            task.Status = updatedTask.Status!.Value;

            if (!wasCompleted && willBeCompleted)
            {
                task.CompletedAt = DateTime.UtcNow;
                task.WasCompletedLate = IsLate(task);
            }
            else if (wasCompleted && willBeCompleted)
            {
                task.WasCompletedLate = IsLate(task);
            }
            else if (wasCompleted && !willBeCompleted)
            {
                task.CompletedAt = null;
                task.WasCompletedLate = null;
            }

            await _context.SaveChangesAsync();

            return Ok(TaskResponseDto.From(task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Compara só a data: concluir no próprio dia do prazo não é atraso.
        private static bool IsLate(TaskItem task) =>
            task.DueDate.HasValue &&
            task.CompletedAt.HasValue &&
            task.CompletedAt.Value.Date > task.DueDate.Value.Date;
    }
}