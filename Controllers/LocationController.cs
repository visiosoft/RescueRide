using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RescueRide.Data;

namespace RescueRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllLocations()
        {
            // Retrieve all the driver locations from the static dictionary
            var allLocations = DriverLocationStore.DriverLocations.Values;

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
        public string DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }  

    }
}

