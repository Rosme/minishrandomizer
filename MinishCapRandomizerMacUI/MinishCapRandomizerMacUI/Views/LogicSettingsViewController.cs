using ObjCRuntime;

namespace MinishCapRandomizerMacUI.Views;

[Register ("LogicSettingsViewController")]
public class LogicSettingsViewController : NSViewController {
    public LogicSettingsViewController ()
    {
    }

    protected LogicSettingsViewController (NativeHandle handle) : base (handle)
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

