using Microsoft.EntityFrameworkCore;
using TaskTracker.Data.Context;

namespace TaskTracker.Api.Preparations
{
    public static class MigrationImplementer
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<TaskContext>());
            }
        }
        public static void SeedData(TaskContext context)
        {
            System.Console.WriteLine("Applying migrations...");
            context.Database.Migrate();
        }
    }
}