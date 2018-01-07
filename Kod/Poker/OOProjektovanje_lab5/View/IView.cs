using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOProjektovanje_lab5.Controller;
namespace OOProjektovanje_lab5.View
{
    public interface IView
    {
        void AddListener(IController controller);
    }
}
