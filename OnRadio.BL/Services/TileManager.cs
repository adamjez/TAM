using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using OnRadio.BL.Models;

namespace OnRadio.BL.Services
{
    public class TileManager : ITileManager
    {
        private readonly IImageManager _imageManager;
        public TileManager(IImageManager imageManager)
        {
            _imageManager = imageManager;
        }

        public async Task<bool> CreateTileAsync(RadioModel radio)
        {
            var result = await _imageManager.DownloadAndSaveAsync(radio.LogoUrl, radio.Id);

            if (!result.Succeed)
                return false;

            SecondaryTile secondaryTile = new SecondaryTile(radio.Id,
                radio.Title,
                radio.Id,
                result.LocalPath,
                TileSize.Default);

            return await secondaryTile.RequestCreateAsync();
        }

        public async Task<bool> RemoveTileAsync(RadioModel radio)
        {
            SecondaryTile secondaryTile = new SecondaryTile(radio.Id);

            await _imageManager.DeleteAsync(radio.Id);

            return await secondaryTile.RequestDeleteAsync();
        }

        public bool Exists(RadioModel radio)
        {
            return SecondaryTile.Exists(radio.Id);
        }
    }
}