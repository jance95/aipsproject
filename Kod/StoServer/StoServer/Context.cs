using StoServer.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
   public class Context
    {
        public bool partijaUToku;
        public bool start;
        public int period;
        public List<String> igraci;
        public List<String> igraciNaCekanju;
        public List<String> vlasniciKarata;
        public List<int> karteNaStolu;
        public List<int> karteIgraca;
        public int trenutni;
        public static StoServer.IModel klasa;
        public Sto s;
        public static string a;
        public Context(string x)
        {
            a = x;
            igraci = new List<string>();
            igraciNaCekanju = new List<string>();
            vlasniciKarata = new List<string>();
            karteNaStolu = new List<int>();
            karteIgraca = new List<int>();
            klasa = new KlasicneKarte();
            period = 0;
            start = false;
            partijaUToku = false;
            trenutni = 0;
            s = klasa.vratiSto(Int32.Parse(x));
        }
    }
}
