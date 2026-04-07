using Domen;
using KorisnickiInterfejs.UIKontroler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KorisnickiInterfejs
{
    public partial class FrmMain : Form
    {
        private Administrator prijavljeniAdmin;

        public FrmMain(Administrator admin)
        {
            InitializeComponent();
            prijavljeniAdmin = admin;
            this.Text = $"Fitness System - {admin.ImePrezime}";
        }

        private void btnClanovi_Click(object sender, EventArgs e)
        {
            FrmClanovi frmClanovi = new FrmClanovi(prijavljeniAdmin);
            frmClanovi.ShowDialog();
        }

        private void btnRacuni_Click(object sender, EventArgs e)
        {
            FrmRacuni frmRacuni = new FrmRacuni(prijavljeniAdmin);
            frmRacuni.ShowDialog();
        }

        private void btnOdjava_Click(object sender, EventArgs e)
        {
            try
            {
                Kontroler.Instance.Disconnect();
            }
            catch { }
            LoginFrm login = new LoginFrm();
            login.Show();
            this.Close();
        }
    }
}
