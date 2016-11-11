using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace OnRadio.BL.Models
{
    public class RadioModel : ViewModelBase, IMusicFormatter
    {
        private bool _isFavorite;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string LogoUrl { get; set; }
        public List<StreamModel> Streams { get; set; }
        public int Listenters { get; set; }
        public bool OnAir { get; set; }

        // Should be in RadioViewModel -> too much work mapping these two items, maybe later
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { Set(ref _isFavorite, value); }
        }

        public MusicInformation CreateMusicInformation()
        {
            return new MusicInformation()
            {
                Title = Title,
                ThumbnailUrl = LogoUrl,
                Artist = string.Empty,
                Album = string.Empty
            };
        }
    }
}