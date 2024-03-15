using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


#region P2P (Point-to-Point)
//string queueName = "Example-P2P-Queue";
//channel.QueueDeclare(queue: queueName, exclusive: false, durable: false, autoDelete: false);

//byte[] message = Encoding.UTF8.GetBytes("Hallo, guten Tag...");
//channel.BasicPublish(exchange: "", routingKey: queueName, body: message);
#endregion

#region Pub/Sub (Publish/Subscribe)
//string exchangeName = "example-pub-sub-exchange";
//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes($"Hallo {i + 1}");

//    channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, body: message);
//}
#endregion

#region Work Queue
//string queueName = "example-work-queue";
//channel.QueueDeclare(queue: queueName, exclusive: false, durable: false, autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes($"Hallo {i + 1}");

//    channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: message);
//}
#endregion

#region Request/Response
string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    exclusive: false,
    durable: false,
    autoDelete: false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

#region Request Mesajini Olusturma ve Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hallo {i + 1}");

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}
#endregion

#region Response Kuyrugu Dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        string message = Encoding.UTF8.GetString(e.Body.Span);
        Console.WriteLine(message);
    }
};

#endregion


#endregion

Console.Read();

