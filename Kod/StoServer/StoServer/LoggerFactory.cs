using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    public abstract class LoggerFactory
    {
        public abstract Logger FactoryMethod();
    }
}
