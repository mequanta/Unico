using System;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    internal class SimpleLifeSpanHandler : CefLifeSpanHandler
    {
        protected override void OnAfterCreated(CefBrowser browser)
        {
            Console.WriteLine("browser created");
        }
    }

    public class SimpleCefClient : CefClient
    {
        private CefLifeSpanHandler lifeSpanHandler = new SimpleLifeSpanHandler();

        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return lifeSpanHandler;
        }
    }
}

