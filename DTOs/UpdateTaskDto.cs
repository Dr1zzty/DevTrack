using DevTrack.Models;

namespace DevTrack.DTOs
{
    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DevTrack.Models.TaskStatus Status { get; set; } = DevTrack.Models.TaskStatus.ToDo;

        public DateTime? DueDate { get; set; }
    }
}