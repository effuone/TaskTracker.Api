using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Data.DTOs
{
    //data transfer objects of task entity
    public class ToDoTaskDTO
    {
        public int ToDoTaskId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Description { get; set; }
        public int Priority { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
    }
    public class CreateToDoTaskDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
    public class UpdateToDoTaskDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int Status { get; set; }

    }
}