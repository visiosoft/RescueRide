using RescueRide.Data;
using Microsoft.EntityFrameworkCore;
using RescueRide.Models;
using RescueRide.Services;
using RescueRide.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();

builder.Services.AddTransient<RabbitMQConsumerService>();  // Singleton for RabbitMQ consumer service
builder.Services.AddHostedService<RabbitMQConsumerWorker>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()     // Allow all origins
                      .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});
app.UseHttpsRedirection();
app.MapHub<LocationHub>("/locationHub");

app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
