namespace Twarchive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Xml.Linq;

    internal static class Twitter
    {
        private const int MaxTweetsThatCanBeRetrieved = 3200;
        private const int TweetsPerPage = 20;
        private const int PagesToProcess = MaxTweetsThatCanBeRetrieved / TweetsPerPage;

        public static List<Tweet> DownloadAllTweets(string username)
        {
            string urlPrefix = string.Format("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name={0}&include_rts=true&exclude_replies=true&count={1}&page=", username, TweetsPerPage);
            return DownloadTweets(urlPrefix);
        }

        public static int DownloadNewTweets(string username, List<Tweet> tweets)
        {
            string lastId = tweets.First().Id;
            string urlPrefix = string.Format("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name={0}&since_id={1}&include_rts=true&exclude_replies=true&count={2}&page=", username, lastId, TweetsPerPage);
            List<Tweet> newTweets = DownloadTweets(urlPrefix);
            tweets.InsertRange(0, newTweets);
            return newTweets.Count;
        }

        private static List<Tweet> DownloadTweets(string urlPrefix)
        {
            var tweets = new List<Tweet>();
            var wc = new WebClient();
            for (int page = 1; page <= PagesToProcess; page++)
            {
                string url = urlPrefix + page;
                string xml = wc.DownloadString(url);
                List<Tweet> newTweets = ParseXml(xml);
                if (newTweets.Count == 0)
                {
                    break;
                }

                tweets.AddRange(newTweets);

                Thread.Sleep(1000);
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
