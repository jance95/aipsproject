using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOProjektovanje_lab5.Entiteti;

namespace OOProjektovanje_lab5.Model
{
    //boje:0-srce 1-detelina 2-list 3-karo

    public interface IModel
    {
        List<int> Karte { get; }
        List<int> vratiKarte(int x);
        Igrac vratiIgraca(string username);
        IList<Sto> vratiStolove();
        void upisiPoene(Igrac id, int iznos, int ulog);
        void dodajSto(Sto x);
    }
}
