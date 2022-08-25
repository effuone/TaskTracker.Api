using TaskTracker.Data.Models;

namespace TaskTracker.Domain.Interfaces
{
    //interface for project repository
    public interface IProjectRepository : IAsyncRepository<Project>
    {
        public Task<Project> GetAsync(string name);
        public Task AddTaskToProject(int id, ToDoTask model);
        public Task RemoveTaskFromProject(int taskId);
    }
}