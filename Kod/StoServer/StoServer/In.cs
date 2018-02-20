using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class In : MessageHandler
    {
        private string message1;
        public override string generisiPoruku( string y, Context c)
        {
            return message1;
        }

        public override bool regulisiIgrace(Context c)
        {
            c.s.trenutniBrIgraca++;
            return true;
        }

        public override bool regulisiRedosledIgraca(Context c)
        {
            throw new NotImplementedException();
        }

        public override bool regulisiTablu(Context c)
        {
            message1 = "";
            if (c.partijaUToku)
            {
                c.igraciNaCekanju.Add(poruka[1]);
                message1 += "@";
            }

            else
                c.igraci.Add(poruka[1]);
            message1 += "Igrac " + poruka[1] + " se pridruzio tabli.";
            if (!c.start && c.igraci.Count > 1)
            {
                c.period++;
                message1 += "%igracNaPotezu:" + c.igraci[0];
                c.start = true;
                c.partijaUToku = true;
                message1 += ":";
                //List<int> karte = new List<int>();
                //List<int> karte1 = new List<int>();
                //karte = c.klasa.vratiKarte(c.igraci.Count * 2);
                //karte1 = c.klasa.vratiKarte(3);
                //message1 += "%karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
                //c.karteNaStolu.Clear();
                //c.karteNaStolu.AddRange(karte1);
                //c.vlasniciKarata.Clear();
                //c.vlasniciKarata.AddRange(c.igraci);
                //c.karteIgraca.Clear();
                //c.karteIgraca.AddRange(karte);
                //foreach (string x in c.igraci)
                //{
                //    message1 += x + "." + karte[0] + "." + karte[1] + ",";
                //    karte.RemoveAt(0);
                //    karte.RemoveAt(0);
                //}
                //message1 = message1.Remove(message1.Length - 1, 1);
                message1 += generisiKarte(c);
            }
            return true;
        }

        public override void staviNaCekanje(Context c, int igrac)
        {
            throw new NotImplementedException();
        }
    }
}
