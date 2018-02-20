using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StoServer.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StoServer
{
    class Program
    {
        public static void Main(String[] args)
        {
           Console.WriteLine(args[0]);
            Context c = new Context(args[0]);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string ex = "igrac" + args[0];
                channel.ExchangeDeclare(exchange: args[0], type: "fanout");
                channel.ExchangeDeclare(exchange: ex, type: "direct");

                //var queueName = channel.QueueDeclare().QueueName;
                channel.QueueDeclare(queue: args[0],
                                             durable: false,
                                             exclusive: false,
                                            autoDelete: false,
                                             arguments: null);
                channel.QueueBind(queue: args[0],
                                  exchange: "igrac"+ args[0],
                                  routingKey: "rise");
                channel.QueueBind(queue: args[0],
                                  exchange: "igrac" + args[0],
                                  routingKey: "fold");
                channel.QueueBind(queue: args[0],
                                  exchange: "igrac" + args[0],
                                  routingKey: "leave");
                channel.QueueBind(queue: args[0],
                                  exchange: "igrac" + args[0],
                                  routingKey: "check");
                channel.QueueBind(queue: args[0],
                                  exchange: "igrac" + args[0],
                                  routingKey: "call");
                channel.QueueBind(queue: args[0],
                                  exchange: "igrac" + args[0],
                                  routingKey: "in");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                    Console.WriteLine(" [x] {0}", ea.RoutingKey);
                    Factory f = new MessageHandlerCreator();
                    MessageHandler mh = f.FactoryMethod(ea.RoutingKey);
                    String m = mh.ObradiPoruku(message, c);
                    //var body1 = Encoding.UTF8.GetBytes(m);
                    LoggerFactory lf = new LoggerCreator();
                    Logger logger = lf.FactoryMethod();
                    logger.Log(m, args[0], channel);
                    //channel.BasicPublish(exchange: args[0],
                    //               routingKey: "",
                    //               basicProperties: null,
                    //               body: body1);
                    //Console.WriteLine(" [x] Sent {0}", m);
                };
                channel.BasicConsume(queue: args[0],
                                     autoAck: true,
                                     consumer: consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

          
        }
    }
}
