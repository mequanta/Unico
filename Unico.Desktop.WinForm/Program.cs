using Xilium.CefGlue;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Unico.Desktop
{
    class MainClass
    {
        [STAThread]
        private static int Main(string[] args)
        {
            CefRuntime.Load();

//            var cmdLine = CefCommandLine.Global;
//            Console.WriteLine("CommandLine: {0}", cmdLine);
//            if (!cmdLine.HasSwitch("type"))
//                Console.WriteLine("ProcessType: BrowserProcess");
            
            var mainArgs = new CefMainArgs(args);
            var app = new SimpleCefApp();
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
            {
                Console.WriteLine("CefRuntime.ExecuteProcess() returns {0}", exitCode);
                return exitCode;
            }

            var binDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var resourceDir = binDir;
            var localesDir = Path.Combine(binDir , "locales");
            var settings = new CefSettings
            { 
                SingleProcess = false,
                MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows,
                ResourcesDirPath =resourceDir,
                LocalesDirPath = localesDir,
                NoSandbox = true,
                WindowlessRenderingEnabled = true
            };

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);
            CefRuntime.RegisterSchemeHandlerFactory("http", "server.unico.local", new SimpleSchemeHandlerFactory());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!settings.MultiThreadedMessageLoop)
                Application.Idle += (s, e) => CefRuntime.DoMessageLoopWork();
            Application.Run(new MainForm());

            CefRuntime.Shutdown();
            return 0;
        }
    }
}
