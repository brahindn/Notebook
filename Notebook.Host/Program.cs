using Microsoft.EntityFrameworkCore;
using NLog;
using Notebook.Application.Services.Contracts;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Repositories.Contracts;
using Notebook.Repositories.Implementation;
using Notebook.WebApi;
using Serilog;
using ILogger = Serilog.ILogger;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json");

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MongoDB(databaseUrl: "mongodb://localhost:27017/NotebookLogDB", collectionName: "AppLogs")
    .CreateLogger();

builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

builder.Services.AddSingleton<ILogger>(logger);
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyReference).Assembly);

builder.Services.AddOpenApiDocument();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
