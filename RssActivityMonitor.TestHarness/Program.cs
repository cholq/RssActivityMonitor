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
            inputRssFeeds.Add("WNYC", "http://feeds.wnyc.org/experiment_podcast");

            IRssReader myRssReader = new RssReader();
            IRssActivityMonitor myActivityMonitor = new ActivityMonitor(myRssReader);

            List<string> InactiveCompanies = myActivityMonitor.FindInactiveRssFeeds(inputRssFeeds);

        }
    }
}
