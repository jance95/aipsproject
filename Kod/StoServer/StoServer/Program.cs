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
        private static void Rotate(List<string> lista, int p)
        {
            while (p > 0)
            {
                string pom = lista[0];
                lista.RemoveAt(0);
                lista.Add(pom);
                p--;
            }
        }
        public static void Main(String[] args)
        {
            Console.WriteLine(args[0]);
            bool partijaUToku = false,start=false;
            int period = 0;
            List<String> igraci=new List<string>();
            List<String> igraciNaCekanju = new List<string>();
            List<String> vlasniciKarata = new List<string>();
            List<int> karteNaStolu = new List<int>();
            List<int> karteIgraca = new List<int>();
            int trenutni=0;
            StoServer.IModel klasa = new KlasicneKarte();
            Sto s = klasa.vratiSto(Int32.Parse(args[0]));
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
                    String[] p = message.Split(':');
                    Console.WriteLine(" [x] {0}", message);
                    Console.WriteLine(" [x] {0}", ea.RoutingKey);
                    if (ea.RoutingKey == "in")
                    {
                        s.trenutniBrIgraca++;
                        var message1 = "";
                        var body1 = Encoding.UTF8.GetBytes(message1);
                        if (partijaUToku)
                        {
                            igraciNaCekanju.Add(p[1]);
                            message1 += "@";
                        }

                        else
                            igraci.Add(p[1]);
                        message1+= "Igrac " + p[1] + " se pridruzio tabli."; 
                        //channel.BasicPublish(exchange: args[0],
                        //                     routingKey: "",
                        //                     basicProperties: null,
                        //                     body: body1);
                        //Console.WriteLine(" [x] Sent {0}", message);
                        if (!start && igraci.Count > 1)
                        {
                            period++;
                            message1 += "%igracNaPotezu:" + igraci[0];
                            start = true;
                            partijaUToku = true;
                            message1 += ":";
                            List<int> karte = new List<int>();
                            List<int> karte1 = new List<int>();
                            karte = klasa.vratiKarte(igraci.Count * 2);
                            karte1 = klasa.vratiKarte(3);
                            message1 += "%karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
                            karteNaStolu.AddRange(karte1);
                            vlasniciKarata.AddRange(igraci);
                            karteIgraca.AddRange(karte);
                            foreach (string x in igraci)
                            {
                                message1 += x + "." + karte[0] + "." + karte[1] + ",";
                                karte.RemoveAt(0);
                                karte.RemoveAt(0);
                            }
                            message1=message1.Remove(message1.Length - 1, 1);
                            }
                        body1 = Encoding.UTF8.GetBytes(message1);
                        
                        channel.BasicPublish(exchange: args[0],
                                       routingKey: "",
                                       basicProperties: null,
                                       body: body1);
                        Console.WriteLine(" [x] Sent {0}", message1);
                    }
                    if (ea.RoutingKey=="leave")
                    {
                        Igrac i = klasa.vratiIgraca(p[1]);
                        i.sto = null;
                        klasa.updateIgrac(i);
                        s.trenutniBrIgraca--;
                        klasa.UpdateSto(s);
                        string message1 = "Igrac " + i.username + " je napustio igru.";
                        //var body1 = Encoding.UTF8.GetBytes(message1);
                        //channel.BasicPublish(exchange: args[0],
                        //                   routingKey: "",
                        //                   basicProperties: null,
                        //                   body: body1);
                        //Console.WriteLine(" [x] Sent {0}", message1);
                        if (igraci.Contains(i.username))
                        {
                            if (igraci.IndexOf(i.username) <= trenutni)
                                trenutni--;
                            igraci.Remove(i.username);
                        }
                        else
                        {
                            igraciNaCekanju.Remove(i.username);
                        }
                        //if (igraci.Count < 2  && igraciNaCekanju.Count == 0)
                        //{
                        //    start = false;
                        //    partijaUToku = false;
                        //}
                        if(igraci.Count<2)
                        {
                            if(igraci.Count==1)
                            {
                                 message1 += "Igrac " +igraci[0] + " je dobio ruku.";
                                // body1 = Encoding.UTF8.GetBytes(message1);
                                //channel.BasicPublish(exchange: args[0],
                                //                   routingKey: "",
                                //                   basicProperties: null,
                                //                   body: body1);
                                //Console.WriteLine(" [x] Sent {0}", message1);
                                start = false;
                                partijaUToku = false;
                            }
                            if (igraciNaCekanju.Count > 0)
                            {
                                for (int ind = 0; ind < igraciNaCekanju.Count; ind++)
                                    igraci.Add(igraciNaCekanju[ind]);
                                igraciNaCekanju.Clear();
                                if (igraci.Count > 1)
                                {
                                    start = true;
                                    partijaUToku = true;
                                }
                                else
                                {
                                    start = false;
                                    partijaUToku = false;
                                }
                                message1 += "%igracNaPotezu:" + igraci[0];
                            }
                            
                        }
                        var body1 = Encoding.UTF8.GetBytes(message1);
                        channel.BasicPublish(exchange: args[0],
                                           routingKey: "",
                                           basicProperties: null,
                                           body: body1);
                        Console.WriteLine(" [x] Sent {0}", message1);
                    }
                    if(ea.RoutingKey=="check")
                    {
                        string message1="";
                        trenutni++;
                        if (trenutni == igraci.Count)
                        {
                            trenutni = 0;
                            period++;
                            //Console.WriteLine(period);
                            if(period==5)
                            {
                                period = 0;
                                klasa.Karte.Clear();
                                List<int> poeni = new List<int>();
                                foreach (string ig in vlasniciKarata)
                                {
                                    klasa.Karte.AddRange(karteNaStolu);
                                    klasa.Karte.Add(karteIgraca[0]);
                                    klasa.Karte.Add(karteIgraca[1]);
                                    karteIgraca.RemoveAt(0);
                                    karteIgraca.RemoveAt(0);
                                    poeni.Add(klasa.vratiPoene());
                                    klasa.Karte.Clear();
                                }
                                string igr=vlasniciKarata[poeni.IndexOf(poeni.Max())];
                                Console.WriteLine(igr);
                                message1 = "Igrac:" + igr + ":je dobio ovu ruku";
                                if (igraciNaCekanju.Count > 0)
                                {
                                    igraci.AddRange(igraciNaCekanju);
                                    igraciNaCekanju.Clear();
                                }
                                List<int> karte = new List<int>();
                                List<int> karte1 = new List<int>();
                                karte = klasa.vratiKarte(igraci.Count * 2);
                                karte1 = klasa.vratiKarte(3);
                                message1 += ":karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
                                karteNaStolu.Clear();
                                karteNaStolu.AddRange(karte1);
                                vlasniciKarata.Clear();
                                vlasniciKarata.AddRange(igraci);
                                karteIgraca.Clear();
                                karteIgraca.AddRange(karte);
                                foreach (string x in igraci)
                                {
                                    message1 += x + "." + karte[0] + "." + karte[1] + ",";
                                    karte.RemoveAt(0);
                                    karte.RemoveAt(0);
                                }
                                message1 = message1.Remove(message1.Length - 1, 1);
                                period++;
                                message1 += ":";
                            }
                            else
                            {
                                message1 = "*";
                            }
                        }
                         message1 += "%Igrac :" + p[1]+ ":check:sledeci:"+igraci[trenutni];
                         var body1 = Encoding.UTF8.GetBytes(message1);

                        channel.BasicPublish(exchange: args[0],
                                           routingKey: "",
                                           basicProperties: null,
                                           body: body1);
                        Console.WriteLine(" [x] Sent {0}", message1);
                    }
                    if (ea.RoutingKey == "call")
                    {
                        string message1 = "";
                        trenutni++;
                        if (trenutni == igraci.Count)
                        {
                            trenutni = 0;
                            period++;
                            Console.WriteLine(period);
                            if (period == 5)
                            {
                                period = 0;
                                klasa.Karte.Clear();
                                List<int> poeni = new List<int>();
                                foreach (string ig in vlasniciKarata)
                                {
                                    klasa.Karte.AddRange(karteNaStolu);
                                    klasa.Karte.Add(karteIgraca[0]);
                                    klasa.Karte.Add(karteIgraca[1]);
                                    karteIgraca.RemoveAt(0);
                                    karteIgraca.RemoveAt(0);
                                    poeni.Add(klasa.vratiPoene());
                                    klasa.Karte.Clear();
                                }
                                string igr = vlasniciKarata[poeni.IndexOf(poeni.Max())];
                                Console.WriteLine(igr);
                                message1 = "Igrac:" + igr + ":je dobio ovu ruku";
                                if (igraciNaCekanju.Count > 0)
                                {
                                    igraci.AddRange(igraciNaCekanju);
                                    igraciNaCekanju.Clear();
                                }
                                List<int> karte = new List<int>();
                                List<int> karte1 = new List<int>();
                                karte = klasa.vratiKarte(igraci.Count * 2);
                                karte1 = klasa.vratiKarte(3);
                                message1 += ":%karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
                                karteNaStolu.Clear();
                                karteNaStolu.AddRange(karte1);
                                vlasniciKarata.Clear();
                                vlasniciKarata.AddRange(igraci);
                                karteIgraca.Clear();
                                karteIgraca.AddRange(karte);
                                foreach (string x in igraci)
                                {
                                    message1 += x + "." + karte[0] + "." + karte[1] + ",";
                                    karte.RemoveAt(0);
                                    karte.RemoveAt(0);
                                }
                                message1 = message1.Remove(message1.Length - 1, 1);
                                period++;
                                message1 += ":";
                            }
                            else
                            {
                                message1 = "*";
                            }
                        }
                        message1 += "Igrac :" + p[1] + ":call:sledeci:"+igraci[trenutni];
                        var body1 = Encoding.UTF8.GetBytes(message1);

                        channel.BasicPublish(exchange: args[0],
                                           routingKey: "",
                                           basicProperties: null,
                                           body: body1);
                        Console.WriteLine(" [x] Sent {0}", message1);
                    }
                    if (ea.RoutingKey == "fold")
                    {
                        string message1 = "";
                        int pom = trenutni;
                        if (trenutni == igraci.Count - 1)
                        {
                            trenutni = 0;
                            period++;
                            //Console.WriteLine(period);
                            if (period == 5)
                            {
                                period = 0;
                                klasa.Karte.Clear();
                                List<int> poeni = new List<int>();
                                foreach (string ig in vlasniciKarata)
                                {
                                    klasa.Karte.AddRange(karteNaStolu);
                                    klasa.Karte.Add(karteIgraca[0]);
                                    klasa.Karte.Add(karteIgraca[1]);
                                    karteIgraca.RemoveAt(0);
                                    karteIgraca.RemoveAt(0);
                                    poeni.Add(klasa.vratiPoene());
                                    klasa.Karte.Clear();
                                }
                                string igr = vlasniciKarata[poeni.IndexOf(poeni.Max())];
                                Console.WriteLine(igr);
                                message1 = "Igrac:" + igr + ":je dobio ovu ruku";
                                if (igraciNaCekanju.Count > 0)
                                {
                                    igraci.AddRange(igraciNaCekanju);
                                    igraciNaCekanju.Clear();
                                }
                                List<int> karte = new List<int>();
                                List<int> karte1 = new List<int>();
                                karte = klasa.vratiKarte(igraci.Count * 2);
                                karte1 = klasa.vratiKarte(3);
                                message1 += ":%karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
                                karteNaStolu.Clear();
                                karteNaStolu.AddRange(karte1);
                                vlasniciKarata.Clear();
                                vlasniciKarata.AddRange(igraci);
                                karteIgraca.Clear();
                                karteIgraca.AddRange(karte);
                                foreach (string x in igraci)
                                {
                                    message1 += x + "." + karte[0] + "." + karte[1] + ",";
                                    karte.RemoveAt(0);
                                    karte.RemoveAt(0);
                                }
                                message1 = message1.Remove(message1.Length - 1, 1);
                                period++;
                                message1 += ":";
                            }
                            else
                            {
                                message1 = "*";
                            }
                        }
                            igraciNaCekanju.Add(igraci[pom]);
                        igraci.RemoveAt(pom);
                        int ind=vlasniciKarata.IndexOf(p[1]);
                        vlasniciKarata.RemoveAt(ind);
                        karteIgraca.RemoveAt(2*ind);
                        karteIgraca.RemoveAt(2*ind);

                        message1 += "Igrac :" + p[1] + ":fold:sledeci:" + igraci[trenutni];
                        var body1 = Encoding.UTF8.GetBytes(message1);

                        channel.BasicPublish(exchange: args[0],
                                           routingKey: "",
                                           basicProperties: null,
                                           body: body1);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                    if (ea.RoutingKey == "rise")
                    {
                        foreach (string x in igraci)
                            Console.Write(x + " ");
                        Console.WriteLine();
                        Rotate(igraci, trenutni);
                        foreach (string x in igraci)
                            Console.Write(x + " ");
                        Console.WriteLine();
                        trenutni = 1;
                        string message1 = "Igrac :" + p[1] + ":sledeci:" + igraci[trenutni] + ":rise:" + p[3];
                        var body1 = Encoding.UTF8.GetBytes(message1);

                        channel.BasicPublish(exchange: args[0],
                                           routingKey: "",
                                           basicProperties: null,
                                           body: body1);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
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
