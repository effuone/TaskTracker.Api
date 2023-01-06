using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Data.DTOs
{
    //data transfer objects of project entity
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public int Priority { get; set; }
        public int Status { get; set; }
        public IEnumerable<ToDoTaskDTO> ToDoTasks {get; set;}
        public ProjectDTO()
        {
            
        }

        public ProjectDTO(string projectName, DateTime startDate, DateTime? endDate, int priority, int status, IEnumerable<ToDoTaskDTO> toDoTasks)
        {
            ProjectName = projectName;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
            Status = status;
            ToDoTasks = toDoTasks;
        }
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

        public CreateProjectDTO(string projectName, DateTime startDate, DateTime? endDate, int priority)
        {
            ProjectName = projectName;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
        }
        public CreateProjectDTO()
        {
            
        }
    }
    public class UpdateProjectDTO
    {
        [MaxLength(30)]
        public string ProjectName { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public int Priority { get; set; }
        public int Status { get; set; }

        public UpdateProjectDTO(string projectName, DateTime startDate, DateTime? endDate, int priority, int status)
        {
            ProjectName = projectName;
            StartDate = startDate;
            EndDate = endDate;
            Priority = priority;
            Status = status;
        }
        public UpdateProjectDTO()
        {
            
        }
    }
}