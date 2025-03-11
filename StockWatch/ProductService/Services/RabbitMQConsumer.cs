using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ProductServiceAPI.Models;

namespace ProductServiceAPI.Services
{
    public class RabbitMQConsumer
    {
        private readonly IEmailService _emailService;

        public RabbitMQConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "low-stock-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockAlert = JsonSerializer.Deserialize<StockAlertMessage>(message);

                _emailService.SendEmail("admin@stockwatch.com", "Düşük Stok Uyarısı",
                    $"Ürün: {stockAlert.ProductName} stok seviyesi {stockAlert.CurrentStock}'e düştü.");
            };

            channel.BasicConsume(queue: "low-stock-queue",
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
