using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Api.Context
{
    public static class PrepDB
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