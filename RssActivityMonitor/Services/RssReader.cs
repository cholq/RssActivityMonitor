using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Xml;
using RssActivityMonitor.Interfaces;

namespace RssActivityMonitor.Services
{
    public class RssReader : IRssReader
    {

        private SyndicationFeed _rssFeed = null;
        private string _errorMessage = string.Empty;

        public bool LoadRssFeed(string url)
        {
            bool loadSucces = false;

            try
            {
                XmlReader xmlReader = XmlReader.Create(url);

                this._rssFeed = SyndicationFeed.Load(xmlReader);
                xmlReader.Close();
                loadSucces = true;
                this._errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                this._errorMessage = ex.Message;
            }

            return loadSucces;
        }

        public void Close()
        {
            this._rssFeed = null;
            this._errorMessage = string.Empty;
        }

        public bool IsLoaded
        {
            get { return !(this._rssFeed == null); }
        }

        public string ErrorMessage
        {
            get { return this._errorMessage; }
        }

        public DateTimeOffset LastUpdateTime
        {
            get
            {
                return IsLoaded ? this._rssFeed.LastUpdatedTime : DateTimeOffset.MinValue;
            }
        }

        public DateTimeOffset MostRecentItemPublished
        {
            get
            {
                if (IsLoaded)
                {
                    return (this._rssFeed.Items.Aggregate((i1, i2) => i1.PublishDate > i2.PublishDate ? i1 : i2)).PublishDate;
                }
                else
                {
                    return DateTimeOffset.MinValue;
                }
            }
        }
    }
}
