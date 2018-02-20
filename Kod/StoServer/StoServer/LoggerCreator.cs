using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoServer
{
    public class LoggerCreator : LoggerFactory
    {
        public override Logger FactoryMethod()
        {
            string[] lines = System.IO.File.ReadAllLines(@".\..\..\..\Config\loggers.txt");
            AdditionalLogger logger=null;
            Logger log = new PlayersLogger();
            if (lines != null)
            {
                logger = napraviLoggera(lines[0], log);
                lines = lines.Skip(1).ToArray();
                foreach (String line in lines)
                    logger = napraviLoggera(line, logger);
                return logger;
            }     
            return log;
        }
        private AdditionalLogger napraviLoggera(String logger,Logger next)
        {
            if (logger.Equals("document"))
                return new DocumentLogger(next);
            if (logger.Equals("console"))
                return new ConsoleLogger(next);
            return new DBLogger(next);
        }
    }
}
