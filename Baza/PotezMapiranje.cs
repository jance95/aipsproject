using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
namespace StoServer.EntitetiMapiranje
{
    class PotezMapiranje:ClassMap<Entiteti.Potez>
    {
        public PotezMapiranje()
        {
            Table("POTEZI");
            Id(x => x.id, "IDPOTEZA").GeneratedBy.TriggerIdentity();
            Map(x => x.potez, "POTEZ");
            Map(x => x.timestamp, "VREME");
            References(x => x.sto).Column("STO").Not.LazyLoad();
        }
    }
}
