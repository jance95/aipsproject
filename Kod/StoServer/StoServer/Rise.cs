using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class Rise : MessageHandler
    {
        public override string generisiPoruku(string y, Context c)
        {
            return y+ "Igrac :" + poruka[1] + ":sledeci:" + c.igraci[c.trenutni] + ":rise:" + poruka[3];
        }

        public override bool regulisiIgrace(Context c)
        {
            return false;
        }

        public override bool regulisiRedosledIgraca(Context c)
        {
            Rotate(c.igraci, c.trenutni);
            c.trenutni = 1;
            return true;
        }

        public override bool regulisiTablu(Context c)
        {
            return false;
        }

        public override void staviNaCekanje(Context c, int igrac)
        {
            throw new NotImplementedException();
        }

        private void Rotate(List<string> lista, int p)
        {
            while (p > 0)
            {
                string pom = lista[0];
                lista.RemoveAt(0);
                lista.Add(pom);
                p--;
            }
        }
    }
}
