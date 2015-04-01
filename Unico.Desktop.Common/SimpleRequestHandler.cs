using System;
using Xilium.CefGlue;

namespace Unico.Desktop
{
    public class SimpleRequestHandler : CefRequestHandler
    {
        protected override bool OnBeforeResourceLoad(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            return base.OnBeforeResourceLoad(browser, frame, request);
        } 
    }
}
