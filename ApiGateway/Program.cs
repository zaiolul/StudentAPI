using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json");
builder.Services.AddOcelot();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().
                          AllowAnyHeader().
                          AllowAnyMethod();
                      });
});
var app = builder.Build();

app.UseOcelot().Wait();

//app.UseCors(builder => builder
//     .AllowAnyOrigin()
//     .AllowAnyMethod()
//     .AllowAnyHeader()
//     .AllowCredentials());

app.UseCors(MyAllowSpecificOrigins);

app.Run();
