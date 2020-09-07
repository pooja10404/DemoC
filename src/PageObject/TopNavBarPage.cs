using OpenQA.Selenium;
using System;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnitTestProject1.Common;


namespace NUnitTestProject1.PageObject
{
    class TopNavBarPage
    {
        private IWebDriver driver;
        WebDriverWait wait;
         private String filePath;
        private FileOperation fileOperation = new FileOperation();
        private String reportTabLocator = "//header[@data-gtm-context='navigation']//a[text()='Reports']";
        private String reportTitle = "article[class='tout tout--default tout--report'] h3";
       
        public TopNavBarPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            filePath = AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"src"+Path.DirectorySeparatorChar+"OutputData"+Path.DirectorySeparatorChar+"Output.txt";
        }

        /*
         * Hover on report on the top Navbar.
         */
        public void hoverOnTopNavBar(String option)
        {
            
            IWebElement reportTab = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(reportTabLocator.Replace("Reports",option))));
            Actions action  = new Actions(driver);
            action.MoveToElement(reportTab).Perform();
        }

        /*
         * Print the title of available report on the file.
         */
         public void printTitlesOfAvailableReports()
        {
            int count =1;
            IReadOnlyCollection<IWebElement> allRows =driver.FindElements(By.CssSelector(reportTitle));
             foreach( IWebElement reportTitle in allRows )
             {
                fileOperation.writeInFile(count + " : " + reportTitle.Text,filePath);
                count++;
             }
        }
    }
}
