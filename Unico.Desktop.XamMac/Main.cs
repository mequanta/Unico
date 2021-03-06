﻿using System;
using AppKit;
using Xilium.CefGlue;
using Foundation;

namespace Unico.Desktop
{
    class SimpleApplication : NSApplication
    {
        public SimpleApplication(IntPtr handle) : base(handle)
        {
        }
        public SimpleApplication(NSCoder coder) : base(coder)
        {
        }
        public SimpleApplication(NSObjectFlag t) : base(t)
        {
        }
    }

    static class MainClass
    {
        static int Main(string[] args)
        {
            CefRuntime.Load();
            var mainArgs = new CefMainArgs(args);
            var app = new SimpleCefApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode >= 0)
                return exitCode;

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;
            settings.SingleProcess = false;

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            SimpleApplication.Init();
            //SimpleApplication.SharedApplication.Delegate = new AppDelegate();
            SimpleApplication.Main(args);

            CefRuntime.Shutdown();
            return 0;
        }
    }
}
