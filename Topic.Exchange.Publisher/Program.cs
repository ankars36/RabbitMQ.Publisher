using RabbitMQ.Client;
using System.Text;

//Baglanti olusturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

//Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-example", type: ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] byteMessage = Encoding.UTF8.GetBytes($"Merhaba {i + 1}");

    Console.WriteLine("Topic giriniz:");
    string topic = Console.ReadLine();

    channel.BasicPublish(exchange: "topic-example", routingKey: topic, body: byteMessage);
}

Console.Read();