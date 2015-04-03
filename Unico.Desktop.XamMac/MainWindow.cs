using System;
using Foundation;
using AppKit;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    public partial class MainWindow : NSWindow
    {
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

        private void Initialize()
        {
            ContentView.AddSubview(new WebBrowser("www.baidu.com"));
//            var windowInfo = CefWindowInfo.Create();
//            var rect = ContentView.Bounds;
//
//            windowInfo.SetAsChild(ContentView.Handle, new CefRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
//            var client = new SimpleCefClient();
//            client.LifeSpanHandler = new MyLifeSpanHandler();
//            CefBrowserHost.CreateBrowser(windowInfo, new SimpleCefClient(), new CefBrowserSettings(), "www.baidu.com");
        }
    }
}
