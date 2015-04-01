using System;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    internal class SimpleLifeSpanHandler : CefLifeSpanHandler
    {
        protected override void OnAfterCreated(CefBrowser browser)
        {
        }
    }

    public class SimpleCefClient : CefClient
    {
        public CefLifeSpanHandler LifeSpanHandler { get; set; }

        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return LifeSpanHandler;
        }
    }
}
