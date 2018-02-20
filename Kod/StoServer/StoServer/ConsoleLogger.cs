using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class ConsoleLogger : AdditionalLogger
    {
        public ConsoleLogger(Logger next):base(next)
        {

        }

        protected override void addLog(string s)
        {
            Console.WriteLine(s);
        }
    }
}
