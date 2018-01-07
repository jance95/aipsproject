using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Receive.Entiteti;
using Receive.Model;

namespace Receive.Model
{
    public class KlasicneKarte : IModel
    {
        List<int> karte = new List<int>();
        List<int> zamena = new List<int>();
        public List<int> Karte
        {
            get
            {
                return karte;
            }
        }

        public void dodajSto(Sto x)
        {
            ISession s = DataLayer.GetSession();
            s.Save(x);
            s.Flush();
            s.Close();
        }

        public void upisiPoene(Igrac id, int iznos, int ulog)
        {
            ISession s = DataLayer.GetSession();
            id.novac += iznos - ulog;
            s.Update(id);
            s.Flush();
            s.Close();
        }

        public Igrac vratiIgraca(string username)
        {
            ISession s = DataLayer.GetSession();
            IQuery q = s.CreateQuery("from Igrac ig  where ig.username=:username");
            q.SetString("username", username);
            Igrac i = q.UniqueResult<Igrac>();
            s.Close();
            return i;
        }

        public List<int> vratiKarte(int x)
        {
            zamena.Clear();
            int k = x;
            Random karta = new Random();
            Random boja = new Random();
            while (k > 0)
            {
                if (generisi(karta, boja))
                    k--;
            }
            return zamena.ToList();
        }

        public IList<Sto> vratiStolove()
        {
            ISession s = DataLayer.GetSession();
            IQuery q = s.CreateQuery("from Sto");
            IList<Sto> rez = q.List<Sto>();
            s.Close();
            return rez;
        }

        private bool generisi(Random karta, Random boja)
        {
            int pom = 1 + karta.Next(14);
            if (pom == 11)
                return false;
            int pom1 = boja.Next(4);
            pom = pom * 10 + pom1;
            foreach (int x in karte)
                if (x == pom)
                    return false;
            karte.Add(pom);
            zamena.Add(pom);
            return true;
        }

    }
}

