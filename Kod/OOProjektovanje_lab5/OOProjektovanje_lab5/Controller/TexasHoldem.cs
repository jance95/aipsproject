using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOProjektovanje_lab5.Entiteti;
using OOProjektovanje_lab5.Model;
using OOProjektovanje_lab5.View;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OOProjektovanje_lab5.Controller
{
    public class TexasHoldem : IController
    {
        private Model.IModel model;
        private IView view;
        //private int poeni = 0;
        public Igrac igr;
        public Sto s;
        public int Poeni
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public TexasHoldem(IView view, Model.IModel model)
        {
            this.view = view;
            this.model = model;
            view.AddListener(this);
        }
        public void izbaci(List<int> lk)
        {
            throw new NotImplementedException();
        }

        public void ocisti()
        {
            model.Karte.Clear();
        }

        public List<int> vratiKarte(int x)
        {
            return model.vratiKarte(x);
        }

        public int vratiPoene(int x)
        {
            bool kenta = daljeKenta();
            bool boja = daljeBoja();
            if (kenta && boja)
                return x * 150;
            bool dog = daljeDog(9,14);
            if (dog && boja)
                return x * 60;
            if (boja)
                return x * 40;
            if (kenta)
                return x * 26;
            if (daljePoker())
                return x * 90;
            if (daljeDog(2, 7))
                return x * 18;
            bool triling = daljeTriling();
            bool dvaPara = daljeDvapara();
            //if (triling && dvaPara)
            //    return x * 24;
            //if (daljeBlejz())
            //    return x * 9;
            if (triling)
                return x * 8;
            if (dvaPara)
                return x * 5;
            if (daljePar())
                return x * 1;


            return 0;
        }

        private bool daljeDog(int x,int y)
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
            {
                pom[i] = pom[i] / 10;
                if (pom[i] == 1)
                    pom[i] = 11;
            }
            pom.Sort();
            if (daljePar())
                return false;
            else
            {
                if (pom[0] >= x && pom[pom.Count-1] <= y)
                    return true;
                return false;
            }
        }
        private bool daljePar()
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            for (int i = 0; i < pom.Count - 1; i++)
                if (pom[i] == pom[i + 1])
                    return true;
            return false;
        }


        private bool daljeDvapara()
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            if ((pom[0] == pom[1] && pom[2] == pom[3]) || (pom[0] == pom[1] && pom[3] == pom[4]) || (pom[1] == pom[2] && pom[3] == pom[4]))
                return true;
            else
                return false;

        }

        private bool daljeTriling()
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            if ((pom[0] == pom[1] && pom[1] == pom[2]) || (pom[1] == pom[2] && pom[2] == pom[3]) || (pom[2] == pom[3] && pom[3] == pom[4]))
                return true;
            else
                return false;
        }

        private bool daljePoker()
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            if ((pom[0] == pom[1] && pom[1] == pom[2] && pom[2] == pom[3]) || (pom[1] == pom[2] && pom[2] == pom[3] && pom[3] == pom[4]))
                return true;
            else
                return false;
        }

        private bool daljeBoja()
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] % 10;
            for (int i = 0; i < pom.Count - 1; i++)
                if (pom[i] != pom[i + 1])
                    return false;
            return true;
        }

        private bool daljeKenta()
        {
            List<int> pom = model.Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            for (int i = 0; i < pom.Count - 1; i++)
                if (pom[i] == 10)
                {
                    if (pom[i] != (pom[i + 1]) - 2)
                        return false;
                }
                else
                if (pom[i] != (pom[i + 1] - 1))
                    return false;
            return true;
        }
        public List<int> vratiSve()
        {
            return model.Karte;
        }

        public Model.IModel getModel()
        {
            return model;
        }

        public void zameniModel(char m)
        {
            if (m == 'f')
                this.model = new Model.FrancuskeKarte();
            else
                if (m == 'k')
                this.model = new Model.KlasicneKarte();
            else
                this.model = new Model.NoveKarte();
        }

        public string proveriIgraca(string username,string pass)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs2", type: "direct");




                var queueName = channel.QueueDeclare().QueueName;
                //channel.QueueDeclare(queue: "nikola1",
                //                             durable: false,
                //                             exclusive: false,
                //                            autoDelete: false,
                //                             arguments: null);
                channel.QueueBind(queue: queueName,
                                  exchange: "logs2",
                                  routingKey: queueName);

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                bool primio = false;
                string messageR = "";
                consumer.Received += (model, ea) =>
                {
                    var body1 = ea.Body;
                    var message1 = Encoding.UTF8.GetString(body1);
                    messageR = message1;
                    //var s1 = JsonConvert.DeserializeObject<Igrac>(message1);
                    //if (s1.GetType() == typeof(String))
                    //    MessageBox.Show(s1.ToString());
                    //else
                    //    MessageBox.Show(((Igrac)s1).id + "\n" + ((Igrac)s1).Ime + "\n" + ((Igrac)s1).Prezime + "\n" + ((Igrac)s1).username + "\n" + ((Igrac)s1).novac);
                    primio = true;
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
                var message = "username:" + username + ": password:" + pass + ": routingKey:" + queueName;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs1",
                                     routingKey: "jana",
                                     basicProperties: null,
                                     body: body);
                while (!primio) ;
                return messageR;
            }
        }

        public IList<Sto> izlistajStolove()
        {
            throw new NotImplementedException();
        }

        public Igrac getIgrac()
        {
            return igr;
        }

        public Sto getSto()
        {
            return s;
        }

        public void setSto(Sto x)
        {
            s = x;
        }

        //public int poeni(int x)
        //{
        //    throw new NotImplementedException();
        //}

        public void dodajSto(string x, int y, int z)
        {
            throw new NotImplementedException();
        }

        public void setIgrac(Igrac i)
        {
            igr = i;
        }

    

        public string prijaviSto(int idStola, string usernameIgraca)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs2", type: "direct");




                var queueName = channel.QueueDeclare().QueueName;
                //channel.QueueDeclare(queue: "nikola1",
                //                             durable: false,
                //                             exclusive: false,
                //                            autoDelete: false,
                //                             arguments: null);
                channel.QueueBind(queue: queueName,
                                  exchange: "logs2",
                                  routingKey: queueName);

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                bool primio = false;
                string messageR = "";
                consumer.Received += (model, ea) =>
                {
                    var body1 = ea.Body;
                    var message1 = Encoding.UTF8.GetString(body1);
                    messageR = message1;
                    //var s1 = JsonConvert.DeserializeObject<Igrac>(message1);
                    //if (s1.GetType() == typeof(String))
                    //    MessageBox.Show(s1.ToString());
                    //else
                    //    MessageBox.Show(((Igrac)s1).id + "\n" + ((Igrac)s1).Ime + "\n" + ((Igrac)s1).Prezime + "\n" + ((Igrac)s1).username + "\n" + ((Igrac)s1).novac);
                    primio = true;
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
                var message = "usernameigraca:" + usernameIgraca + ": idstola:" + idStola + ": routingKey:" + queueName;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs1",
                                     routingKey: "janaZ",
                                     basicProperties: null,
                                     body: body);
                while (!primio) ;
                return messageR;
            }
        }

        public void takeIn()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "igrac" + s.id.ToString(), type: "direct");

                var message = "Igrac :" + igr.username + ":novac:" + igr.novac;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "igrac" + s.id.ToString(),
                                     routingKey: "in",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void takeout()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "igrac" + s.id.ToString(), type: "direct");

                var message = "username:" + igr.username + ":novac:" + igr.novac;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "igrac" + s.id.ToString(),
                                     routingKey: "leave",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void check()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "igrac" + s.id.ToString(), type: "direct");

                var message = "username:" + igr.username + ":novac:" + igr.novac;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "igrac" + s.id.ToString(),
                                     routingKey: "check",
                                     basicProperties: null,
                                     body: body); ;
            }
        }

        public void call()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "igrac" + s.id.ToString(), type: "direct");

                var message = "username:" + igr.username + ":novac:" + igr.novac;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "igrac" + s.id.ToString(),
                                     routingKey: "call",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void fold()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "igrac" + s.id.ToString(), type: "direct");

                var message = "username:" + igr.username + ":novac:" + igr.novac;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "igrac" + s.id.ToString(),
                                     routingKey: "fold",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void rise(string ulog)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "igrac" + s.id.ToString(), type: "direct");

                var message = "username:" + igr.username + ":ulog:" + ulog+":novac:"+igr.novac;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "igrac" + s.id.ToString(),
                                     routingKey: "rise",
                                     basicProperties: null,
                                     body: body);
            }
        }

        Model.IModel IController.getModel()
        {
            throw new NotImplementedException();
        }

        int IController.poeni(int x)
        {
            throw new NotImplementedException();
        }
    }
}
