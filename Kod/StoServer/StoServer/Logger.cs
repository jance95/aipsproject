using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
namespace StoServer
{
    public abstract class Logger
    {
        public abstract void Log(String s,String a, RabbitMQ.Client.IModel channel);
    }
}
