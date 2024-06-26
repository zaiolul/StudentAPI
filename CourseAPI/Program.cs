﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CourseAPI.Data;
using CourseAPI;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CourseAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseAPIContext") ?? throw new InvalidOperationException("Connection string 'CourseAPIContext' not found.")));
builder.Services.AddSingleton<Publisher>(s => new Publisher(
    builder.Configuration["MQ:Host"],
    builder.Configuration["MQ:UserName"],
    builder.Configuration["MQ:Password"],
    "test",
    "test_topic"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["MQ:Host"]!, h =>
        {
            h.Username(builder.Configuration["MQ:UserName"]);
            h.Password(builder.Configuration["MQ:Password"]);
        });
        configurator.ConfigureEndpoints(context);
    });
});


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
