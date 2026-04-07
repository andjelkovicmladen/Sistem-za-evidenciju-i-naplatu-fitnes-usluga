using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Server
{
    public partial class ServerFrm : Form
    {
        private Server server = new Server();

        public ServerFrm()
        {
            InitializeComponent();
        }

        public void DodajLog(string poruka)
        {
            if (lstLog.InvokeRequired)
                lstLog.Invoke(new Action(() => lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {poruka}")));
            else
                lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {poruka}");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start(this);
            DodajLog("Server pokrenut!");
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            DodajLog("Server zaustavljen!");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}
