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

        /// <summary>
        ///  Function takes a Dictionary keyed by Company Name (string) and valued by a list of RSS Feed URLs (List<string>) 
        ///  and finds which companies have inactive RSS Feeds.
        /// </summary>
        /// <param name="FeedsToCheck">Dictionary of Companies with their RSS Feeds</param>
        /// <param name="NumberOfDays">Number of days back from current Date-Time to check for activity</param>
        /// <param name="ConsiderErrorsInactive">If an exception is thrown while looking at the RSS Feed, treat that feed as inactive</param>
        /// <returns>A list of Company Names that have been inactive on RSS Feeds</returns>
        public List<string> FindInactiveRssFeeds(Dictionary<string, List<string>> FeedsToCheck, int NumberOfDays = 1, bool ConsiderErrorsInactive = true)
        {
            if (FeedsToCheck == null) { throw new ArgumentOutOfRangeException("FeedsToCheck", "Dictionary of Feeds to Check cannot be NULL"); }
            if (NumberOfDays <= 0) { throw new ArgumentOutOfRangeException("NumberOfDays", "Value must be greater than zero"); }

            DateTimeOffset compareDate = DateTimeOffset.Now.AddDays(NumberOfDays * -1);

            Dictionary<string, DateTimeOffset> MostRecentActivity = new Dictionary<string, DateTimeOffset>();

            foreach (KeyValuePair<string, List<string>> company in FeedsToCheck)
            {
                foreach (string url in company.Value)
                {
                    try
                    {
                        DateTimeOffset lastActivityDate = GetRssFeedLastActivityDate(url);
                        MostRecentActivity = this.AddDateTimeOffsetToDictionary(MostRecentActivity, company.Key, lastActivityDate);
                    }
                    catch (Exception ex)
                    {
                        if (ConsiderErrorsInactive)
                        {
                            MostRecentActivity = this.AddDateTimeOffsetToDictionary(MostRecentActivity, company.Key, DateTimeOffset.MinValue);
                        }
                        else
                        {
                            throw;
                        }
                    }
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
            else
            {
                throw new Exception(string.Format("Unable to load RSS Feed: {0}", url));
            }

            return DateTimeOffset.MinValue;
        }

        private Dictionary<string, DateTimeOffset> AddDateTimeOffsetToDictionary(Dictionary<string, DateTimeOffset> dict, string key, DateTimeOffset value)
        {
            if (dict.ContainsKey(key))
            {
                if (dict[key] < value)
                {
                    dict[key] = value;
                }
            }
            else
            {
                dict.Add(key, value);
            }

            return dict;
        }
    }
}
