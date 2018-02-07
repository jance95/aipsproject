using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOProjektovanje_lab5.Entiteti;

namespace OOProjektovanje_lab5.Model
{
    class NoveKarte:IModel
    {
        List<int> karte = new List<int>();

        public List<int> Karte
        {
            get
            {
                return karte;
            }
        }

        public void dodajSto(Sto x)
        {
            throw new NotImplementedException();
        }

        public void upisiPoene(Igrac id, int iznos, int ulog)
        {
            throw new NotImplementedException();
        }

        public Igrac vratiIgraca(string username)
        {
            throw new NotImplementedException();
        }

        public List<int> vratiKarte(int y)
        {
            int k = y;
            Random karta = new Random();
            Random boja = new Random();
            while (k > 0)
            {
                if (generisi(karta, boja))
                    k--;
            }
            return karte.ToList();
        }

        public IList<Sto> vratiStolove()
        {
            throw new NotImplementedException();
        }

        private bool generisi(Random karta, Random boja)
        {
            int pom = 7 + karta.Next(8);
            if (pom == 11)
                pom = 1;
            int pom1 = boja.Next(5);
            pom = pom * 10 + pom1;
            foreach (int x in karte)
                if (x == pom)
                    return false;
            karte.Add(pom);
            return true;
        }
    }
}
