using Windows.Media.Playback;

namespace OnRadio.BL.Services
{
    public interface IMediaNotify
    {
        void Update(MediaPlaybackSession sender, object args);
    }
}