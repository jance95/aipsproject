using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receive.Entiteti
{
    public class Igrac
    {
        public virtual int id { get; set; }
        public virtual string username { get; set; }
        public virtual string Ime { get; set; }
        public virtual string Prezime { get; set; }
        public virtual string password { get; set; }
        public virtual int novac { get; set; }
        public virtual int brDobijenihPartija { get; set; }
        public virtual Sto sto { get; set; }
    }
}
