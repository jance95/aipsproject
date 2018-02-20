using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    class DocumentLogger : AdditionalLogger
    {
        public DocumentLogger(Logger next):base(next)
        {
            
        }
        protected override void addLog(string s)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@".\..\..\..\Potezi\istorija.txt", true))
            {
                file.WriteLine(s);
            }
        }
    }
}
