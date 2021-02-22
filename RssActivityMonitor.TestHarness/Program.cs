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
            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add("New York Times", new List<string>() { "https://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml", "https://rss.nytimes.com/services/xml/rss/nyt/US.xml" });
            inputRssFeeds.Add("USA Today", new List<string>() { "http://rssfeeds.usatoday.com/UsatodaycomNation-TopStories" });
            inputRssFeeds.Add("WNYC", new List<string>() { "http://feeds.wnyc.org/experiment_podcast" });
            inputRssFeeds.Add("Fake", new List<string>() { "http://fake.feed.xml" });

            IRssReader myRssReader = new RssReader();
            IRssActivityMonitor myActivityMonitor = new ActivityMonitor(myRssReader);

            try
            {
                List<string> InactiveCompanies = myActivityMonitor.FindInactiveRssFeeds(inputRssFeeds, 1, true);
                Console.WriteLine("inactives: " + InactiveCompanies.Count.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

        }
    }
}
