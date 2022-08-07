using TaskTracker.Api.Models;

namespace TaskTracker.Api.Interfaces
{
    //interface for task repository
    public interface ITaskRepository : IAsyncRepository<ToDoTask>
    {
        public Task<ToDoTask> GetAsync(string name);
        public Task<IEnumerable<ToDoTask>> GetAllAsync(int projectId);
    }
}