using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace RyanairTests
{
    internal class RyanairResultsPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public RyanairResultsPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        private By FlightCards => By.CssSelector("flight-card-new[data-ref='flight-card_all_information']");

        public int GetFlightsCount()
        {
            var flights = wait.Until(d => d.FindElements(FlightCards));
            return flights.Count;
        }

        public bool HasFlights()
        {
            return GetFlightsCount() > 0;
        }
        public void SelectFirstFlight()
        {
            var flights = wait.Until(d => d.FindElements(FlightCards));
            flights.First().Click();
        }

        public bool AssertTodayIsSelectedDate()
        {
            var selectedDate = wait.Until(d =>
                d.FindElement(By.CssSelector("button[data-e2e='date-item'][data-selected='true']"))
            );

            string actualDate = selectedDate.GetAttribute("data-ref");

            string expectedDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.Equals(expectedDate, actualDate, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Expected date: {expectedDate}, but searched date is: {actualDate}");
            }

            return true;
        }
    }
}
