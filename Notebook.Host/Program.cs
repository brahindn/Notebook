using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Contracts;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Repositories.Contracts;
using Notebook.Repositories.Implementation;
using Notebook.WebApi;
using Notebook.WebApi.RabbitMQ;
using Serilog;
using ILogger = Serilog.ILogger;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json");

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MongoDB(databaseUrl: builder.Configuration.GetConnectionString("MongoDBconnection"), collectionName: "AppLogs")
    .CreateLogger();

builder.Services.AddHostedService<AddConsumer>();
builder.Services.AddHostedService<UpdateConsumer>();
builder.Services.AddHostedService<DeleteConsumer>();

builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddSingleton<ILogger>(logger);
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<MessageProducer>();


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