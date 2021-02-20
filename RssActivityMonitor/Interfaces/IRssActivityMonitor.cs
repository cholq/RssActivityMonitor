using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssActivityMonitor.Interfaces
{
    public interface IRssActivityMonitor
    {
        List<string> FindInactiveRssFeeds(Dictionary<string, List<string>> FeedsToCheck, int NumberOfDays = 1, bool ConsiderErrorsInactive = true);
    }
}
