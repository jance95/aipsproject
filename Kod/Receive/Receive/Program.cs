using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Receive.Model;
using Receive.Entiteti;
using Receive;

class Worker
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs1", type: "direct");
            channel.ExchangeDeclare(exchange: "logs2", type: "direct");

            //var queueName = channel.QueueDeclare().QueueName;
            channel.QueueDeclare(queue: "jana1",
                                         durable: false,
                                         exclusive: false,
                                        autoDelete: false,
                                         arguments: null);
            channel.QueueBind(queue: "jana1",
                              exchange: "logs1",
                              routingKey: "jana");
            channel.QueueBind(queue: "jana1",
                              exchange: "logs1",
                              routingKey: "janaZ");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Receive.Model.IModel klasa = new KlasicneKarte();
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
                Console.WriteLine(" [x] {0}", ea.RoutingKey);
                if (ea.RoutingKey == "jana")
                {
                    
                    String[] p = message.Split(':');
                    var message1 = "";
                    Object o = null;
                    Prikaz pr = new Prikaz();
                    Igrac i = klasa.vratiIgraca(p[1]);
                    if (i == null)
                        message1 = "Uneli ste pogresan username";
                    else if (i.password != p[3])
                        message1 = "Uneli ste pogresan password";
                    else
                    {
                        pr.i = i;
                        pr.stolovi = klasa.vratiStolove();
                    }
                    if (pr == null)
                        o = message1;
                    else o = pr;
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.TypeNameHandling = TypeNameHandling.Auto;
                    var body1 = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
                    channel.BasicPublish(exchange: "logs2",
                                         routingKey: p[p.Length - 1],
                                         basicProperties: null,
                                         body: body1);
                    Console.WriteLine(" [x] Sent {0}", message1);
                }
                else
                {
                    Console.WriteLine("ja sam ja" + ea.RoutingKey);
                    int id = Int32.Parse(message);
                    klasa.
                }
            };
                channel.BasicConsume(queue: "jana1",
                                     autoAck: true,
                                     consumer: consumer);
            


            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}