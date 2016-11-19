using GalaSoft.MvvmLight;

namespace OnRadio.BL.Models
{
    public class RadioInfoModel : ViewModelBase
    {
        public string Id { get; set; }
        public string Web1 { get; set; }
        public string Web2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Gplus { get; set; }
        public string Youtube { get; set; }

    }
}