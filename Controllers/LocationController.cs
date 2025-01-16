using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RescueRide.Data;
using RescueRide.Services;

namespace RescueRide.Controllers
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
        public IActionResult PostLocation([FromBody] DriverLocation location)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            // Read RabbitMQ settings
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
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _configuration["RabbitMQ:QueueName"],
                                     basicProperties: null,
                                     body: body);
            }
            return Ok();
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

