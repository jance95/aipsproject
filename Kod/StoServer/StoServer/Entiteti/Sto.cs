using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer.Entiteti
{
    public class Sto
    {
        public virtual int id { get; set; }
        public virtual String Naziv { get; set; }
        public virtual int maxBrIgraca { get; set; }
        public virtual int trenutniBrIgraca { get; set; }
        public virtual int minUlog { get; set; }
        public virtual IList<Igrac> igraci { get; set; }
        public Sto()
        {
            igraci = new List<Igrac>();
        }
    }
}
