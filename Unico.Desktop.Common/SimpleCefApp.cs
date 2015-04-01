using System;
using Xilium.CefGlue;
using System.Reflection;
using System.IO;

namespace Unico.Desktop
{
    public class SimpleCefApp : CefApp
    {
        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
        }
    }
}