using Microsoft.EntityFrameworkCore;
using TaskTracker.Data.Context;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Data.Models;

namespace TaskTracker.Domain.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(ToDoTask model)
        {
            await _context.Tasks.AddAsync(model);
            await _context.SaveChangesAsync();
            return model.ToDoTaskId;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.Tasks.FindAsync(id);
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetAllAsync(int projectId)
        {
            return await _context.Tasks.Where(x=>x.ProjectId == projectId).ToListAsync();
        }

        public async Task<ToDoTask> GetAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<ToDoTask> GetAsync(string name)
        {
            return await _context.Tasks.Where(x=>x.Name == name).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, ToDoTask model)
        {
            model.ToDoTaskId = id;
            _context.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}