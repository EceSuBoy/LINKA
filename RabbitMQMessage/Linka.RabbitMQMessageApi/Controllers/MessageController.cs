using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Linka.RabbitMQMessageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateMessage()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"      
            };

            var connection = connectionFactory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("Queue2", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var messageContent = "The weather is hot today.";

            var byteMessageContent = Encoding.UTF8.GetBytes(messageContent);

            channel.BasicPublish(exchange: "", routingKey: "Queue2", basicProperties: null, body: byteMessageContent);

            return Ok("Your message is in queue.");
        }

        private static string message;

        [HttpGet]
        public IActionResult ReadMessage()
        {
            var factory = new ConnectionFactory();

            factory.HostName = "localhost";

            var connection = factory.CreateConnection();    
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, x) =>
            {
                var byteMessage = x.Body.ToArray();
                message = Encoding.UTF8.GetString(byteMessage);
                
            };
            channel.BasicConsume(queue: "Queue1", autoAck: false, consumer:consumer);

            if (string.IsNullOrEmpty(message))
            {
                return NoContent();

            }
            else
            {
                return Ok(message);
            }
                
        }
    }
}
