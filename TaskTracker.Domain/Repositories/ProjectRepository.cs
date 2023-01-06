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
            var projects = await _context.Projects.ToListAsync();
            foreach (var project in projects)
            {
                project.Tasks = await _context.Tasks.Where(task=>task.ProjectId == project.ProjectId).ToListAsync();
            }
            return projects;
        }

        public async Task<Project> GetAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if(project is null)
            {
                return null;
            }
            project.Tasks = await _context.Tasks.Where(task=>task.ProjectId == id).ToListAsync();
            return project;
        }

        public async Task<Project> GetAsync(string name)
        {
            var project = await _context.Projects.Where(project=>project.ProjectName == name).FirstOrDefaultAsync();
            if(project is null) return null;
            project.Tasks = await _context.Tasks.Where(task=>task.ProjectId == project.ProjectId).ToListAsync();
            return project;
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