using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OOProjektovanje_lab5.Controller;
using OOProjektovanje_lab5.View;
using OOProjektovanje_lab5.Controller;
namespace OOProjektovanje_lab5
{
    public partial class Form1 : Form, IView
    {
        struct tag { public int karta; public bool select; };
        enum tipPokera { texasHoldem, KlasicanPoker }
        tipPokera tip;
        private IController controller;
        public Form1()
        {
            InitializeComponent();
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
                        lblUlog.Text = controller.getSto().minUlog.ToString();
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
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            controller.ocisti();
            btnZameni.Tag = false;
            lblPoeni.Text = (int.Parse(lblPoeni.Text) - int.Parse(lblUlog.Text)).ToString();
            //lblUlog.Text = "20";
            switch (tip)
            {
                case tipPokera.KlasicanPoker:
                    {
                        List<int> karte = new List<int>();
                        karte = controller.vratiKarte(5);
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

                        List<int> karte = new List<int>();
                        karte = controller.vratiKarte(5);
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
            //if (tip == tipPokera.KlasicanPoker)
            //{
            //    controller = new Controller.TexasHoldem(this, controller.getModel());
            //    tip = tipPokera.texasHoldem;
            //}
            //else
            //{
                controller = new Controller.KlasicanPoker(controller.getModel(),this);
                tip = tipPokera.KlasicanPoker;

            //}
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
            tip = tipPokera.KlasicanPoker;
            kreirajFormu();
            lblPoeni.Text = controller.getIgrac().novac.ToString();
            btnTipKarti.Text = "Francuske karte";
        }
    }
}
