namespace Twarchive
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;

    internal static class Downloader
    {
        private const int NumRetries = 10;

        private static WebClient webClient;

        static Downloader()
        {
            webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Downloads the contents of a URL
        /// Retries downloading after increasing delays
        /// Throws an exception after failing N=10 times
        /// </summary>
        /// <param name="url">The URL to download</param>
        /// <returns>The contents of the URL</returns>
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
                        Thread.Sleep(i * 1000);
                    }
                }
            }

            throw new Exception("Unable to download URL: " + url);
        }
    }
}
