using System;
using Gtk;
using Xilium.CefGlue;
using Unico.Desktop;
using System.Runtime.InteropServices;

public partial class MainWindow: Window
{
    public MainWindow() : base(WindowType.Toplevel)
    {
        DefaultWidth = 800;
        DefaultHeight = 600;
//        var vbox = new VBox();
//        vbox.Add(new WebBrowser("www.baidu.com"));
//        Add(vbox);
        Add(new WebBrowser("www.baidu.com"));
        ShowAll();
        DeleteEvent += (sender, e) =>
        {        
            CefRuntime.QuitMessageLoop();
            e.RetVal = true;
        };
    }
}
