﻿using System;
using Gtk;
using System.IO;
using Xilium.CefGlue;
using System.Reflection;

namespace Unico.Desktop
{
    class MainClass
    {
        [STAThread]
        public static int Main(string[] args)
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

            var libPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var settings = new CefSettings
            { 
                SingleProcess = false,
                MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows,
                ResourcesDirPath =libPath,
                LocalesDirPath = Path.Combine(libPath , "locales"),
                NoSandbox = CefRuntime.Platform == CefRuntimePlatform.Linux,
                WindowlessRenderingEnabled = true
            };

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);
            CefRuntime.RegisterSchemeHandlerFactory("http", "server.unico.local", new SimpleSchemeHandlerFactory());

            Application.Init();
            var win = new MainWindow();
            win.Show();
            CefRuntime.RunMessageLoop();
            CefRuntime.Shutdown();
            return 0;
        }
    }
}
