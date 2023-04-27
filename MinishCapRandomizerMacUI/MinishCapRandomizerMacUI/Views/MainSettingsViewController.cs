using ObjCRuntime;

namespace MinishCapRandomizerMacUI.Views;

[Register ("MainSettingsViewController")]
public class MainSettingsViewController : NSViewController {
    public MainSettingsViewController ()
    {
    }

    protected MainSettingsViewController (NativeHandle handle) : base (handle)
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

