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
            // Return the list of locations as a response
            return Ok(allLocations);
        }

        [HttpPost]
        public IActionResult PostLocation([FromBody] DriverLocation location)
        {
            // Send location to RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = "0.tcp.in.ngrok.io",  // Ngrok public hostname
                Port = 19881,                    // Ngrok forwarded port
                UserName = "guest",              // Default RabbitMQ username
                Password = "guest"               // Default RabbitMQ password
            };

            //var collection = _mongoDbService.GetCollection<DriverLocation>("DriversLocation");
            //collection.InsertOneAsync(location);

            //  var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:Host"] };
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

