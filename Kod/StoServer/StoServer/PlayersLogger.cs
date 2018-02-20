using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class PlayersLogger : Logger
    {
  

        public override void Log(string s, String a, RabbitMQ.Client.IModel channel)
        {
            var body1 = Encoding.UTF8.GetBytes(s);
            channel.BasicPublish(exchange: a,
                                   routingKey: "",
                                   basicProperties: null,
                                   body: body1);
           
        }
    }
}
