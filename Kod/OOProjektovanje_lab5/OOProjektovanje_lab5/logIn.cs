using OOProjektovanje_lab5.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OOProjektovanje_lab5.Controller;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using OOProjektovanje_lab5.Entiteti;

namespace OOProjektovanje_lab5
{
    public partial class logIn : Form,IView
    {
        private IController controller;
        public logIn()
        {
            InitializeComponent();
            tbpass.PasswordChar = '*';
        }

        public void AddListener(IController controller)
        {
            this.controller = controller;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {

            string u = tbusername.Text;
            string p = tbpass.Text;
            string poruka = controller.proveriIgraca(u, p);
            try
            {
                var s1 = JsonConvert.DeserializeObject<Prikaz>(poruka);
                // if (s1.GetType() != typeof(String))
                //MessageBox.Show(((Igrac)s1).id + "\n" + ((Igrac)s1).Ime + "\n" + ((Igrac)s1).Prezime + "\n" + ((Igrac)s1).username + "\n" + ((Igrac)s1).novac);
                listastolova ls = new listastolova(s1);
                this.Hide();
                ls.AddListener(controller);
                if (ls.ShowDialog() == DialogResult.OK)
                { }
                this.Close();
            }
            catch
            {
                MessageBox.Show(poruka);
            }
            
                //if (poruka == null)
                //{
                //    listastolova ls = new listastolova();
                //    this.Hide();
                //    ls.AddListener(controller);
                //    if (ls.ShowDialog() == DialogResult.OK)
                //    {
                //    }
                //    this.Close();
                //}
                //else
                //    MessageBox.Show(poruka);
                


        }
    }
}
