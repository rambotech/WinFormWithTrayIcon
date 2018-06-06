using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormWithTrayIcon
{
    public partial class Form1 : Form
    {
        bool Terminating = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Items.Clear();
            this.contextMenuStrip1.Items.Add("&Restore");
            this.contextMenuStrip1.Items.Add("-");
            this.contextMenuStrip1.Items.Add("E&xit");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Terminating)
            {
                // the idle state has occurred, and the tray notification should be gone.
                // ok to shutdown now
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing && MessageBox.Show("Are you sure you want to close this form?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            {
                // only the user, selecting Cancel in a MessageBox, can do this.
                e.Cancel = true;
            }
            if (!e.Cancel)
            {
                // The application will shut down.

                // We cancel the shutdown, because the timer will do the shutdown when it fires.
                // This will return to the app and allow the idle state to occur.
                e.Cancel = true;

                // Dispose of the tray icon this way.
                this.notifyIcon1.Dispose();
                
                // Set the termination flag so that the next entry into this event will
                // not be cancelled.
                Terminating = true;
                
                // Activate the timer to fire
                this.timer1.Interval = 100;
                this.timer1.Enabled = true;
                this.timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // the idle state is past.. at this point, the tray notification is gone from
            // the system tray.  

            // Deactivate the timer.. it is no longer needed.
            timer1.Stop();
            timer1.Enabled = false;

            // close the form, which will start the shutdown of the application.
            this.Close();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "&Restore")
            {
                Show();
                WindowState = FormWindowState.Normal;
                return;
            }

            else if (e.ClickedItem.Text == "E&xit")
            {
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
    }
}
