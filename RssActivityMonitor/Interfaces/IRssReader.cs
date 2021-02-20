using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssActivityMonitor.Interfaces
{
    public interface IRssReader
    {
        bool LoadRssFeed(string url);

        void Close();

        bool IsLoaded { get; }

        string ErrorMessage { get; }
    }
}
