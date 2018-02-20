using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    public class MessageHandlerCreator : Factory
    {
        public override MessageHandler FactoryMethod(string s)
        {
            if (s == "in")
                return new In();
            if (s == "leave")
                return new Leave();
            if (s == "check")
                return new Check();
            if (s == "call")
                return new Call();
            if (s == "fold")
                return new Fold();
            return new Rise();

        }
    }
}
