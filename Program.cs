using RescueRide.Data;
using Microsoft.EntityFrameworkCore;
using RescueRide.Models;
using RescueRide.Services;
using RescueRide.SignalR;
using RescueRide.Repositories;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<RabbitMQConsumerService>();  // Singleton for RabbitMQ consumer service
builder.Services.AddHostedService<RabbitMQConsumerWorker>();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

// Register MongoDbService with DI container
builder.Services.AddSingleton<MongoDbService>();


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
app.MapGet("/", context =>
{
   context.Response.Redirect("/home/index.html");
    return Task.CompletedTask;
});
app.UseHttpsRedirection();
app.MapHub<LocationHub>("/locationHub");
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "home")),
//    RequestPath = "/home"
//});

app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
