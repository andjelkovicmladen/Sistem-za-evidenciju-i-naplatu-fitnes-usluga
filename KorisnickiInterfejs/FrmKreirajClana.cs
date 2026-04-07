using Domen;
using KorisnickiInterfejs.UIKontroler;

namespace KorisnickiInterfejs
{
    public partial class FrmKreirajClana : Form
    {
        private List<TipClanarine> listaTipova;
        private Clan postojeciClan = null;
        private bool jeIzmena = false;

        // Konstruktor za dodavanje novog clana
        public FrmKreirajClana(List<TipClanarine> tipovi)
        {
            InitializeComponent();
            listaTipova = tipovi;
            PopuniTipove();
            this.Text = "Dodaj novog člana";
        }

        // Konstruktor za izmenu postojeceg clana
        public FrmKreirajClana(List<TipClanarine> tipovi, Clan clan)
        {
            InitializeComponent();
            listaTipova = tipovi;
            PopuniTipove();
            postojeciClan = clan;
            jeIzmena = true;
            this.Text = "Izmeni člana";
            PopuniPolja();
        }

        private void PopuniTipove()
        {
            cmbTipClanarine.Items.Clear();
            foreach (var tip in listaTipova)
                cmbTipClanarine.Items.Add(tip);
            cmbTipClanarine.DisplayMember = "Naziv";
            if (cmbTipClanarine.Items.Count > 0)
                cmbTipClanarine.SelectedIndex = 0;
        }

        private void PopuniPolja()
        {
            txtIme.Text = postojeciClan.Ime;
            txtPrezime.Text = postojeciClan.Prezime;
            txtEmail.Text = postojeciClan.Email;
            txtTelefon.Text = postojeciClan.BrojTelefona;
            txtPassword.Text = postojeciClan.Password;

            var tip = listaTipova.FirstOrDefault(t => t.IdTipClanarine == postojeciClan.IdTipClanarine);
            if (tip != null)
                cmbTipClanarine.SelectedItem = tip;
        }

        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            if (!Validiraj()) return;

            try
            {
                TipClanarine odabraniTip = (TipClanarine)cmbTipClanarine.SelectedItem;

                if (jeIzmena)
                {
                    postojeciClan.Ime = txtIme.Text.Trim();
                    postojeciClan.Prezime = txtPrezime.Text.Trim();
                    postojeciClan.Email = txtEmail.Text.Trim();
                    postojeciClan.BrojTelefona = txtTelefon.Text.Trim();
                    postojeciClan.Password = txtPassword.Text.Trim();
                    postojeciClan.IdTipClanarine = odabraniTip.IdTipClanarine;
                    Kontroler.Instance.PromeniClana(postojeciClan);
                    MessageBox.Show("Član je uspešno izmenjen!", "Uspeh",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Clan noviClan = new Clan
                    {
                        Ime = txtIme.Text.Trim(),
                        Prezime = txtPrezime.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        BrojTelefona = txtTelefon.Text.Trim(),
                        Password = txtPassword.Text.Trim(),
                        IdTipClanarine = odabraniTip.IdTipClanarine
                    };
                    Kontroler.Instance.KreirajClana(noviClan);
                    MessageBox.Show("Član je uspešno dodat!", "Uspeh",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool Validiraj()
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text))
            { MessageBox.Show("Ime je obavezno!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtIme.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtPrezime.Text))
            { MessageBox.Show("Prezime je obavezno!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPrezime.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            { MessageBox.Show("Email je obavezan!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtEmail.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtTelefon.Text))
            { MessageBox.Show("Broj telefona je obavezan!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTelefon.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            { MessageBox.Show("Lozinka je obavezna!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPassword.Focus(); return false; }
            if (cmbTipClanarine.SelectedItem == null)
            { MessageBox.Show("Odaberite tip članarine!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }
    }
}
