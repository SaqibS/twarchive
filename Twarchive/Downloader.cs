namespace Twarchive
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;

    internal static class Downloader
    {
        private const int NumRetries = 3;

        private static WebClient webClient;

        static Downloader()
        {
        webClient=new WebClient();
        webClient.Encoding = Encoding.UTF8;
    }

        public static string Download(string url)
        {
            for (int i = 1; i <= NumRetries; i++)
            {
                try
                {
                    return webClient.DownloadString(url);
                }
                catch
                {
                    if (i == NumRetries)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }

            throw new Exception("Unable to download URL: " + url);
        }
    }
}
