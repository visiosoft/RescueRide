using Microsoft.EntityFrameworkCore;
using RescueRide.Infrastructure.Security;
using RescueRide.Application.Services;
using RescueRide.Infrastructure.Repositories;
using RescueRide.API.SignalR;
using RescueRide.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? jwtSettings["Key"];
//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

string firebasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "firebase-adminsdk.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(firebasePath)
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<RabbitMQConsumerService>();  // Singleton for RabbitMQ consumer service
builder.Services.AddHostedService<RabbitMQConsumerWorker>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

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
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});
app.UseHttpsRedirection();
app.MapHub<LocationHub>("/locationHub");
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
