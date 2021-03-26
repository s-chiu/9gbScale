using System;
using System.Collections.ObjectModel;
using System.Configuration;
using OpenQA.Selenium;

namespace _9gbScale
{
    class TestPage
    {
        static WebDriver _browser;
        public TestPage(WebDriver browser)
        {
            _browser = browser;
        }

        public void GoToURL()
        {
            _browser.Driver.Url=ConfigurationManager.AppSettings["testURL"];
        }

        public void ClickReset()
        {
            _browser.ClickElement(_browser.GetElement("//button[text()='Reset']"));
        }

        public void ClickWeigh()
        {
            _browser.ClickElement(_browser.GetElement("//button[text()='Weigh']"));
        }

        public void SetBowls(string side, int[] bars)
        {
            int count = 0;
            foreach(int num in bars)
            {
                _browser.SetElement(_browser.GetElement("//input[@id='" + side + "_" + count + "']"), num.ToString());
                count++;
            }

        }

        public string GetResult()
        {
            return _browser.GetElement("//*[@class='result'] /button").Text;
        }

        public void GetWeighings()
        {
            int count = 1;
            ReadOnlyCollection<IWebElement> weighList = _browser.Driver.FindElements(By.XPath("//*[@class='game-info'] /ol/li"));
            foreach(IWebElement iteration in weighList)
            {
                Console.WriteLine(count + " - " + iteration.Text);
                count++;

            }
        }
        public void ClickCoin(string num)
        {
            _browser.ClickElement(_browser.GetElement("//*[@id='coin_" + num + "']"));
        }

        public void GetAlertMessage()
        {
            Console.WriteLine("Printing Alert Message");
            Console.WriteLine(_browser.Driver.SwitchTo().Alert().Text);
        }

        public void CloseAlertMessage()
        {
            _browser.Driver.SwitchTo().Alert().Accept();
        }
    }
}
