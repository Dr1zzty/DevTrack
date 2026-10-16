namespace DevTrack.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty; 
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "To Do";
        public DataTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; } 
        public bool IsDeleted { get; set; } = false;
    }
}