namespace Twarchive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;

    internal static class Twitter
    {
        private const int MaxTweetsPerPage = 200;

        public static List<Tweet> DownloadAllTweets(string username)
        {
            return DownloadTweets(username, null, null);
        }

        public static int DownloadNewTweets(string username, List<Tweet> tweets)
        {
            string sinceId = tweets.First().Id;
            List<Tweet> newTweets = DownloadTweets(username, sinceId, null);
            tweets.InsertRange(0, newTweets);
            return newTweets.Count;
        }

        private static List<Tweet> DownloadTweets(string username, string sinceId, string maxId)
        {
            string url = "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + username + "&trim_user=true&include_rts=true&exclude_replies=true&count=" + MaxTweetsPerPage;
            if (!string.IsNullOrEmpty(sinceId))
            {
                url += "&since_id=" + sinceId;
            }
            if (!string.IsNullOrEmpty(maxId))
            {
                url += "&max_id=" + maxId;
            }

            string xml = Downloader.Download(url);
            List<Tweet> newTweets = ParseXml(xml);
            if (newTweets.Count == 0)
            {
                return newTweets;
            }
            else
            {
                string newMaxId = (long.Parse(newTweets.Last().Id) - 1).ToString();
                newTweets.AddRange(DownloadTweets(username, sinceId, newMaxId));
                return newTweets;
            }
        }

        private static List<Tweet> ParseXml(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            return doc.Root.Descendants("status").Select(x => new Tweet()
            {
                Id = x.Element("id").Value,
                Text = HttpUtility.HtmlDecode(x.Element("text").Value).Split('\n')[0],
                CreatedAt = x.Element("created_at").Value
            }).ToList();
        }
    }
}
