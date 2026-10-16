namespace DevTrack.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public DateTime? DueDate { get; set; }
        public DateTime? OriginalDueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; } 
        public bool? WasCompletedLate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}