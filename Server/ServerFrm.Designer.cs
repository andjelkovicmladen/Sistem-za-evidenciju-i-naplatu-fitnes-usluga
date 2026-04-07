using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class ServerFrm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lstLog = new ListBox();
            btnStart = new Button();
            btnStop = new Button();
            SuspendLayout();

            lstLog.Dock = DockStyle.None;
            lstLog.Location = new System.Drawing.Point(0, 40);
            lstLog.Size = new System.Drawing.Size(594, 360);
            lstLog.Name = "lstLog";
            lstLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnStart.Location = new System.Drawing.Point(10, 5);
            btnStart.Size = new System.Drawing.Size(100, 30);
            btnStart.Name = "btnStart";
            btnStart.Text = "Start";
            btnStart.Click += new System.EventHandler(btnStart_Click);

            btnStop.Location = new System.Drawing.Point(120, 5);
            btnStop.Size = new System.Drawing.Size(100, 30);
            btnStop.Name = "btnStop";
            btnStop.Text = "Stop";
            btnStop.Enabled = false;
            btnStop.Click += new System.EventHandler(btnStop_Click);

            ClientSize = new System.Drawing.Size(600, 440);
            Controls.Add(lstLog);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Name = "ServerFrm";
            Text = "Fitness Server";
            ResumeLayout(false);
        }

        private ListBox lstLog;
        private Button btnStart;
        private Button btnStop;
    }
}
