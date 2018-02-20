using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class Fold : MessageHandler
    {
        int format=0;
        public override string generisiPoruku(string y, Context c)
        {
            return format==0? y+ "Igrac :" + poruka[1] + ":fold:sledeci:" + c.igraci[c.trenutni]
                            : "Igrac" + c.igraci[c.trenutni] + "je dobio ovu ruku"+ "%igracNaPotezu:"+c.igraci[c.trenutni]+":"
                            +generisiKarte(c);
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
            c.igraciNaCekanju.Add(c.igraci[igrac]);
            c.igraci.RemoveAt(igrac);
            int ind = c.vlasniciKarata.IndexOf(poruka[1]);
            c.vlasniciKarata.RemoveAt(ind);
            c.karteIgraca.RemoveAt(2 * ind);
            c.karteIgraca.RemoveAt(2 * ind);
            if(c.trenutni!=0)
                c.trenutni = igrac;
            if(c.igraci.Count==1)
            {
                c.igraci.AddRange(c.igraciNaCekanju);
                c.igraciNaCekanju.Clear();
                c.trenutni = 0;
                format = 1;
            }

        }
    }
}
