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

            DateTimeOffset compareDate = DateTimeOffset.Now.AddDays(NumberOfDays * -1);

            Dictionary<string, DateTimeOffset> MostRecentActivity = new Dictionary<string, DateTimeOffset>();

            foreach (KeyValuePair<string, string> feed in FeedsToCheck)
            {
                DateTimeOffset lastActivityDate = GetRssFeedLastActivityDate(feed.Value);
                if (MostRecentActivity.ContainsKey(feed.Key))
                {
                    if (MostRecentActivity[feed.Key] < lastActivityDate)
                    {
                        MostRecentActivity[feed.Key] = lastActivityDate;
                    }
                }
                else
                {
                    MostRecentActivity.Add(feed.Key, lastActivityDate);
                }
            }

            return new List<string>(MostRecentActivity.Where(x => x.Value < compareDate).Select(y => y.Key));
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
