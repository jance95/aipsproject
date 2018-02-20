using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    abstract class AdditionalLogger:Logger
    {
        private Logger logger;
        public AdditionalLogger(Logger next)
        {
            logger = next;
        }
        public override void Log(string s, String a, RabbitMQ.Client.IModel channel)
        {
            addLog(s);
            logger.Log(s,a,channel);
        }
        protected abstract void addLog(string s);
    }
}
