using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer.Entiteti
{
    public class Potez
    {
        public virtual int id { get; set; }
        public virtual string potez { get; set; }
        public virtual DateTime timestamp { get; set; }
        public virtual Sto sto { get; set; }
    }
}
