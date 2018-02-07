using StoServer.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    public interface IModel
    {
        List<int> Karte { get; }
        List<int> vratiKarte(int x);
        Igrac vratiIgraca(string username);
        IList<Sto> vratiStolove();
        void upisiPoene(Igrac id, int iznos, int ulog);
        void dodajSto(Sto x);
        Sto vratiSto(int id);
        void UpdateSto(Sto x);
        void updateIgrac(Igrac i);
        int vratiPoene(int x=1);
    }
}
