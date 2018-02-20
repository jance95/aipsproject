using StoServer.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class Leave : MessageHandler
    {
        private string message1;
        private Igrac igrac;
        public override string generisiPoruku(string y, Context c)
        {
            return message1;
        }

        public override bool regulisiIgrace(Context c)
        {
            Igrac i = Context.klasa.vratiIgraca(poruka[1]);
            i.sto = null;
            i.novac = Int32.Parse(poruka[3]);
            Context.klasa.updateIgrac(i);
            igrac = i;
            c.s.trenutniBrIgraca--;
            Context.klasa.UpdateSto(c.s);
            message1 = "Igrac: " + i.username + " je napustio igru.:a";
            return true;
        }

        public override bool regulisiRedosledIgraca(Context c)
        {
            throw new NotImplementedException();
        }

        public override bool regulisiTablu(Context c)
        {
            if (c.igraci.Contains(igrac.username))
            {
                if (c.igraci.IndexOf(igrac.username) < c.trenutni)
                    c.trenutni--;
                c.igraci.Remove(igrac.username);
            }
            else
            {
                c.igraciNaCekanju.Remove(igrac.username);
            }
            if (c.igraci.Count < 2)
            {
                message1 = message1.Substring(0,message1.Length - 2);
                message1 = message1.Remove(message1.IndexOf(':'), 1);
                if (c.igraci.Count == 1)
                {
                    message1 += "Igrac " + c.igraci[0] + " je dobio ruku.";
                    c.start = false;
                    c.partijaUToku = false;
                }
                if (c.igraciNaCekanju.Count > 0)
                {
                    for (int ind = 0; ind < c.igraciNaCekanju.Count; ind++)
                        c.igraci.Add(c.igraciNaCekanju[ind]);
                    c.igraciNaCekanju.Clear();
                    if (c.igraci.Count > 1)
                    {
                        c.start = true;
                        c.partijaUToku = true;
                        c.period = 1;
                        message1 += "%igracNaPotezu:" + c.igraci[0];
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
                        message1= message1 + generisiKarte(c);
                    }
                    else
                    {
                        c.start = false;
                        c.partijaUToku = false;
                    }
                }

            }
            else
                message1+=":sledeci:" + c.igraci[c.trenutni];
            return true;
        }

        public override void staviNaCekanje(Context c, int igrac)
        {
            throw new NotImplementedException();
        }
    }
}
