using System;
using AppKit;
using Foundation;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    internal class MyLifeSpanHandler : CefLifeSpanHandler
    {
        public delegate void OnAfterBrowserCreate(CefBrowser browser);

        public delegate void OnBeforeBrowserClose(CefBrowser browser);

        public event OnAfterBrowserCreate AfterBrowserCreate;
        public event OnBeforeBrowserClose BeforeBrowserClose;

        public MyLifeSpanHandler(WebBrowser webBrowser)
        {
            //this.webBrowser = webBrowser;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);
            if (AfterBrowserCreate != null)
                AfterBrowserCreate(browser);
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
            base.OnBeforeClose(browser);
            if (BeforeBrowserClose != null)
                BeforeBrowserClose(browser);
        }
    }

    public class WebBrowser : NSView
    {
        private string url;
        private MyLifeSpanHandler lifeSpanHandler;
        private CefBrowser browser;
        public WebBrowser(string url)
            : base()
        {
          //  Initialize();
            this.url = url;
            this.lifeSpanHandler = new MyLifeSpanHandler(this);
            this.lifeSpanHandler.AfterBrowserCreate += OnAfterBrowserCreate;

            var windowInfo = CefWindowInfo.Create();
            var rect = Bounds;
            Console.WriteLine(Handle);
            windowInfo.SetAsChild(Handle, new CefRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
            var client = new SimpleCefClient();
            client.LifeSpanHandler = this.lifeSpanHandler;
            CefBrowserHost.CreateBrowser(windowInfo, new SimpleCefClient(), new CefBrowserSettings(), this.url);
        }

        private void OnAfterBrowserCreate(CefBrowser browser)
        {
            Console.WriteLine("browser created");
            this.browser = browser;
        }
    }
}

