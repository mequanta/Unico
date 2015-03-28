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
            Size = new Size(800, 600);
            SetStyle(ControlStyles.UserPaint, true);
        }
            
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            var winInfo = CefWindowInfo.Create();
            winInfo.SetAsChild(Handle, new CefRectangle { X = 0, Y = 0, Width = Width, Height = Height });
            CefBrowserHost.CreateBrowser(winInfo, new SimpleCefClient(), new CefBrowserSettings(), "www.baidu.com");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }
    }
}
