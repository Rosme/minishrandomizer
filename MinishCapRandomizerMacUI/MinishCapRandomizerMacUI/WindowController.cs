using ObjCRuntime;
using RandomizerCore.Controllers;
using System;
namespace MinishCapRandomizerMacUI
{
    [Register("WindowController")]
    public class WindowController : NSWindowController
    {
        ShufflerController ShufflerController = new ShufflerController();

        protected WindowController(NativeHandle handle) : base(handle)
        {

        }

        public override void WindowDidLoad()
        {
            base.WindowDidLoad();

            Window.Title = $"Minish Cap Randomizer {ShufflerController.VersionName} {ShufflerController.RevName}";
        }
    }
}

