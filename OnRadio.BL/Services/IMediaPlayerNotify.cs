using Windows.Media.Playback;

namespace OnRadio.BL.Services
{
    public interface IMediaPlayerNotify
    {
        void Update(MediaPlaybackSession sender, object args);
    }
}