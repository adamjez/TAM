namespace OnRadio.BL.Models
{
    public class SongModel : IMusicFormatter
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Artist { get; set; }

        public static SongModel CreateUndefined()
        {
            return new SongModel()
            {
                Title = "undefined"
            };
        }

        public MusicInformation CreateMusicInformation()
        {
            return new MusicInformation()
            {
                Album = Album,
                Artist = Artist,
                ThumbnailUrl = ThumbnailUrl,
                Title = Title
            };
        }
    }
}