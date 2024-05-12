using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentAPI;
using StudentAPI.Data;
using StudentsAPI.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentAPIContext") ?? throw new InvalidOperationException("Connection string 'StudentAPIContext' not found.")));

builder.Services.AddSingleton<Subscriber>(s => new Subscriber(
    builder.Configuration["MQ:Host"],
    builder.Configuration["MQ:UserName"],
    builder.Configuration["MQ:Password"],
    "test",
    "testq",
    "test_topic"));

builder.Services.AddHostedService<EnrollmentDataCollector>();
// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
