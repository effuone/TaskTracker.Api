using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Api.Models
{
    //project entity domain model
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime? EndDate {get; set;}
        public ProjectStatus Status { get; set; }
        public int Priority { get; set; }

        public IEnumerable<ToDoTask> Tasks { get; set; }
    }
}