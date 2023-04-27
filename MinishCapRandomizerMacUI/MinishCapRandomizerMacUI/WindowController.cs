using ObjCRuntime;

using System;
namespace MinishCapRandomizerMacUI
{
    [Register("WindowController")]
    public class WindowController : NSWindowController
    {
        protected WindowController(NativeHandle handle) : base(handle)
        {

        }

        public override void WindowDidLoad()
        {
            base.WindowDidLoad();


        }
    }
}

