using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KorisnickiInterfejs
{
    internal class Helper
    {
        public static void ProveraServerGreske(Exception ex)
        {
            if (ex.Message.Contains("transport connection") || ex.Message.Contains("forcibly closed") || ex.Message.Contains("Cannot read") || ex.Message.Contains("aborted"))
            {
                MessageBox.Show("Server je ugasen\n Izbaceni ste iz aplikacije!", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
