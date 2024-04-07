using Microsoft.EntityFrameworkCore;
using Notebook.Application.Services.Contracts;
using Notebook.Application.Services.Implementation;
using Notebook.DataAccess;
using Notebook.Repositories.Contracts;
using Notebook.Repositories.Implementation;
using Notebook.WebApi;
using Notebook.WebApi.RabbitMQ;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Serilog;
using ILogger = Serilog.ILogger;
using System.Text;
using Notebook.WebApi.Requests;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json");

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MongoDB(databaseUrl: builder.Configuration.GetConnectionString("MongoDBconnection"), collectionName: "AppLogs")
    .CreateLogger();

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

RabbitMQConsumer();

app.Run();

void RabbitMQConsumer()
{
    var factory = new ConnectionFactory { HostName = "localhost" };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.QueueDeclare(queue: "ForAdding",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += async (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Console.WriteLine(message.ToString());

        var contact = JsonSerializer.Deserialize<ContactForCreateUpdateDTO>(message);

        using var scope = app.Services.CreateScope();
        var serviceManager = scope.ServiceProvider.GetService<IServiceManager>();

        try
        {
            await serviceManager.ContactService.CreateContactAsync(contact.FirstName, contact.LastName, contact.PhoneNumber, contact.Email, contact.DateOfBirth);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.InnerException.Message);
        } 
    };

    channel.BasicConsume(queue: "ForAdding",
                         autoAck: true,
                         consumer: consumer);
}
