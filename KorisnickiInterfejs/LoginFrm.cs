using Domen;
using KorisnickiInterfejs.UIKontroler;
namespace KorisnickiInterfejs
{
    public partial class LoginFrm : Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Kontroler.Instance.Connect();
                Administrator admin = new Administrator
                {
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };
                Administrator prijavljeni = Kontroler.Instance.Login(admin);
                MessageBox.Show($"Dobrodosli, {prijavljeni.ImePrezime}!", "Uspeh");
                FrmMain main = new FrmMain(prijavljeni);
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}