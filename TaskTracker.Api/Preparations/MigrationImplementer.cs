using Microsoft.EntityFrameworkCore;
using TaskTracker.Data.Context;

namespace TaskTracker.Api.Preparations
{
    public static class MigrationImplementer
    {
        public static async Task PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                await SeedData(serviceScope.ServiceProvider.GetService<TaskContext>());
            }
        }
        public static async Task SeedData(TaskContext context)
        {
            System.Console.WriteLine("Applying migrations...");
            await context.Database.MigrateAsync();
        }
    }
}