using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class DBLogger : AdditionalLogger
    {
        public DBLogger(Logger next):base(next)
        {

        }
        protected override void addLog(string s)
        {
            Context.klasa.UpisiPotez(s, Context.a);
        }
    }
}
