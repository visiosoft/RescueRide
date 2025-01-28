using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RescueRide.Application.Services;

namespace RescueRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MongoDbService _mongoDbService;

        public LocationController(IConfiguration configuration, MongoDbService mongoDbService)
        {
            _configuration = configuration;
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public IActionResult GetAllLocations()
        {
            var allLocations = _mongoDbService.GetCollection<DriverLocation>("DriverLocations").AsQueryable().ToList();
            return Ok(allLocations);
        }

        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] DriverLocation location)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var rabbitMqConfig = configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqConfig["HostName"],
                Port = int.Parse(rabbitMqConfig["Port"]),
                UserName = rabbitMqConfig["UserName"],
                Password = rabbitMqConfig["Password"]
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"],
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonConvert.SerializeObject(location);

                await _mongoDbService.InsertDocumentAsync(location, "DriverLocations");
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _configuration["RabbitMQ:QueueName"],
                                     basicProperties: null,
                                     body: body);
            }
            return Ok("Document inserted and message published to RabbitMQ.");
        }

        [HttpPatch]
        public async Task<IActionResult> updateLocation([FromBody] DriverLocation location)
        {
            if (location == null || string.IsNullOrEmpty(location.DriverId))
            {
                return BadRequest("Invalid location data.");
            }

            try
            {
                var collection = _mongoDbService.GetCollection<DriverLocation>("DriverLocations");

                var filter = Builders<DriverLocation>.Filter.Eq(d => d.DriverId, location.DriverId);

                var update = Builders<DriverLocation>.Update
                    .Set(d => d.Latitude, location.Latitude)
                    .Set(d => d.Longitude, location.Longitude)
                    .Set(d => d.Timestamp, DateTime.UtcNow);

                var updateResult = await collection.UpdateOneAsync(filter, update);

                if (updateResult.MatchedCount == 0)
                {
                    return NotFound("Driver not found.");
                }

                return Ok("Location updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

    public class DriverLocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }

    }
}

