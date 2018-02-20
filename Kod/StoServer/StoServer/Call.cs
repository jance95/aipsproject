using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class Call : MessageHandler
    {
        public override string generisiPoruku(string y, Context c)
        {
            return y+ "Igrac :" + poruka[1] + ":call:sledeci:" + c.igraci[c.trenutni];
        }

        public override bool regulisiIgrace(Context c)
        {
            return false;
        }

        public override bool regulisiRedosledIgraca(Context c)
        {
            return false;
        }

        public override bool regulisiTablu(Context c)
        {
            return false;
        }

        public override void staviNaCekanje(Context c, int igrac)
        {
        }
    }
}
