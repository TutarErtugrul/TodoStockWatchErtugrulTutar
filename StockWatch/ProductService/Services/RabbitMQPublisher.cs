using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

public class RabbitMQPublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMQPublisher()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
    }

    public void PublishStockAlert(int productId, int currentStock)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "low-stock-queue",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var message = new
        {
            ProductId = productId,
            CurrentStock = currentStock
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(exchange: "",
                             routingKey: "low-stock-queue",
                             basicProperties: null,
                             body: body);

    }
}
