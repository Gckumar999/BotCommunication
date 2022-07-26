using Bot.Common.Services;
using Foundation;
using Microsoft.Botframework.Xamarin.ViewModels;
using System;
using UIKit;

namespace Bot.iOS
{
    public partial class ViewController : UIViewController
    {
        UIButton button;
        ConversationViewModel conversationViewModel;
        int count = 0;
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            button = new UIButton();
            button.BackgroundColor = UIColor.Green;
            button.SetTitle( "Click me!!",UIControlState.Normal);
            button.Frame = new CoreGraphics.CGRect(20, 20, 100, 50);
            View.AddSubview(button);
            View.BackgroundColor = UIColor.Yellow;
            button.TouchUpInside += Button_TouchUpInside;
            //button.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            //button.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;
            //button.HeightAnchor.ConstraintEqualTo(40).Active = true;
            //button.WidthAnchor.ConstraintEqualTo(100).Active = true;
            conversationViewModel = new ConversationViewModel(new BotService());
            // Perform any additional setup after loading the view, typically from a nib.
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {
            count++;
            conversationViewModel.Message = $"Hi";
            conversationViewModel.OnSendMessage(null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
