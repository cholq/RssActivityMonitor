using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RssActivityMonitor.Interfaces;
using RssActivityMonitor.Services;
using Moq;

namespace RssActivityMonitorTests
{
    [TestClass]
    public class RssActivityMonitorTests
    {
        IRssReader mockReader;

        [TestMethod]
        public void FindInactiveRssFeeds_CompanyWith1InactiveFeed_ReturnCompanyInList()
        {
            DateTimeOffset inactiveDate = DateTimeOffset.Now.AddHours(-25);
            string companyName = "Fake Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.Setup(r => r.MostRecentItemPublished).Returns(inactiveDate);
            mock.SetupSequence(r => r.IsLoaded)
                .Returns(false)
                .Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName, new List<string>() { "http://fake.feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds);

            Assert.AreEqual(InactiveCompanies.Count, 1);
            Assert.AreEqual(InactiveCompanies[0], companyName, false, "Company Name is not the correct value");

        }

        [TestMethod]
        public void FindInactiveRssFeeds_CompanyWith1ActiveFeed_ReturnEmptyList()
        {
            DateTimeOffset inactiveDate = DateTimeOffset.Now.AddHours(-23);
            string companyName = "Fake Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.Setup(r => r.MostRecentItemPublished).Returns(inactiveDate);
            mock.SetupSequence(r => r.IsLoaded)
                .Returns(false)
                .Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName, new List<string>() { "http://fake.feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds);

            Assert.AreEqual(InactiveCompanies.Count, 0);

        }

    }
}
