using System;

namespace OnRadio.App.Services
{
    public class DownloadResult
    {
        public bool Succeed { get; set; }
        public Uri LocalPath { get; set; }

        public static DownloadResult Fail()
        {
            return new DownloadResult();
        }

        public static DownloadResult Success(Uri localPath)
        {
            return new DownloadResult()
            {
                Succeed = true,
                LocalPath = localPath
            };
        }
    }
}