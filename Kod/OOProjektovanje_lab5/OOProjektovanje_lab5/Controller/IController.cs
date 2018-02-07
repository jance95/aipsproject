using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOProjektovanje_lab5.Model;
using OOProjektovanje_lab5.Entiteti;

namespace OOProjektovanje_lab5.Controller
{
    public interface IController
    {
        void zameniModel(char m);
        int Poeni { get; set; }
        int vratiPoene(int x);
        List<int> vratiKarte(int x);

        void izbaci(List<int> lk);

        List<int> vratiSve();

        void ocisti();
        IModel getModel();

        string proveriIgraca(string username,string pass);
        String prijaviSto(int idStola, string idIgraca);
         Igrac getIgrac();
        void setIgrac(Igrac i);
        Sto getSto();
        void setSto(Sto x);
        int poeni(int x);
        void takeIn();
        void dodajSto(string x, int y, int z);
        void takeout();

        void check();
        void call();
        void fold();
        void rise(string ulog);
        void snimiIgraca(Igrac i);
        //IList<Sto> izlistajStolove();
    }
}
