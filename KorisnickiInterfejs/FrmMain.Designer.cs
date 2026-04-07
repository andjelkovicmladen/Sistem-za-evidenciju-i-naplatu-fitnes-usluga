using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KorisnickiInterfejs
{
    partial class FrmMain
    {
        private void InitializeComponent()
        {
            btnClanovi = new Button();
            btnRacuni = new Button();
            btnOdjava = new Button();
            SuspendLayout();

            btnClanovi.Location = new System.Drawing.Point(30, 30);
            btnClanovi.Size = new System.Drawing.Size(150, 50);
            btnClanovi.Name = "btnClanovi";
            btnClanovi.Text = "Upravljanje clanovima";
            btnClanovi.Click += new System.EventHandler(btnClanovi_Click);

            btnRacuni.Location = new System.Drawing.Point(30, 100);
            btnRacuni.Size = new System.Drawing.Size(150, 50);
            btnRacuni.Name = "btnRacuni";
            btnRacuni.Text = "Upravljanje racunima";
            btnRacuni.Click += new System.EventHandler(btnRacuni_Click);

            btnOdjava.Location = new System.Drawing.Point(30, 170);
            btnOdjava.Size = new System.Drawing.Size(150, 50);
            btnOdjava.Name = "btnOdjava";
            btnOdjava.Text = "Odjava";
            btnOdjava.Click += new System.EventHandler(btnOdjava_Click);

            ClientSize = new System.Drawing.Size(300, 280);
            Controls.Add(btnClanovi);
            Controls.Add(btnRacuni);
            Controls.Add(btnOdjava);
            Name = "FrmMain";
            Text = "Fitness System";
            ResumeLayout(false);
        }

        private Button btnClanovi;
        private Button btnRacuni;
        private Button btnOdjava;
    }
}
