﻿using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace Unico.Desktop
{
    public partial class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;

        public override void FinishedLaunching(NSObject notification)
        {
            mainWindowController = new MainWindowController();
            mainWindowController.Window.MakeKeyAndOrderFront(this);
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }
    }
}

