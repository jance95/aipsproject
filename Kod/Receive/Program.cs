using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Receive.Model;
using Receive.Entiteti;
using Receive;
using System.Diagnostics;

class Worker
{
    public static void Main()
    {
        Receive.Model.IModel klasa = new KlasicneKarte();
        //Dictionary<int,Sto> stolovi=new Dictionary<int, Sto>();
        //IList<Sto> pom = klasa.vratiStolove();
        //foreach (Sto s in pom)
        //    stolovi.Add(s.id, s);
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
                
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                String[] p = message.Split(':');
                Console.WriteLine(" [x] {0}", message);
                Console.WriteLine(" [x] {0}", ea.RoutingKey);
                if (ea.RoutingKey == "jana")
                { 
                    var message1 = "";
                    Object o = null;
                    Prikaz pr=null;
                    Igrac i = klasa.vratiIgraca(p[1]);
                    if (i == null)
                        message1 = "Uneli ste pogresan username";
                    else if (i.password != p[3])
                        message1 = "Uneli ste pogresan password";
                    else
                    {
                        pr = new Prikaz();
                        pr.i = i;
                        pr.stolovi = klasa.vratiStolove();
                    }
                    if (pr == null)
                        o = message1;
                    else o = pr;
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.TypeNameHandling = TypeNameHandling.Auto;
                    var body1 = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o,Formatting.Indented,new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    channel.BasicPublish(exchange: "logs2",
                                         routingKey: p[p.Length - 1],
                                         basicProperties: null,
                                         body: body1);
                    Console.WriteLine(" [x] Sent {0}", message1);
                }
                else
                {
                    Console.WriteLine("ja sam ja" + ea.RoutingKey);
                    int id = Int32.Parse(p[3]);
                    Sto s = klasa.vratiSto(id);
                    Igrac i = klasa.vratiIgraca(p[1]);
                    String message1;
                    //if (s.trenutniBrIgraca == 0)
                    //{
                    //    Process pr = new Process();
                    //    pr.StartInfo.Arguments = s.id.ToString();
                    //    pr.StartInfo.FileName = @"C:\Users\Nikola\Documents\Visual Studio 2015\Projects\StoServer\StoServer\bin\Release\StoServer.exe";
                    //    pr.Start();
                    //}
                    if (s.trenutniBrIgraca < s.maxBrIgraca)
                    {
                        s.trenutniBrIgraca++;
                        s.igraci.Add(i);
                        i.sto = s;
                        klasa.updateIgrac(i);
                        klasa.UpdateSto(s);

                        message1 = "Uspesno";
                    }
                    else
                    {
                        message1 = "Neuspesno";
                    }
                    var body1 = Encoding.UTF8.GetBytes(message1);
                    channel.BasicPublish(exchange: "logs2",
                                        routingKey: p[p.Length - 1],
                                        basicProperties: null,
                                        body: body1);
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