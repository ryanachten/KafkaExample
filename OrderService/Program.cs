using Microsoft.AspNetCore.Mvc;
using Common;
using OrderService.Services;
using OrderService.Data;
using OrderService.Repositories;
using OrderService.Models;
using OrderService.Models.DTOs;
using OrderService.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Configure<KafkaConfiguration>(
    builder.Configuration.GetRequiredSection(KafkaConfiguration.SectionName));
builder.Services.Configure<ConsumerRetryConfiguration>(
    builder.Configuration.GetRequiredSection(ConsumerRetryConfiguration.SectionName));

builder.Services.Configure<OutboxOptions>(
    builder.Configuration.GetRequiredSection(OutboxOptions.SectionName));

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();

builder.Services.AddSingleton<IOrderProducer, OrderProducer>();
builder.Services.AddScoped<IOrderService, OrderService.Services.OrderService>();

builder.Services.AddHostedService<OrderOutboxWorker>();
builder.Services.AddHostedService<OrderFulfilledConsumer>();

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("OrdersDatabase")
    ?? throw new InvalidOperationException("OrdersDatabase connection string is not configured");

try
{
    DatabaseMigrator.MigrateDatabase(connectionString, app.Logger);
}
catch (Exception ex)
{
    app.Logger.LogError("Migration failed: {Error}", ex.Message);
}

app.MapPost("/orders", async ([FromBody] CreateOrderRequest order, IOrderService service) => await service.CreateOrder(new Order(order)));

app.Logger.LogInformation("Waiting for workers to initialize...");
await Task.Delay(10000);
app.Logger.LogInformation("Starting HTTP server");

await app.RunAsync();