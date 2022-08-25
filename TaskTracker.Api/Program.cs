using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Preparations;
using TaskTracker.Data.Context;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configuring MSSQL database connection string from secret store
var server = builder.Configuration["DB_SERVER"];
var port = builder.Configuration["DB_PORT"];
var user = builder.Configuration["DB_USER"];
var password = builder.Configuration["DB_PASSWORD"];
var database = builder.Configuration["DB_DATABASE"];
var connectionString = $"Server=tcp:{server}, {port};Database={database};User Id={user};Password={password}";                         
builder.Services.AddDbContext<TaskContext>(options=>options.UseSqlServer(connectionString));

//AutoMapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Dependency Injection
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddSwaggerGen();

//supressing async suffix in action names for retrieving object via "CreatedAtAction" method after HttpPost requests
builder.Services.AddMvc(options =>
{
   options.SuppressAsyncSuffixInActionNames = false;
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.MapControllers();

MigrationImplementer.PrepPopulation(app);

app.Run();
