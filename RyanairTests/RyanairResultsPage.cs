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

        // Lokator na wszystkie karty lotów
        private By FlightCards => By.CssSelector("flight-card-new[data-ref='flight-card_all_information']");

        // Zwraca ile lotów znalazło się na liście
        public int GetFlightsCount()
        {
            var flights = wait.Until(d => d.FindElements(FlightCards));
            return flights.Count;
        }

        // Sprawdza, czy są jakiekolwiek loty
        public bool HasFlights()
        {
            return GetFlightsCount() > 0;
        }

        // Wybiera pierwszy dostępny lot
        public void SelectFirstFlight()
        {
            var flights = wait.Until(d => d.FindElements(FlightCards));
            flights.First().Click();
        }

        public bool AssertTodayIsSelectedDate()
        {
            // znajdź element z zaznaczoną datą
            var selectedDate = wait.Until(d =>
                d.FindElement(By.CssSelector("button[data-e2e='date-item'][data-selected='true']"))
            );

            // pobierz wartość atrybutu data-ref (format YYYY-MM-DD)
            string actualDate = selectedDate.GetAttribute("data-ref");

            // wylicz dzisiejszą datę w tym samym formacie
            string expectedDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.Equals(expectedDate, actualDate, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Oczekiwana data: {expectedDate}, ale zaznaczona jest: {actualDate}");
            }

            return true;
        }
    }
}
