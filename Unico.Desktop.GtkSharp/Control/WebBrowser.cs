using System;
using Gtk;
using Gdk;
using Xilium.CefGlue;
using System.Runtime.InteropServices;

namespace Unico.Desktop
{
    public class WebBrowser : Bin
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
        {
            if (CefRuntime.Platform != CefRuntimePlatform.Linux)
                throw new NotSupportedException();
            this.url = url;
            this.lifeSpanHandler = new MyLifeSpanHandler(this);
            this.lifeSpanHandler.BrowserCreate += this.OnBrowserCreate;
        }

        protected override void OnRealized()
        {
            base.OnRealized();
            var windowInfo = CefWindowInfo.Create();
            var handle = NativeMethods.gdk_x11_drawable_get_xid(GdkWindow.Handle);
            windowInfo.SetAsChild(handle, new CefRectangle(0, 0, 0, 0));
      //      CefBrowserHost.CreateBrowser(windowInfo, new SimpleCefClient(), new CefBrowserSettings(), "www.baidu.com");
        }

        protected override void OnSizeAllocated(Rectangle allocation)
        {
            base.OnSizeAllocated(allocation);
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

