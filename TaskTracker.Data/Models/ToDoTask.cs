using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskTracker.Data.Models
{
    //task entity domain model
    public class ToDoTask
    {
        public int ToDoTaskId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Description { get; set; }
        public int Priority { get; set; }
        public TodoStatus Status { get; set; }
        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
    }
}