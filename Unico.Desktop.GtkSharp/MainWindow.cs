using System;
using Gtk;
using Xilium.CefGlue;
using Unico.Desktop;
using System.Runtime.InteropServices;

public partial class MainWindow: Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        DefaultWidth = 800;
        DefaultHeight = 600;
        Show ();
        DeleteEvent += (sender, e) =>
        {        
            if (CefRuntime.Platform == CefRuntimePlatform.Windows)
                Application.Quit();
            else 
                CefRuntime.QuitMessageLoop();
            e.RetVal = true;
        };
    }

    protected override void OnRealized()
    {
        base.OnRealized();
        ChildVisible = true;
        var windowInfo = CefWindowInfo.Create();
        switch (CefRuntime.Platform)
        {
            case CefRuntimePlatform.Windows:
                var parentHandle = gdk_win32_drawable_get_handle(GdkWindow.Handle);
                windowInfo.SetAsChild(parentHandle, new CefRectangle(0, 0, 0, 0)); // TODO: set correct  x, y, width, height  to do not waiting OnSizeAllocated event
                break;

            case CefRuntimePlatform.Linux:
                Console.WriteLine("REALIZED - RAW = {0}, HANDLE = {1}", Raw, Handle);
                windowInfo.SetAsChild(Handle, new CefRectangle(0, 0, 0, 0));
                break;

            case CefRuntimePlatform.MacOSX:
            default:
                throw new NotSupportedException();
        }
//        windowInfo.SetAsChild(Handle, new CefRectangle(0, 0, 0, 0));
        CefBrowserHost.CreateBrowser(windowInfo, new SimpleCefClient(), new CefBrowserSettings(), "www.baidu.com");

     //   _core.Create(windowInfo);
    }
    [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr gdk_win32_drawable_get_handle(IntPtr raw);
}
