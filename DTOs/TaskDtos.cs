using System.ComponentModel.DataAnnotations;
using DevTrack.Models;

namespace DevTrack.DTOs
{
    public abstract class TaskInputDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 100 characters.")]
        [RegularExpression(@".*\S.*",
            ErrorMessage = "Title cannot contain only whitespace.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
    }

    public class CreateTaskDto : TaskInputDto
    {
        [EnumDataType(typeof(TaskPriority), ErrorMessage = "Invalid priority.")]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }

    public class UpdateTaskDto : TaskInputDto
    {
        [Required(ErrorMessage = "Priority is required.")]
        [EnumDataType(typeof(TaskPriority), ErrorMessage = "Invalid priority.")]
        public TaskPriority? Priority { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(TaskItemStatus), ErrorMessage = "Invalid status.")]
        public TaskItemStatus? Status { get; set; }
    }
}