using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;

namespace _9gbScale
{
    class WebDriver
    {
        private IWebDriver _currentWebDriver;
        private WebDriverWait _wait;
        private string browserConfig => ConfigurationManager.AppSettings["browserConfig"];
        public WebDriverWait Wait => _wait ?? (_wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WaitTime)));
        public int WaitTime { get; set; }
        private IJavaScriptExecutor javaScriptE;
        public IJavaScriptExecutor js => javaScriptE ?? (javaScriptE = (IJavaScriptExecutor)_currentWebDriver);
        public WebDriver()
        {
            WaitTime = 30;
        }

        public IWebDriver Driver
        {
            get
            {
                if (_currentWebDriver != null)
                    return _currentWebDriver;

                switch (browserConfig)
                {
                    case "Chrome":
                        ChromeOptions options = new ChromeOptions();
                        //options.AddArgument("headless");                                                                                  
                        options.AddArgument("window-size=1280,960");
                        options.AddArgument("no-sandbox");
                        _currentWebDriver = new ChromeDriver(options);
                        break;
                    //case "IE":
                    //    _currentWebDriver = new InternetExplorerDriver(new InternetExplorerOptions() { IgnoreZoomLevel = true });
                    //    _currentWebDriver.Manage().Window.Maximize();
                    //    break;
                    //case "Firefox":
                    //    _currentWebDriver = new FirefoxDriver();
                    //    _currentWebDriver.Manage().Window.Maximize();
                    //    break;
                    //case "Edge":
                    //    _currentWebDriver = new EdgeDriver();
                    //    _currentWebDriver.Manage().Window.Maximize();
                    //    break;
                    default:
                        throw new NotSupportedException($"{browserConfig} is not a supported browser");
                }
                _currentWebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                return _currentWebDriver;
            }
        }

        public IWebElement GetElement(string query)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(query)));
        }

        public void ClickElement(IWebElement element)
        {

             js.ExecuteScript("arguments[0].click()", element);
        }

        public void SetElement(IWebElement element,string value)
        {

            //js.ExecuteScript("arguments[0].value=arguments[1]", element, value);
            element.SendKeys(value);
        }
    }
}
