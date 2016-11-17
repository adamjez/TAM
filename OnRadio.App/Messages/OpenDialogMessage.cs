using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;

namespace OnRadio.App.Messages
{
    public class OpenDialogMessage : MessageBase
    {
        public string DialogName { get; set; }

        public OpenDialogMessage(object sender, string dialogName)
            : base(sender)
        {
            Sender = sender;
            DialogName = dialogName;
        }
    }
}