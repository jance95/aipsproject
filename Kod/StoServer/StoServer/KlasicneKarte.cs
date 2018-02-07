
using StoServer.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
namespace StoServer
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
            set
            {
                karte = value;
            }
        }

        public void dodajSto(Sto x)
        {
            ISession s = DataLayer.GetSession();
            s.Save(x);
            s.Flush();
            s.Close();
        }

        public void updateIgrac(Igrac i)
        {
            ISession s = DataLayer.GetSession();
            s.SaveOrUpdate(i);
            s.Flush();
            s.Close();
        }

        public void UpdateSto(Sto x)
        {
            ISession s = DataLayer.GetSession();
            s.SaveOrUpdate(x);
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

        public Sto vratiSto(int id)
        {

            ISession s = DataLayer.GetSession();
            IQuery q = s.CreateQuery("from Sto s  where s.id=:id");
            q.SetInt32("id", id);
            Sto st = q.UniqueResult<Sto>();
            s.Close();
            return st;
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

        public int vratiPoene(int x=1)
        {
            bool kenta = daljeKenta();
            bool boja = daljeBoja();
            if (kenta && boja)
                return x * 150;
            bool dog = daljeDog(9, 14);
            if (dog && boja)
                return x * 60;
            if (boja)
                return x * 40;
            if (kenta)
                return x * 26;
            if (daljePoker())
                return x * 90;
            if (daljeDog(2, 7))
                return x * 18;
            bool triling = daljeTriling();
            bool dvaPara = daljeDvapara();
            //if (triling && dvaPara)
            //    return x * 24;
            //if (daljeBlejz())
            //    return x * 9;
            if (triling)
                return x * 8;
            if (dvaPara)
                return x * 5;
            if (daljePar())
                return x * 1;


            return 0;
        }

        private bool daljeDog(int x, int y)
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
            {
                pom[i] = pom[i] / 10;
                if (pom[i] == 1)
                    pom[i] = 11;
            }
            pom.Sort();
            if (daljePar())
                return false;
            else
            {
                if (pom[0] >= x && pom[pom.Count - 1] <= y)
                    return true;
                return false;
            }
        }
        private bool daljePar()
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            for (int i = 0; i < pom.Count - 1; i++)
                if (pom[i] == pom[i + 1])
                    return true;
            return false;
        }


        private bool daljeDvapara()
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            if ((pom[0] == pom[1] && pom[2] == pom[3]) || (pom[0] == pom[1] && pom[3] == pom[4]) || (pom[1] == pom[2] && pom[3] == pom[4]))
                return true;
            else
                return false;

        }

        private bool daljeTriling()
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            if ((pom[0] == pom[1] && pom[1] == pom[2]) || (pom[1] == pom[2] && pom[2] == pom[3]) || (pom[2] == pom[3] && pom[3] == pom[4]))
                return true;
            else
                return false;
        }

        private bool daljePoker()
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            if ((pom[0] == pom[1] && pom[1] == pom[2] && pom[2] == pom[3]) || (pom[1] == pom[2] && pom[2] == pom[3] && pom[3] == pom[4]))
                return true;
            else
                return false;
        }

        private bool daljeBoja()
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] % 10;
            for (int i = 0; i < pom.Count - 1; i++)
                if (pom[i] != pom[i + 1])
                    return false;
            return true;
        }

        private bool daljeKenta()
        {
            List<int> pom = Karte.ToList();
            for (int i = 0; i < pom.Count; i++)
                pom[i] = pom[i] / 10;
            pom.Sort();
            for (int i = 0; i < pom.Count - 1; i++)
                if (pom[i] == 10)
                {
                    if (pom[i] != (pom[i + 1]) - 2)
                        return false;
                }
                else
                if (pom[i] != (pom[i + 1] - 1))
                    return false;
            return true;
        }

    }
}
