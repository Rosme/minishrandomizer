using ObjCRuntime;

namespace MinishCapRandomizerMacUI.Views;

[Register ("WorldSettingsViewController")]
public class WorldSettingsViewController : NSViewController {
    public WorldSettingsViewController ()
    {
    }

    protected WorldSettingsViewController (NativeHandle handle) : base (handle)
    {
        // This constructor is required if the view controller is loaded from a xib or a storyboard.
        // Do not put any initialization here, use ViewDidLoad instead.
    }

    public override void ViewDidLoad ()
    {
        base.ViewDidLoad ();

        // Perform any additional setup after loading the view
    }
}

