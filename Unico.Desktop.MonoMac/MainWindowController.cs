using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Unico.Desktop
{
    public partial class MainWindowController : NSWindowController
    {
        #region Constructors

        public MainWindowController(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }
		
        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }
		
        public MainWindowController()
            : base("MainWindow")
        {
            Initialize();
        }
		
        void Initialize()
        {
        }

        #endregion

        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }
    }
}
