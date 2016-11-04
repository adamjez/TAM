using System;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Toolkit.Uwp;
using OnRadio.App.Services;

namespace OnRadio.BL.Services
{
    public class ImageManager : IImageManager
    {
        private const string PathPrefix = @"ms-appdata:///local/";


        public async Task<DownloadResult> DownloadAndSaveAsync(string uri, string fileName)
        {
            var distantUri = new Uri(uri);
            // Load a specific image from the cache. If the image is not in the cache, ImageCache will try to download and store it
         
            byte[] buffer;
            {
                try
                {
                    using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                    {
                        buffer = await client.GetByteArrayAsync(distantUri); // Download file
                    }
                }
                catch
                {
                    return DownloadResult.Fail();
                }
            }

            await StorageFileHelper.WriteBytesToLocalFileAsync(buffer, fileName, CreationCollisionOption.ReplaceExisting);

            return DownloadResult.Success(new Uri(PathPrefix + fileName));
        }

        public async Task DeleteAsync(string fileName)
        {
            var localFile = PathPrefix + fileName;

            var item = await StorageFile.GetFileFromPathAsync(localFile);

            await item.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}