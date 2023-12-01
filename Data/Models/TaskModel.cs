namespace Planner.Data.Models
{
    public class TaskModel
    {
        public required string Task { get; set; }
        public DateTime DueDate { get; set; }
        public string? TaskDescription { get; set; }
    }
}
