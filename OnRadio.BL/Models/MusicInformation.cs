using System;

namespace OnRadio.BL.Models
{
    public class MusicInformation
    {
        private string _title = string.Empty;
        private string _album = string.Empty;
        private string _thumbnailUrl = string.Empty;
        private string _artist = string.Empty;

        public string Title
        {
            get { return _title; }
            set { _title = value ?? string.Empty; }
        }

        public string Album
        {
            get { return _album; }
            set { _album = value ?? string.Empty; }
        }

        public string ThumbnailUrl
        {
            get { return _thumbnailUrl; }
            set { _thumbnailUrl = value ?? string.Empty; }
        }

        public string Artist
        {
            get { return _artist; }
            set { _artist = value ?? string.Empty; }
        }
    }
}