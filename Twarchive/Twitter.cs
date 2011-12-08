namespace Twarchive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;

    internal static class Twitter
    {
        public static List<Tweet> DownloadAllTweets(string username)
        {
            string urlPrefix = "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + username + "&include_rts=true&exclude_replies=true&count=20&page=";
            return DownloadTweets(urlPrefix);
        }

        public static int DownloadNewTweets(string username, List<Tweet> tweets)
        {
            string lastId = tweets.First().Id;
            string urlPrefix = "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + username + "&since_id=" + lastId + "&include_rts=true&exclude_replies=true&count=20&page=";
            List<Tweet> newTweets = DownloadTweets(urlPrefix);
            tweets.InsertRange(0, newTweets);
            return newTweets.Count;
        }

        private static List<Tweet> DownloadTweets(string urlPrefix)
        {
            var tweets = new List<Tweet>();
            var wc = new WebClient();
            for (int i = 1; i <= 160; i++)
            {
                string xml = wc.DownloadString(urlPrefix+i);
                List<Tweet> newTweets = ParseXml(xml);
                if (newTweets.Count == 0)
                {
                    break;
                }
                tweets.AddRange(newTweets);
            }

            return tweets;
        }

        private static List<Tweet> ParseXml(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            return doc.Root.Descendants("status").Select(x => new Tweet()
            {
                Id=x.Element("id").Value,
                Text=WebUtility.HtmlDecode(x.Element("text").Value).Split('\n')[0],
                CreatedAt=x.Element("created_at").Value
            }).ToList();
        }
    }
}
