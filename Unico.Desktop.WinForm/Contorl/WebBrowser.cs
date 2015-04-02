using System;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    class DevToolsClient : CefClient
    {
    }

    public class WebBrowser : UserControl
    {
        internal class MyLifeSpanHandler : CefLifeSpanHandler
        {
            public delegate void OnBrowserCreate(CefBrowser browser);

            public delegate void OnBrowserClose(CefBrowser browser);

            public event OnBrowserCreate BrowserCreate;
            public event OnBrowserClose BrowserClose;

            private WebBrowser webBrowser;

            public MyLifeSpanHandler(WebBrowser webBrowser)
            {
                this.webBrowser = webBrowser;
            }

            protected override void OnAfterCreated(CefBrowser browser)
            {
                base.OnAfterCreated(browser);
                if (BrowserCreate != null)
                    BrowserCreate(browser);
            }

            protected override void OnBeforeClose(CefBrowser browser)
            {
                base.OnBeforeClose(browser);
                if (BrowserClose != null)
                    BrowserClose(browser);
            }
        }

        private CefBrowser browser;
        private MyLifeSpanHandler lifeSpanHandler;
        private string url;

        public CefBrowser Browser
        {
            get
            {
                return browser; 
            }
        }

        public WebBrowser(string url)
            : base()
        {
            if (CefRuntime.Platform != CefRuntimePlatform.Windows)
                throw new NotSupportedException("For Windows only!");

            this.url = url;
            this.lifeSpanHandler = new MyLifeSpanHandler(this);
            this.lifeSpanHandler.BrowserCreate += this.OnBrowserCreate;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            var winInfo = CefWindowInfo.Create();
            winInfo.SetAsChild(Handle, new CefRectangle { X = 0, Y = 0, Width = Width, Height = Height });
            var client = new SimpleCefClient();
            client.LifeSpanHandler = this.lifeSpanHandler;
            CefBrowserHost.CreateBrowser(winInfo, client, new CefBrowserSettings(), this.url);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (browser != null)
            {
                var flags = NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoZOrder;
                var handle = browser.GetHost().GetWindowHandle();
                NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, Width, Height, flags); 
            }
        }

        private void OnBrowserCreate(CefBrowser browser)
        {
            this.browser = browser;
        }

        private void OnBrowserClose(CefBrowser browser)
        {
        }
    }
}