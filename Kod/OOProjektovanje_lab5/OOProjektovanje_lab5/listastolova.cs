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
using OOProjektovanje_lab5.Entiteti;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace OOProjektovanje_lab5
{
    public partial class listastolova : Form,IView
    {
        private IController controller;
        private IList<Sto> sto;
        private Igrac igrac;

        public listastolova(Prikaz p)
        {
            InitializeComponent();
            sto = new List<Sto>();
            sto = p.stolovi;
            igrac = p.i;
            
        }

        public void AddListener(IController controller)
        {
            this.controller = controller;
            controller.setIgrac(igrac);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listastolova_Load(object sender, EventArgs e)
        {
           // IList<Sto> s = new List<Sto>();
           // s = controller.izlistajStolove();
            foreach (Sto x in sto)
            {
                ListViewItem item = new ListViewItem(new string[] { x.id.ToString(), x.Naziv.ToString(),x.minUlog.ToString(),x.maxBrIgraca.ToString() });
                item.Tag=x;
                listView1.Items.Add(item);
            }
            listView1.Refresh();

        }

 

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
            int id = Int32.Parse(listView1.Items[listView1.FocusedItem.Index].SubItems[0].Text);
            int ul = Int32.Parse(listView1.Items[listView1.FocusedItem.Index].SubItems[2].Text);
            if(controller.prijaviSto(id, igrac.username).Equals("Uspesno"))
            {
                controller.setSto((Sto)listView1.Items[listView1.FocusedItem.Index].Tag);
                //controller.takeIn();
            }
            else
            {
                MessageBox.Show("Neuspesno prijavljivanje na sto. Pokusajte ponovo.");
                return;
            }
           
            Form1 f = new Form1();
            f.AddListener(controller);
            //controller.takeIn();
            if (f.ShowDialog() == DialogResult.OK)
            {
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string ime = textBox1.Text;
            //int ulog = Int32.Parse(textBox2.Text);
            //int igraci = Int32.Parse(textBox3.Text);
            //controller.dodajSto(ime, ulog, igraci);
            //IList<Sto> s = new List<Sto>();
            //s = controller.izlistajStolove();
            //listView1.Items.Clear();
            //foreach (Sto x in s)
            //{
            //    ListViewItem item = new ListViewItem(new string[] { x.id.ToString(), x.Naziv.ToString(), x.minUlog.ToString(), x.maxBrIgraca.ToString() });
            //    item.Tag = x;
            //    listView1.Items.Add(item);
            //}
            //listView1.Refresh();

        }
    }
}
