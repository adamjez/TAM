using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;

namespace OnRadio.App.Messages
{
    public class FavoriteChangeMessage : MessageBase
    {
        public string RadioId { get; set; }
        public bool Favorited { get; set; }

        public FavoriteChangeMessage(object sender, string radioId, bool favorited)
            : base(sender)
        {
            Sender = sender;
            RadioId = radioId;
            Favorited = favorited;
        }
    }
}