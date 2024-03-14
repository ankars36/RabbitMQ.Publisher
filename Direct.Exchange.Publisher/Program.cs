using RabbitMQ.Client;
using System.Text;

//Baglanti olusturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

//Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-example", type: ExchangeType.Direct);

while (true)
{
    Console.WriteLine("Mesaj:");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "direct-example", routingKey: "direct-queue", body: byteMessage);
}

Console.Read();