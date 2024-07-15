using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Application.Mapping;
using Notebook.Application.Services.Contracts;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Host.Extensions;
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

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MongoDB(databaseUrl: builder.Configuration.GetConnectionString("MongoDBconnection"), collectionName: "AppLogs")
    .CreateLogger();

var rabbitSettings = new RabbitMqSettings();
rabbitSettings.StringHostName = builder.Configuration.GetConnectionString("RabbitMQConnection");

builder.Services.AddHostedService<AddConsumer>();
builder.Services.AddHostedService<UpdateConsumer>();
builder.Services.AddHostedService<DeleteConsumer>();

builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddSingleton<ILogger>(loggerConfiguration);
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<MessageProducer>();
builder.Services.AddSingleton(rabbitSettings);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAutoMapper(typeof(MappingProfile));
    
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyReference).Assembly);

builder.Services.AddOpenApiDocument();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var log = app.Services.GetRequiredService<ILogger>();
app.ConfigureExceptionHandler(log);

app.UseExceptionHandler(opt => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();