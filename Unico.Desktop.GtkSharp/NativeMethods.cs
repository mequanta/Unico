using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Unico.Desktop
{
    internal static class NativeMethods
    {
        private const string GdkNativeDll = "libgdk-win32-2.0-0.dll";

        [DllImport(GdkNativeDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_win32_drawable_get_handle(IntPtr raw);

        [DllImport(GdkNativeDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_x11_drawable_get_xid(IntPtr raw);
    }
}
