using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Xilium.CefGlue;
using System.Drawing;

namespace Unico.Desktop
{
    public partial class MainWindow : NSWindow
    {
        #region Constructors

        public MainWindow(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }
		
        [Export("initWithCoder:")]
        public MainWindow(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }
		
        void Initialize()
        {
            var windowInfo = CefWindowInfo.Create();
            var rect = ContentView.Bounds;
            windowInfo.SetAsChild(ContentView.Handle, new CefRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
            CefBrowserHost.CreateBrowser(windowInfo, new SimpleCefClient(), new CefBrowserSettings(), "www.baidu.com");
        }

        #endregion
    }
}
