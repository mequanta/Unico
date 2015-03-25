using Xilium.CefGlue;
using System;
using System.Windows.Forms;

namespace Unico.Desktop
{
    class MainClass
    {
        [STAThread]
        private static int Main(string[] args)
        {
            CefRuntime.Load();
            var mainArgs = new CefMainArgs(args);
            var app = new SimpleCefApp();
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
            {
                Console.WriteLine("CefRuntime.ExecuteProcess() returns {0}", exitCode);
                return exitCode;
            }

            var settings = new CefSettings { SingleProcess = false, MultiThreadedMessageLoop = true };
            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);
            CefRuntime.RegisterSchemeHandlerFactory("http", "server.unico.local", new SimpleSchemeHandlerFactory());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            CefRuntime.Shutdown();
            return 0;
        }
    }
}
