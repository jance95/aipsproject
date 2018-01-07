using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receive.Mapiranja
{
    class IgracMapiranje : ClassMap<Entiteti.Igrac>
    {
        public IgracMapiranje()
        {
            Table("IGRAC");
            Id(x => x.id, "IDIGRAC").GeneratedBy.TriggerIdentity();
            Map(x => x.Ime, "IME");
            Map(x => x.Prezime, "PREZIME");
            Map(x => x.username, "USERNAME");
            Map(x => x.password, "PASS");
            Map(x => x.novac, "NOVAC");
            Map(x => x.brDobijenihPartija, "BRDOBIJENIHPARTIJA");
            References(x => x.sto).Column("STO").Not.LazyLoad();
        }
    }
}
