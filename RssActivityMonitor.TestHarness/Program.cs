using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssActivityMonitor.Interfaces;
using RssActivityMonitor.Services;

namespace RssActivityMonitor.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> inputRssFeeds = new Dictionary<string, string>();
            inputRssFeeds.Add("New York Times", "https://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml");
            inputRssFeeds.Add("USA Today", "http://rssfeeds.usatoday.com/UsatodaycomNation-TopStories");

            IRssReader myRssReader = new RssReader();
            IRssActivityMonitor myActivityMonitor = new ActivityMonitor(myRssReader);

            myActivityMonitor.FindInactiveRssFeeds(inputRssFeeds);

            //bool success = false;
            //foreach(KeyValuePair<string, string> feed in inputRssFeeds)
            //{
            //    success = myRssReader.LoadRssFeed(feed.Value);
            //    Console.WriteLine(string.Format("Loaded feed for {0} successfully: {1}", feed.Key, myRssReader.IsLoaded.ToString()));

            //    myRssReader.Close();
            //}
        }
    }
}
