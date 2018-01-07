using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OOProjektovanje_lab5.Entiteti
{
    public class Igrac
    {
        //[JsonProperty("id")]
        public virtual int id { get; set; }
        //[JsonProperty("username")]
        public virtual string username { get; set; }
        //[JsonProperty("Ime")]
        public virtual string Ime { get; set; }
        //[JsonProperty("Prezime")]
        public virtual string Prezime { get; set; }
        //[JsonProperty("password")]
        public virtual string password { get; set; }
        //[JsonProperty("novac")]
        public virtual int novac { get; set; }
        //[JsonProperty("brDobijenihPartija")]
        public virtual int brDobijenihPartija { get; set; }
        //[JsonProperty("sto")]
        public virtual Sto sto { get; set; }
    }
}
