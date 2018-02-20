﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receive.Mapiranja
{
    class StoMapiranje : ClassMap<Entiteti.Sto>
    {
        public StoMapiranje()
        {
            Table("STO");
            Id(x => x.id, "IDSTO").GeneratedBy.TriggerIdentity();
            Map(x => x.Naziv, "NAZIV");
            Map(x => x.minUlog, "MINULOG");
            Map(x => x.maxBrIgraca, "MAXBRIGRACA");
            Map(x => x.trenutniBrIgraca, "TRENUTNIBRIGRACA");
            HasMany(x => x.igraci).KeyColumn("STO").Not.LazyLoad().Inverse().Cascade.All();
        }
    }
}
