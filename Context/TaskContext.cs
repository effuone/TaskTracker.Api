using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Models;

namespace TaskTracker.Api.Context
{
    //EntityFrameworkCore database context configuration
    public class TaskContext : DbContext
    {
        public TaskContext()
        {
            
        }
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }
        public DbSet<ToDoTask> Tasks {get; set;}
        public DbSet<Project> Projects {get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // UNIQUE additions
            builder.Entity<ToDoTask>().HasIndex(x=>x.Name).IsUnique(true);
            builder.Entity<Project>().HasIndex(x=>x.ProjectName).IsUnique(true);
            //One-to-many relationship configuration
            builder.Entity<Project>().HasMany(x=>x.Tasks).WithOne(x=>x.Project);
        }
    }
}