namespace Twarchive
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage: Twarchive <twitterUsername>");
                    return;
                }

                string username = args[0];
                string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Twarchive." + username + ".txt");
                List<Tweet> tweets;
                if (File.Exists(filename))
                {
                    tweets = File.ReadAllLines(filename).Select(x => x.Split('\t')).Select(x => new Tweet()
                    {
                        Id = x[0],
                        Text = x[1],
                        CreatedAt = x[2]
                    }).ToList();
                    int n = Twitter.DownloadNewTweets(username, tweets);
                    Console.WriteLine("Downloaded {0} tweets", n);
                }
                else
                {
                    tweets = Twitter.DownloadAllTweets(username);
                    Console.WriteLine("Downloaded {0} tweets", tweets.Count);
                }

                File.WriteAllLines(filename, tweets.Select(x => string.Format("{0}\t{1}\t{2}", x.Id, x.Text, x.CreatedAt)).ToArray(), Encoding.UTF8);
            }
            catch (Exception x)
            {
                Console.Write("Error: {0}", x.Message);
                if (x.InnerException != null)
                {
                    Console.Write(" - {0}", x.InnerException.Message);
                }
                Console.WriteLine();
                Console.WriteLine(x.StackTrace);
            }
        }
    }
}
