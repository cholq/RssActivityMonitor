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

            Assert.AreEqual(1, InactiveCompanies.Count);
            Assert.AreEqual(companyName, InactiveCompanies[0], false, "Company Name is not the correct value");

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

            Assert.AreEqual(0, InactiveCompanies.Count);

        }

        [TestMethod]
        public void FindInactiveRssFeeds_CompanyWithMultipleFeedMixActivity_ReturnEmptyList()
        {
            DateTimeOffset activeDate = DateTimeOffset.Now.AddHours(-23);
            DateTimeOffset inactiveDate = DateTimeOffset.Now.AddHours(-25);
            string companyName = "Fake Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.SetupSequence(r => r.MostRecentItemPublished)
                .Returns(activeDate)
                .Returns(activeDate)
                .Returns(inactiveDate)
                .Returns(inactiveDate);
            mock.Setup(r => r.IsLoaded).Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName, new List<string>() { "http://fake.feed.xml", "http://fake.second-feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds);

            Assert.AreEqual(0, InactiveCompanies.Count);

        }

        [TestMethod]
        public void FindInactiveRssFeeds_TwoCompaniesOneInactive_ReturnListOfOne()
        {
            DateTimeOffset activeDate = DateTimeOffset.Now.AddHours(-23);
            DateTimeOffset inactiveDate = DateTimeOffset.Now.AddHours(-25);
            string companyName1 = "Fake Company";
            string companyName2 = "Other Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.SetupSequence(r => r.MostRecentItemPublished)
                .Returns(activeDate)
                .Returns(activeDate)
                .Returns(inactiveDate)
                .Returns(inactiveDate);
            mock.Setup(r => r.IsLoaded).Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName1, new List<string>() { "http://fake.feed.xml"});
            inputRssFeeds.Add(companyName2, new List<string>() { "http://fake.second-feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds);

            Assert.AreEqual(1, InactiveCompanies.Count);
            Assert.IsTrue(InactiveCompanies.Contains(companyName2), "Company 2 should have been inactive");
            Assert.IsFalse(InactiveCompanies.Contains(companyName1), "Company 1 should not have been inactive");

        }

        [TestMethod]
        public void FindInactiveRssFeeds_TwoCompaniesBothInactive_ReturnListOfTwo()
        {
            DateTimeOffset inactiveDate1 = DateTimeOffset.Now.AddHours(-26);
            DateTimeOffset inactiveDate2 = DateTimeOffset.Now.AddHours(-25);
            string companyName1 = "Fake Company";
            string companyName2 = "Other Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.SetupSequence(r => r.MostRecentItemPublished)
                .Returns(inactiveDate1)
                .Returns(inactiveDate1)
                .Returns(inactiveDate2)
                .Returns(inactiveDate2);
            mock.Setup(r => r.IsLoaded).Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName1, new List<string>() { "http://fake.feed.xml" });
            inputRssFeeds.Add(companyName2, new List<string>() { "http://fake.second-feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds);

            Assert.AreEqual(2, InactiveCompanies.Count);
            Assert.IsTrue(InactiveCompanies.Contains(companyName1), "Company 1 should have been inactive");
            Assert.IsTrue(InactiveCompanies.Contains(companyName2), "Company 2 should have been inactive");

        }

        [TestMethod]
        public void FindInactiveRssFeeds_ChangeDaysParamToTwo_ReturnListOfZero()
        {
            DateTimeOffset inactiveDate1 = DateTimeOffset.Now.AddHours(-26);
            DateTimeOffset inactiveDate2 = DateTimeOffset.Now.AddHours(-25);
            string companyName1 = "Fake Company";
            string companyName2 = "Other Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.SetupSequence(r => r.MostRecentItemPublished)
                .Returns(inactiveDate1)
                .Returns(inactiveDate1)
                .Returns(inactiveDate2)
                .Returns(inactiveDate2);
            mock.Setup(r => r.IsLoaded).Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName1, new List<string>() { "http://fake.feed.xml" });
            inputRssFeeds.Add(companyName2, new List<string>() { "http://fake.second-feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds, 2);

            Assert.AreEqual(0, InactiveCompanies.Count);

        }

        [TestMethod]
        public void FindInactiveRssFeeds_RssReaderErrorTreatAsInactive_ReturnListOfOne()
        {
            DateTimeOffset inactiveDate1 = DateTimeOffset.Now.AddHours(-26);
            DateTimeOffset inactiveDate2 = DateTimeOffset.Now.AddHours(-25);
            string companyName1 = "Fake Company";
            string companyName2 = "Other Company";

            var mock = new Mock<IRssReader>();
            mock.Setup(r => r.LoadRssFeed(It.IsAny<string>())).Returns(true);
            mock.SetupSequence(r => r.MostRecentItemPublished)
                .Returns(inactiveDate1)
                .Returns(inactiveDate1)
                .Returns(inactiveDate2)
                .Returns(inactiveDate2);
            mock.Setup(r => r.IsLoaded).Returns(true);
            mockReader = mock.Object;

            IRssActivityMonitor monitor = new ActivityMonitor(mockReader);

            Dictionary<string, List<string>> inputRssFeeds = new Dictionary<string, List<string>>();
            inputRssFeeds.Add(companyName1, new List<string>() { "http://fake.feed.xml" });
            inputRssFeeds.Add(companyName2, new List<string>() { "http://fake.second-feed.xml" });

            List<string> InactiveCompanies = monitor.FindInactiveRssFeeds(inputRssFeeds, 2);

            Assert.AreEqual(0, InactiveCompanies.Count);

        }

    }
}
