using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace RyanairTests
{
    internal class RyanairMainPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private const string Url = "https://www.ryanair.com/";

        public RyanairMainPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        private By AcceptCookiesBtn => By.CssSelector("button[data-ref='cookie.accept-all']");

        private By OneWayBtn => By.CssSelector("label[for='ry-radio-button--0']");

        private By DepartureInput => By.CssSelector("#input-button__departure");

        private By AirportSuggestions => By.CssSelector("fsw-airport-item span[data-ref='airport-item__name']");

        private By ArrivalInput => By.CssSelector("#input-button__destination");

        private By DepartureDateInput => By.CssSelector("div[data-ref='input-button__display-value']");

        private By AvailableDates => By.CssSelector("calendar-body .calendar-body__cell[data-type='day'][tabindex='0']");


        private By SearchBtn => By.CssSelector("button[aria-label='Szukaj']");
        private By AirportSuggestion => By.CssSelector("span[role='option']");


        public void AcceptCookiesIfVisible()
        {
            try
            {
                var btn = wait.Until(d => d.FindElement(AcceptCookiesBtn));
                btn.Click();
            }
            catch {  }
        }

        public void SelectOneWay()
        {
            wait.Until(d => d.FindElement(OneWayBtn)).Click();
        }

        public void SetDeparture(string departure, string airportName)
        {
            var input = wait.Until(d => d.FindElement(DepartureInput));
            input.Click();
            input.Clear();
            input.SendKeys(departure);

            SelectAirport(airportName);
        }

        public void SelectAirport(string airportName)
        {
            var options = wait.Until(d => d.FindElements(AirportSuggestions));

            var option = options.FirstOrDefault(o => o.Text.Contains(airportName, StringComparison.OrdinalIgnoreCase));
            if (option != null)
            {
                option.Click();
            }
            else
            {
                throw new Exception($"Lotnisko '{airportName}' nie znalezione w sugestiach.");
            }
        }


        public void SetArrival(string arrival, string airportName)
        {
            var input = wait.Until(d => d.FindElement(ArrivalInput));
            input.Click();
            input.Clear();
            input.SendKeys(arrival);

            var options = wait.Until(d => d.FindElements(AirportSuggestions));
            var option = options.FirstOrDefault(o => o.Text.Contains(airportName, StringComparison.OrdinalIgnoreCase));
            if (option != null)
            {
                option.Click();
            }
            else
            {
                throw new Exception($"Lotnisko '{airportName}' nie znalezione w sugestiach.");
            }
        }


        public void SelectFirstAvailableDate()
        {
            var dateInput = wait.Until(d => d.FindElement(DepartureDateInput));
            dateInput.Click();

            IWebElement firstAvailable = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    var allCells = wait.Until(d => d.FindElements(By.CssSelector("calendar-body .calendar-body__cell")));

                    firstAvailable = allCells
                        .FirstOrDefault(c => !c.GetAttribute("class").Contains("--disabled") && c.Displayed && c.Enabled);

                    if (firstAvailable != null)
                    {
                        firstAvailable.Click();
                        return;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(200);
                }
            }

            throw new Exception("No available date in calendar.");
        }


        public void Search()
        {
            driver.FindElement(SearchBtn).Click();
        }

        public void Open()
        {
            driver.Navigate().GoToUrl(Url);
        }
    }
}