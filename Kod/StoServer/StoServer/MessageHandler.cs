using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    public abstract class MessageHandler
    {
        protected string[] poruka;
        public string ObradiPoruku(string xy,Context c)
        {
            poruka = xy.Split(':');
            string message1 = "";
            if (!(regulisiIgrace(c) && regulisiTablu(c)))
            {
                #region funkcija
                if (!regulisiRedosledIgraca(c))
                {
                    int pom = c.trenutni;
                    c.trenutni++;
                    if (c.trenutni == c.igraci.Count)
                    {
                        c.trenutni = 0;
                        c.period++;
                        if (c.period == 5)
                        {   
                            c.period = 0;
                            Context.klasa.Karte.Clear();
                            List<int> poeni = new List<int>();
                            foreach (string ig in c.vlasniciKarata)
                            {
                                Context.klasa.Karte.AddRange(c.karteNaStolu);
                                Context.klasa.Karte.Add(c.karteIgraca[0]);
                                Context.klasa.Karte.Add(c.karteIgraca[1]);
                                c.karteIgraca.RemoveAt(0);
                                c.karteIgraca.RemoveAt(0);
                                poeni.Add(Context.klasa.vratiPoene());
                                Context.klasa.Karte.Clear();
                            }
                            string igr = c.vlasniciKarata[poeni.IndexOf(poeni.Max())];
                            Console.WriteLine(igr);
                            message1 = "Igrac:" + igr + ":je dobio ovu ruku";
                            if (c.igraciNaCekanju.Count > 0)
                            {
                                c.igraci.AddRange(c.igraciNaCekanju);
                                c.igraciNaCekanju.Clear();
                            }
                            List<int> karte = new List<int>();
                            List<int> karte1 = new List<int>();
                            karte = Context.klasa.vratiKarte(c.igraci.Count * 2);
                            karte1 = Context.klasa.vratiKarte(3);
                            message1 += ":karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
                            c.karteNaStolu.Clear();
                            c.karteNaStolu.AddRange(karte1);
                            c.vlasniciKarata.Clear();
                            c.vlasniciKarata.AddRange(c.igraci);
                            c.karteIgraca.Clear();
                            c.karteIgraca.AddRange(karte);
                            foreach (string x in c.igraci)
                            {
                                message1 += x + "." + karte[0] + "." + karte[1] + ",";
                                karte.RemoveAt(0);
                                karte.RemoveAt(0);
                            }
                            message1 = message1.Remove(message1.Length - 1, 1);
                            c.period++;
                            message1 += ":";
                        }
                        else
                        {
                            message1 = "*";
                        }
                    }
                    staviNaCekanje(c, pom);
                }
                #endregion
            }
            return generisiPoruku(message1, c);
        }
        protected string generisiKarte(Context c)
        {
            List<int> karte = new List<int>();
            List<int> karte1 = new List<int>();
            karte = Context.klasa.vratiKarte(c.igraci.Count * 2);
            karte1 = Context.klasa.vratiKarte(3);
            string message1="";
            message1 += "%karte." + karte1[0] + "." + karte1[1] + "." + karte1[2] + ",";
            c.karteNaStolu.Clear();
            c.karteNaStolu.AddRange(karte1);
            c.vlasniciKarata.Clear();
            c.vlasniciKarata.AddRange(c.igraci);
            c.karteIgraca.Clear();
            c.karteIgraca.AddRange(karte);
            foreach (string x in c.igraci)
            {
                message1 += x + "." + karte[0] + "." + karte[1] + ",";
                karte.RemoveAt(0);
                karte.RemoveAt(0);
            }
            message1 = message1.Remove(message1.Length - 1, 1);
            return message1;
        }
        public abstract bool regulisiIgrace(Context c);
        public abstract bool regulisiTablu(Context c);
        public abstract bool regulisiRedosledIgraca(Context c);
        public abstract void staviNaCekanje(Context c,int igrac);
        public abstract string generisiPoruku(string y, Context c);
    }
}
