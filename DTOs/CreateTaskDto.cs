using System.ComponentModel.DataAnnotations;
using DevTrack.Models;

namespace DevTrack.DTOs
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500,
            ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [EnumDataType(typeof(TaskPriority),
            ErrorMessage = "Invalid priority.")]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [EnumDataType(typeof(DevTrack.Models.TaskStatus),
            ErrorMessage = "Invalid status.")]
        public DevTrack.Models.TaskStatus Status { get; set; }
            = DevTrack.Models.TaskStatus.ToDo;

        public DateTime? DueDate { get; set; }
    }
}