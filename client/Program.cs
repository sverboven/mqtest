using client.Models;
using client.Services;
using client.Workers;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var AppHost = new GenericAppHost(typeof(MyService).Assembly)
        {
            ConfigureAppHost = host =>
            {
                var mqServer = new RabbitMqServer(hostContext.Configuration.GetConnectionString("RabbitMq")) {DisablePublishingToOutq = true,};
                mqServer.RegisterHandler<Hello>(host.ExecuteMessage);
                host.Register<IMessageService>(mqServer);
            }
        }.Init();

        services.AddSingleton(AppHost.Resolve<IMessageService>());
        services.AddHostedService<MqWorker>();
    }).Build().StartAsync();

app.Run();