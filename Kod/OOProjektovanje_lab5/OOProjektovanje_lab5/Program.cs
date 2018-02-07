using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOProjektovanje_lab5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            logIn view = new logIn();
            Model.IModel kk = new Model.KlasicneKarte();
            Controller.IController ic = new Controller.TexasHoldem(view,kk);
            Application.Run(view);
        }
    }
}
