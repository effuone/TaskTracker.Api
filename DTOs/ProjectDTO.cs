using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Api.DTOs
{
    //data transfer objects of project entity
    public class ProjectDTO
    {
        public string ProjectName { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public int Priority { get; set; }
        public int Status { get; set; }
        public IEnumerable<ToDoTaskDTO> ToDoTasks {get; set;}
    }
    public class CreateProjectDTO
    {
        [Required]
        [MaxLength(30)]
        public string ProjectName { get; set; }
        [Required]
        public DateTime StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        [Required]

        public int Priority { get; set; }
    }
    public class UpdateProjectDTO
    {
        [MaxLength(30)]
        public string ProjectName { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public int Priority { get; set; }
        public int Status { get; set; }
    }
}