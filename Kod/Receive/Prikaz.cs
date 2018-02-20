using Receive.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receive
{
    public class Prikaz
    {
        public virtual Igrac i { get; set; }
        public virtual IList<Sto> stolovi { get; set; }

        public Prikaz()
        {
            stolovi = new List<Sto>();
        }
    }
}
