using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Xend.Data;
using Xend.EventHandlers;
using Xend.MessageBrokers;
using Xend.MessageHandlers;
using Xend.Models;
using Xend.Repositories;
using Xend.Services;

static bool DisableCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
{
    return true; // Accept all certificates (disable validation)
}

var builder = WebApplication.CreateBuilder(args);



// Disable certificate validation
System.Net.ServicePointManager.ServerCertificateValidationCallback += DisableCertificateValidationCallback;

builder.WebHost.ConfigureKestrel((hostingContext, options) =>
{
    options.Listen(IPAddress.Loopback, 4000); // Set the desired IP address and port number
});

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var exchangeName = "transactions_exchange";

// Configure services

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));




var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

builder.Services.AddSingleton<IConnection>(provider =>
{
    var factory = new ConnectionFactory
    {
        HostName = rabbitMQConfig.HostName,
        Port = rabbitMQConfig.Port,
        UserName = rabbitMQConfig.UserName,
        Password = rabbitMQConfig.Password
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(provider =>
    provider.GetRequiredService<IConnection>().CreateModel());

builder.Services.AddSingleton<IMessageBroker>(provider =>
    new RabbitMQMessageBroker(provider.GetRequiredService<IModel>(), exchangeName));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IEventHandler<TransactionReceivedEvent>, TransactionReceivedEventHandler>();
builder.Services.AddScoped<UpdateTransactionsCommandHandler>();
builder.Services.AddHttpClient<ICryptoApiClient, CryptoApiClient>();
builder.Services.AddSingleton<IEventBus>(provider =>
{
    var connection = provider.GetRequiredService<IConnection>();
    return new EventBus(connection, exchangeName);
});
builder.Services.AddControllers();

// Create RabbitMQ connection
var serviceProvider = builder.Services.BuildServiceProvider();
var connection = serviceProvider.GetService<IConnection>();
var channel = connection.CreateModel();


// Gracefully stop the message consumer on application shutdown
var appLifetime = serviceProvider.GetService<IHostApplicationLifetime>();
appLifetime.ApplicationStopping.Register(() =>
{
    channel.Close();
    channel.Dispose();
    connection.Close();
    connection.Dispose();
});

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

app.Run();
