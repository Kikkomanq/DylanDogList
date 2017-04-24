
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Automation
{
    public class Test
    {
        IWebDriver driver = new ChromeDriver();


        public void goToPage()
        {

            driver.Navigate().GoToUrl("https://www.kupindo.com/");
            driver.Manage().Window.Maximize();

        }
        public void searchForDylanDog()
        {

            driver.FindElement(By.Id("txtPretraga")).SendKeys("dylan dog");
            driver.FindElement(By.Id("search_button")).Click();
        }

        public void sortByPrice()
        {

            //Wait for elements to load
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));


            //Sort by price Ascending 

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='container_right']/div[1]/div/select/option[2]")));
            driver.FindElement(By.XPath("//*[@id='container_right']/div[1]/div/select/option[2]")).Click();


            //For test to Fail -> Sort by price Desccending 

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='container_right']/div[1]/div/select/option[3]")));
            //driver.FindElement(By.XPath("//*[@id='container_right']/div[1]/div/select/option[3]")).Click();

        }

        public int[] getPrices()
        {


            //Waits for page to load
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30)); wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));


            IList<IWebElement> prices = driver.FindElements(By.CssSelector("span.item_price"));


            int[] intPrices = new int[prices.Count];



            for (int i = 0; i < prices.Count; i++)

            {

                //Removes (.) if prices are bigger then 999 -> Sort by price Desccending
                string pricesWithoutDot = getDiscountPrices(prices[i]).Replace(".", "");

                //Removes "din" and blank spaces
                string[] pricesWithoutString = pricesWithoutDot.Split(new string[] { "din", "     " }, StringSplitOptions.RemoveEmptyEntries);


                for (int j = 0; j < pricesWithoutString.Length; j++)

                {

                    intPrices[i] = Convert.ToInt32(pricesWithoutString[j]);

                    Console.WriteLine("List of prices " + pricesWithoutString[j]);
                }
            }

            return intPrices;

        }

        static string getDiscountPrices(IWebElement parent)
        {
            string text = parent.Text;

            foreach (IWebElement e in parent.FindElements(By.CssSelector("*")))
            {
                text = text.Replace(e.Text, "");
            }

            return text.Trim();
        }

        public bool IsSorted(int[] prices)
        {

            int lastPrice = 0;

            for (int i = 0; i < prices.Length; i++)

            {

                if (prices[i] < lastPrice)
                    return false;

                lastPrice = prices[i];
            }

            return true;
        }


        public static void Main()
        {


            Test test = new Test();
            test.goToPage();
            test.searchForDylanDog();
            test.sortByPrice();
            int[] prices = test.getPrices();
            bool isSorted = test.IsSorted(prices);

            if (isSorted == true)

                Console.WriteLine(Environment.NewLine + "List is sorted by Acsending Order!" + Environment.NewLine);
            else

                Console.WriteLine(Environment.NewLine + "List is not sorted out by Ascending Order!!" + Environment.NewLine);

        }

    }

}
