using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using OOProjektovanje_lab5.Controller;
using OOProjektovanje_lab5.View;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace OOProjektovanje_lab5
{
    public partial class Form1 : Form, IView
    {
        struct tag { public int karta; public bool select; };
        enum tipPokera { texasHoldem, KlasicanPoker }
        tipPokera tip;
        private IController controller;
        bool exit = false;
        int ulog = 0;
        public Form1()
        {
            InitializeComponent();
            disableButtons();
            CheckForIllegalCrossThreadCalls = false;
            Thread thread = null;
            Task.Run(() =>
            {
                thread = Thread.CurrentThread;
                int prom=3;
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    string queueName=channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange:controller.getSto().id.ToString(),
                                      routingKey:"",
                                      arguments:null);
                    var consumer= new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                      {
                          var body = ea.Body;
                          string message = Encoding.UTF8.GetString(body);
                          string log = message.Replace(':', ' ').Replace('*', ' ').Replace('@', ' ').Replace('.', ' ');
                          string[] logs = log.Split('%');
                          foreach(string l in logs)
                            lbLogs.Items.Add(l);
                          lbLogs.Refresh();
                          if(message[0]=='*')
                          {
                              if(prom==3)
                              {
                                  pictureBox3.Image = pictureBox3.Tag as Image;
                                 
                              }
                              if(prom==2)
                              {
                                  pictureBox1.Image = pictureBox1.Tag as Image;
                                  
                              }
                              if (prom == 1)
                              {
                                  pictureBox2.Image = pictureBox2.Tag as Image;
                                 
                              }
                              prom--;
                          }
                          String[] p = message.Split(':');
                          if (p.Length == 3)
                          {
                              if (p[1].Equals(controller.getIgrac().username))
                              {
                                  btnCheck.Enabled = true;
                                  btnCall.Enabled = true;
                                  btnFold.Enabled = true;
                                  btnRise.Enabled = true;
                              }
                              String[] i = p[2].Split(',');
                              String[] k = i[0].Split('.');
                              List<int> karte = new List<int>();
                              foreach(String si in i)
                              if (si.Contains(controller.getIgrac().username))
                              {
                                  String[] j = si.Split('.');

                                  karte.Add(Int32.Parse(j[1]));
                                  karte.Add(Int32.Parse(j[2]));
                                  karte.Add(Int32.Parse(k[1]));
                                  karte.Add(Int32.Parse(k[2]));
                                  karte.Add(Int32.Parse(k[3]));

                                  deliKarte(karte);
                              }
                              //else
                              //{
                              //    String[] j = i[2].Split('.');
                              //    karte.Add(Int32.Parse(j[1]));
                              //    karte.Add(Int32.Parse(j[2]));
                              //    karte.Add(Int32.Parse(k[1]));
                              //    karte.Add(Int32.Parse(k[2]));
                              //    karte.Add(Int32.Parse(k[3]));
                              //    deliKarte(karte);
                              //}
                          }
                          else if (p.Length == 2)
                          {
                              if (p[1].Equals(controller.getIgrac().username))
                              {
                                  btnCheck.Enabled = true;
                                  btnCall.Enabled = true;
                                  btnFold.Enabled = true;
                                  btnRise.Enabled = true;
                              }
                          }
                          else if (p.Length == 5)
                          {
                              if (p[4].Equals(controller.getIgrac().username))
                              {
                                  btnCheck.Enabled = true;
                                  btnCall.Enabled = true;
                                  btnFold.Enabled = true;
                                  btnRise.Enabled = true;
                              }
                          }
                          else if (p.Length == 6)
                          {
                              if (p[3].Equals(controller.getIgrac().username))
                              {
                                  btnCheck.Enabled = false;
                                  btnCall.Enabled = true;
                                  btnFold.Enabled = true;
                                  btnRise.Enabled = true;
                              }
                              lblUlog.Text = p[5];
                          }
                          else if (p.Length == 9)
                          {
                              if (p[1].Equals(controller.getIgrac().username))
                                  lblPoeni.Text =(2*(Int32.Parse(lblPoeni.Text)+ ulog)).ToString();
                              lblUlog.Text = controller.getSto().minUlog.ToString();
                              ulog = 0;
                              String[] i = p[3].Split(',');
                              String[] k = i[0].Split('.');
                              List<int> karte = new List<int>();
                              foreach (String si in i)
                                  if (si.Contains(controller.getIgrac().username))
                                  {
                                      String[] j = si.Split('.');

                                      karte.Add(Int32.Parse(j[1]));
                                      karte.Add(Int32.Parse(j[2]));
                                      karte.Add(Int32.Parse(k[1]));
                                      karte.Add(Int32.Parse(k[2]));
                                      karte.Add(Int32.Parse(k[3]));

                                      deliKarte(karte);
                                  }
                              prom = 3;
                              disableButtons();
                              if (p[8].Equals(controller.getIgrac().username))
                                  EnableButtons();
                          }
                          else if (p[0][0] != '@')
                              disableButtons();

                          
                      };
                    channel.BasicConsume(queue:queueName,
                                         autoAck:true,
                                         consumer:consumer);
                    while (!exit) ;
                    thread.Abort();
                }
            });
        }

        private void selektuj(object sender, EventArgs e)
        {
            if ((bool)btnZameni.Tag)
                return;
            PictureBox pic = sender as PictureBox;
            if (pic == null)
                return;
            Point pom = pic.Location;
            tag x = (tag)pic.Tag;
            if (x.select)
            {

                pom.Y += 10;
                pic.Location = pom;
                x.select = false;
                pic.Tag = x;
                int pom1 = (int)pictureBox6.Tag - 1;
                pictureBox6.Tag = pom1;
            }
            else
            {
                if ((int)pictureBox6.Tag >= 3)
                    return;
                pom.Y -= 10;
                pic.Location = pom;
                x.select = true;
                pic.Tag = x;
                int pom1 = (int)pictureBox6.Tag + 1;
                pictureBox6.Tag = pom1;
            }
            if ((int)pictureBox6.Tag > 0)
                btnZameni.Enabled = true;
            else
                btnZameni.Enabled = false;

        }

        private void kreirajFormu()
        {
            switch (tip)
            {
                case tipPokera.KlasicanPoker:
                    {
                        this.Size = new Size(731, 368);
                        btnRezim.Location = new Point(12, 22);
                        btnRezim.Size = new Size(81, 40);
                        btnRezim.Text = "Igraj holdem poker";
                        pictureBox1.Location = new Point(33, 190);
                        pictureBox1.Tag = true;
                        pictureBox1.Visible = false;
                        pictureBox2.Location = new Point(180, 190);
                        pictureBox2.Visible = false;
                        pictureBox2.Tag = true;
                        pictureBox3.Location = new Point(313, 190);
                        pictureBox3.Tag = true;
                        pictureBox3.Visible = false;
                        pictureBox4.Location = new Point(451, 190);
                        pictureBox4.Tag = true;
                        pictureBox4.Visible = false;
                        pictureBox5.Location = new Point(587, 190);
                        pictureBox5.Tag = true;
                        pictureBox5.Visible = false;
                        pictureBox6.Location = new Point(298, 12);
                        pictureBox6.Tag = 0;
                        //pictureBox6.Image = Properties.Resources.images__1_;
                        pictureBox6.Image=Properties.Resources.pozadina;
                        pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
                        btnZameni.Enabled = false;
                        btnZameni.Tag = false;
                        lblUlog.Location = new Point(652, 9);
                        lblUlog.Text = controller.getSto().minUlog.ToString();
                        lblPoeni.Location = new Point(652, 163);
                        //lblPoeni.Text = controller.Poeni.ToString();
                        //pictureBox1.Click += selektuj;
                        //pictureBox2.Click += selektuj;
                        //pictureBox3.Click += selektuj;
                        //pictureBox4.Click += selektuj;
                        //pictureBox5.Click += selektuj;
                        dogadjaji();
                        btnZameni.Visible = true;
                        break;
                    }
                case tipPokera.texasHoldem:
                    {
                        this.Size = new Size(537, 469);
                        btnRezim.Location = new Point(12, 22);
                        btnRezim.Size = new Size(81, 40);
                        btnRezim.Text = "Igraj klasican poker";
                        btnRezim.Hide();
                        pictureBox1.Location = new Point(206, 145);
                        pictureBox1.Visible = false;
                        pictureBox2.Location = new Point(368, 145);
                        pictureBox2.Visible = false;
                        pictureBox3.Location = new Point(50, 145);
                        pictureBox3.Visible = false;
                        pictureBox4.Location = new Point(117,300);
                        pictureBox4.Visible = false;
                        pictureBox5.Location = new Point(298, 300);
                        pictureBox5.Visible = false;
                        pictureBox6.Location = new Point(206, 12);
                        pictureBox6.Image = Properties.Resources.images__1_;
                        pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
                        lblUlog.Location = new Point(447, 49);
                        //lblUlog.Text = controller.getSto().minUlog.ToString();
                        lblPoeni.Location = new Point(447, 300);
                        //lblPoeni.Text=controller.Poeni.ToString();
                        //pictureBox1.Click -= selektuj;
                        //pictureBox2.Click -= selektuj;
                        //pictureBox3.Click -= selektuj;
                        //pictureBox4.Click -= selektuj;
                        //pictureBox5.Click -= selektuj;
                        //pictureBox1.Click += okreni;
                        //pictureBox2.Click += okreni;
                        //pictureBox3.Click += okreni;
                        dogadjaji();
                        btnZameni.Visible = false;
                        break;
                    }
            }

        } //dopuniti

        private void dogadjaji()
        {
            switch(tip)
            {
                case tipPokera.KlasicanPoker:
                    {
                        pictureBox1.Click += selektuj;
                        pictureBox2.Click += selektuj;
                        pictureBox3.Click += selektuj;
                        pictureBox4.Click += selektuj;
                        pictureBox5.Click += selektuj;
                        break;
                    }
                case tipPokera.texasHoldem:
                    {
                        pictureBox1.Click += okreni;
                        pictureBox2.Click += okreni;
                        pictureBox3.Click += okreni;
                        break;
                    }
            }
        }

        private void okreni(object sender, EventArgs e)
        {
            PictureBox px = sender as PictureBox;
            px.Image = px.Tag as Image;
            int x = (int)pictureBox6.Tag;
            x--;
            pictureBox6.Tag = x;
        }

        public void AddListener(IController controller)
        {
            this.controller = controller;
            lblUlog.Text = this.controller.getSto().minUlog.ToString();
        }

        private void deliKarte(List<int> karte)
        {
            //controller.ocisti();
            btnZameni.Tag = false;
            //lblPoeni.Text = (int.Parse(lblPoeni.Text) - int.Parse(lblUlog.Text)).ToString();
            //lblUlog.Text = "20";
            switch (tip)
            {
                case tipPokera.KlasicanPoker:
                    {
                        //List<int> karte = new List<int>();
                       // karte = controller.vratiKarte(5);
                        int j = 0;
                        for (int i = 0; i < this.Controls.Count; i++)
                        {
                            
                            PictureBox pic = this.Controls[i] as PictureBox;
                            if (pic != null)
                            {
                                if (pic.Name == "pictureBox6")
                                    continue;
                                Bitmap bm = Properties.Resources.ResourceManager.GetObject("_" + karte[j]) as Bitmap;
                                
                                pic.Image = bm;
                                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                                pic.Visible = true;
                                tag x;
                                x.karta = karte[j];
                                x.select = false;
                                pic.Tag = x;
                                j++;
                            }
                        }
                        break;
                    }
                case tipPokera.texasHoldem:
                    {

                        //List<int> karte = new List<int>();
                        //karte = controller.vratiKarte(5);
                        int j = 0;
                        for (int i = 0; i < this.Controls.Count; i++)
                        {
                            
                            PictureBox pic = this.Controls[i] as PictureBox;
                            if (pic != null)
                            {
                                if (pic.Name == "pictureBox6")
                                {
                                    pic.Tag = 3;
                                    continue;
                                }
                                Bitmap bm = Properties.Resources.ResourceManager.GetObject("_" + karte[j]) as Bitmap;
                                if (j < 2)
                                    pic.Image = bm;
                                else
                                {
                                    pic.Image = Properties.Resources.images__1_;
                                    pic.Tag = (object)bm;
                                }
                                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                                pic.Visible = true;
                                j++;
                            }

                        }
                        break;

                    }
                 
            }
        }

        private void btnRezim_Click(object sender, EventArgs e)
        {
            if (tip == tipPokera.KlasicanPoker)
            {
                controller = new Controller.TexasHoldem(this, controller.getModel());
                tip = tipPokera.texasHoldem;
            }
            else
            {
                controller = new Controller.KlasicanPoker(controller.getModel(),this);
                tip = tipPokera.KlasicanPoker;

            }
            skinidogadjaje();
            kreirajFormu();
        }
        private void skinidogadjaje()
        {
            switch (tip)
            {
                case tipPokera.KlasicanPoker:
                    {
                        pictureBox1.Click -= okreni;
                        pictureBox2.Click -= okreni;
                        pictureBox3.Click -= okreni;
                        break;
                    }
                case tipPokera.texasHoldem:
                    {
                        pictureBox1.Click -= selektuj;
                        pictureBox2.Click -= selektuj;
                        pictureBox3.Click -= selektuj;
                        pictureBox4.Click -= selektuj;
                        pictureBox5.Click -= selektuj;
                        break;
                    }
            }
        }
        private void btnZameni_Click(object sender, EventArgs e)
        {
            List<int> izbacene = new List<int>();
            List<int> karte = controller.vratiKarte((int)pictureBox6.Tag);
            int y = karte.Count - 1;
            for (int i = this.Controls.Count - 1; i > this.Controls.Count - 6; i--)
            {
                tag x = (tag)this.Controls[i].Tag;
                if (x.select == true)
                {
                    izbacene.Add(x.karta);
                    x.karta = karte[y];
                    y--;
                    PictureBox px = this.Controls[i] as PictureBox;
                    Bitmap bm = Properties.Resources.ResourceManager.GetObject("_" + x.karta.ToString()) as Bitmap;
                    px.Image = bm;
                    px.Tag = x;
                }
            }
            controller.izbaci(izbacene);
            btnZameni.Enabled = false;
            btnZameni.Tag = true;
            //MessageBox.Show(controller.vratiPoene(1).ToString());
            //MessageBox.Show(controller.vratiSve().Count.ToString());
        }

        private void btnZavrsi_Click(object sender, EventArgs e)
        {
            switch (tip)
            {
                case tipPokera.KlasicanPoker:
                    {
                        //MessageBox.Show(controller.vratiSve().Count.ToString());
                        lblPoeni.Text = (int.Parse(lblPoeni.Text) + controller.poeni(int.Parse(lblUlog.Text))).ToString();
                        if (int.Parse(lblPoeni.Text) <= 0)
                        {
                            MessageBox.Show("Izgubili ste sve poene!");
                            this.Close();
                        }
                        vratiNamesto();
                        break;
                    }
                case tipPokera.texasHoldem:
                    {
                        if((int)pictureBox6.Tag==3)
                            lblPoeni.Text=(int.Parse(lblPoeni.Text)+5*controller.vratiPoene(int.Parse(lblUlog.Text))).ToString();
                        if((int)pictureBox6.Tag == 2)
                            lblPoeni.Text = (int.Parse(lblPoeni.Text) + 2 * controller.vratiPoene(int.Parse(lblUlog.Text))).ToString();
                        if ((int)pictureBox6.Tag == 1)
                            lblPoeni.Text = (int.Parse(lblPoeni.Text) -int.Parse(lblUlog.Text)+  controller.vratiPoene(int.Parse(lblUlog.Text))).ToString();
                        if ((int)pictureBox6.Tag == 0)
                            lblPoeni.Text = (int.Parse(lblPoeni.Text) - 2*int.Parse(lblUlog.Text) +  controller.vratiPoene(int.Parse(lblUlog.Text))).ToString();
                        otvoriSve();
                        if (int.Parse(lblPoeni.Text) <= 0)
                        {
                            MessageBox.Show("Izgubili ste sve poene!");
                            this.Close();
                        }
                        break;
                    }
             }


        }

        private void otvoriSve()
        {
            for (int i = this.Controls.Count - 1; i > this.Controls.Count - 6; i--)
            {
                PictureBox px = this.Controls[i] as PictureBox;
                Image x = px.Tag as Image;
                if (x!=null)
                {
                    px.Image = x;
                }
            }
        }

        private void vratiNamesto()
        {
            for (int i = this.Controls.Count - 1; i > this.Controls.Count - 6; i--)
            {
                PictureBox px = this.Controls[i] as PictureBox;
                Point pom = px.Location;
                tag x = (tag)px.Tag;
                if (x.select == true)
                {
                    pom.Y += 10;
                    px.Tag = false;
                    px.Location = pom;
                }
                pictureBox6.Tag = 0;
            }
        }

        private void btnTipKarti_Click(object sender, EventArgs e)
        {
            if (btnTipKarti.Text == "Francuske karte")
            {
                this.controller.zameniModel('f');
                btnTipKarti.Text = "Klasicne karte";
            }
            else
            {
                this.controller.zameniModel('k');
                btnTipKarti.Text = "Francuske karte";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(lblPoeni.Text) < int.Parse(comboBox1.Items[comboBox1.SelectedIndex].ToString()))
                MessageBox.Show("Nema!");
            else
                lblUlog.Text = comboBox1.Items[comboBox1.SelectedIndex].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.controller.zameniModel('n');
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controller.takeIn();
            tip = tipPokera.texasHoldem;
            kreirajFormu();
            lblPoeni.Text = controller.getIgrac().novac.ToString();
            btnTipKarti.Text = "Francuske karte";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            exit = true;
            controller.takeout();
            controller.snimiIgraca(controller.getIgrac());
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            controller.check();
            disableButtons();
            controller.getIgrac().novac = Int32.Parse(lblPoeni.Text);
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            if ((Int32.Parse(lblUlog.Text) - ulog) > Int32.Parse(lblPoeni.Text))
            {
                ulog += Int32.Parse(lblPoeni.Text);
                lblPoeni.Text = "0";
            }
            else
            {
                lblPoeni.Text = (Int32.Parse(lblPoeni.Text) - (Int32.Parse(lblUlog.Text) - ulog)).ToString();
                ulog += Int32.Parse(lblUlog.Text);
                
            }
            controller.call();
            disableButtons();
            controller.getIgrac().novac = Int32.Parse(lblPoeni.Text);
        }

        private void btnFold_Click(object sender, EventArgs e)
        {
            controller.fold();
            disableButtons();
            controller.getIgrac().novac = Int32.Parse(lblPoeni.Text);
        }

        private void btnRise_Click(object sender, EventArgs e)
        {
            int pom = Int32.Parse(tbRise.Text) - ulog;
            ulog = Int32.Parse(tbRise.Text);
            controller.rise(ulog.ToString());
            disableButtons();
            lblPoeni.Text = (Int32.Parse(lblPoeni.Text) - pom).ToString();
            controller.getIgrac().novac = Int32.Parse(lblPoeni.Text);
        }

        private void disableButtons()
        {
            btnCheck.Enabled = false;
            btnCall.Enabled = false;
            btnFold.Enabled = false;
            btnRise.Enabled = false;
        }
        private void EnableButtons()
        {
            btnCheck.Enabled = true;
            btnCall.Enabled = true;
            btnFold.Enabled = true;
            btnRise.Enabled = true;
        }

        private void tbRise_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
                return;
            string pom = tbRise.Text;
            if (!Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                pom += e.KeyChar.ToString();
                if (Int32.Parse(pom) > Int32.Parse(lblPoeni.Text))
                {
                    e.Handled = true;
                    tbRise.Text = lblPoeni.Text;
                }
            }
        }
    }
}
