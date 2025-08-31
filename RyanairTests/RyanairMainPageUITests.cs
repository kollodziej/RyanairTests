using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace RyanairTests
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }

        [Test]
        public void OneWayFlight_WarsawToPaphos_ShouldReturnResults()
        {
            var mainPage = new RyanairMainPage(driver, wait);
            mainPage.Open();

            mainPage.AcceptCookiesIfVisible();
            mainPage.SelectOneWay();
            mainPage.SetDeparture("Warszawa-Chopina", "Warszawa-Chopina");
            mainPage.SetArrival("Pafos", "Pafos");
            mainPage.SelectFirstAvailableDate();
            mainPage.Search();

            var resultsPage = new RyanairResultsPage(driver, wait);
            bool searchedDate = resultsPage.AssertTodayIsSelectedDate();
            Assert.IsTrue(resultsPage.HasFlights(), "Brak dostêpnych lotów.");
            resultsPage.SelectFirstFlight();
        }
    }
}
