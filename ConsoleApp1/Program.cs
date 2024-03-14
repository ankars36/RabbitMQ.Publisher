using RabbitMQ.Client;
using System.Text;

//Baglanti olusturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://joyrhfww:OrRKGmmKZv2_JULc8tyrHt6K0D3aaSpC@cow.rmq2.cloudamqp.com/joyrhfww");

//Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue olusturma
channel.QueueDeclare(queue: "Example-Queue-1", exclusive: false);

//Queue ya mesaj gönderme
byte[] message = Encoding.UTF8.GetBytes("Hallo, guten Tag...");
channel.BasicPublish(exchange: "", routingKey: "Example-Queue-1", body: message);

Console.Read();

