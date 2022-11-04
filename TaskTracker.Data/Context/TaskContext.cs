using Microsoft.EntityFrameworkCore;
using TaskTracker.Data.Models;

namespace TaskTracker.Data.Context
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
        //uncomment this code for migrations using dotnet ef migrations add "name"
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("");
        // }
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