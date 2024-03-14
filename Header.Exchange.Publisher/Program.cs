using RabbitMQ.Client;
using System.Text;

//Baglanti olusturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

//Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "header-example", type: ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] byteMessage = Encoding.UTF8.GetBytes($"Merhaba {i + 1}");

    Console.WriteLine("Header value giriniz:");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>
    {
        ["no"] = value
    };

    channel.BasicPublish(exchange: "header-example", routingKey: string.Empty, body: byteMessage, basicProperties: basicProperties);
}

Console.Read();