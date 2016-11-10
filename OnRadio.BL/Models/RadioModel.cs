using System.Collections.Generic;

namespace OnRadio.BL.Models
{
    public class RadioModel : IMusicFormatter
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string LogoUrl { get; set; }
        public List<StreamModel> Streams { get; set; }
        public int Listenters { get; set; }
        public bool OnAir { get; set; }
        public bool IsFavorite { get; set; }

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