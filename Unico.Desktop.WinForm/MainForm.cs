using System;
using System.Windows.Forms;
using System.Drawing;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    public class MainForm : Form
    {
        public MainForm()
        {
            WindowState = FormWindowState.Maximized;
            var browser = new WebBrowser("http://server.unico.local/ide.html");
            browser.Dock = DockStyle.Fill;
            Controls.Add(browser);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }
    }
}
