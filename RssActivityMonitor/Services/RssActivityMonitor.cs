using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssActivityMonitor.Interfaces;

namespace RssActivityMonitor.Services
{
    public class ActivityMonitor : IRssActivityMonitor
    {

        private IRssReader _reader;

        public ActivityMonitor(IRssReader rssReader)
        {
            this._reader = rssReader;
        }

        public List<string> FindInactiveRssFeeds(Dictionary<string, string> FeedsToCheck, int NumberOfDays = 1, bool ConsiderErrorsInactive = true)
        {

            List<string> InactiveCompanies = new List<string>();

            foreach (KeyValuePair<string, string> feed in FeedsToCheck)
            {
                DateTimeOffset lastActivityDate = GetRssFeedLastActivityDate(feed.Value);
            }

            return InactiveCompanies;
        }

        private DateTimeOffset GetRssFeedLastActivityDate(string url)
        {
            if (this._reader.IsLoaded) { this._reader.Close(); }

            if (this._reader.LoadRssFeed(url))
            {
                return (this._reader.LastUpdateTime > this._reader.MostRecentItemPublished) ? this._reader.LastUpdateTime : this._reader.MostRecentItemPublished;
            }

            return DateTimeOffset.MinValue;
        }
    }
}
