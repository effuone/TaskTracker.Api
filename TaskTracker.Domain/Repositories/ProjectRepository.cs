using Microsoft.EntityFrameworkCore;
using TaskTracker.Data.Context;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Data.Models;

namespace TaskTracker.Domain.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskContext _context;

        public ProjectRepository(TaskContext context)
        {
            _context = context;
        }

        public async Task AddTaskToProject(int id, ToDoTask model)
        {
            var existingModel = await _context.Projects.FindAsync(id);
            model.ProjectId = id;
            existingModel.Tasks.Append(model);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateAsync(Project model)
        {
            await _context.Projects.AddAsync(model);
            await _context.SaveChangesAsync();
            return model.ProjectId;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.Projects.FindAsync(id);
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<Project> GetAsync(string name)
        {
            return await _context.Projects.Where(x=>x.ProjectName == name).FirstOrDefaultAsync();
        }

        public async Task RemoveTaskFromProject(int taskId)
        {
            var existingTask = await _context.Projects.FindAsync(taskId);
            _context.Remove(existingTask);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Project model)
        {
            model.ProjectId = id;
            _context.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}