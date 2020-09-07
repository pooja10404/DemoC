using OpenQA.Selenium;
using System;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using NUnitTestProject1.Common;

namespace NUnitTestProject1.PageObject
{
    class ReportsPage
    {
        private IWebDriver driver;
        WebDriverWait wait;
        private String filePath;
        private FileOperation fileOperation = new FileOperation();
        private String filterByYearLocator = "//section[@id='filter']//a[text()='year']";
        private String numberOfResultsFound = "div.collection-group.collection-group--custom  h2  span.sub-title__inner";
        private String missingImage = "img.js-lazyload.error";
        private String missingImageReportName = "//img[@class='js-lazyload error'][1]/../../../../../following-sibling::div//h4";
        private String resultRows = "div.collection-group.collection-group--custom.js-scroll div.report__image-wrapper:nth-child(1)";
        private String filterByPlatformLocator = "ul.collection__topic-filters__list li:nth-child(index) a";
        private String missingLink = "//a[contains(text(),'Find out more >') and @href!='']";
        private String missingLinkReportName = "//a[contains(text(),'Find out more >') and @href='']/../../../following-sibling::div//h4";

        public ReportsPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            filePath = AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"src"+Path.DirectorySeparatorChar+"OutputData"+Path.DirectorySeparatorChar+"Output.txt";

        }

        /*
         * Click on Select filter by year.
         */
        public void selectFilter(String year)
        {
            String filterByYearLocatorFinal = filterByYearLocator.Replace("year",year);
            IWebElement filterByYear = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(filterByYearLocatorFinal)));
            filterByYear.Click();
        }
        
        /*
         * Scroll the page to load more reports.
         */
         public void scrollToBottomToGetMoreResults()
        {
           int startcount=0; 
            int reportCount = getTotalReportCount();
            int numberOfScrolls = (reportCount/12)+1;
            for (int i=1;i<=numberOfScrolls;i++){
            IReadOnlyCollection<IWebElement> allRows =driver.FindElements(By.CssSelector(resultRows));
            List<IWebElement> allrowList = new List<IWebElement>(allRows);
                 for (int j=startcount;j<i*12 && j<reportCount;j++,startcount++){
                    var elem = allrowList[j];
               IJavaScriptExecutor ij =  (IJavaScriptExecutor)driver;
               ij.ExecuteScript("arguments[0].scrollIntoView(true);", elem);
            }
            Thread.Sleep(1000);
            startcount++;
            }
        }

        /*
         * it returns total report count for any filters.
         */
        public int getTotalReportCount(){
            IWebElement reportCountText = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(numberOfResultsFound)));
            String text = reportCountText.Text;
            string[] reportCountArray = text.Split(" ");
            int reportcount = int.Parse(reportCountArray[0]);
            return reportcount;
        }

        /*
         * it prints missing image count.
         * and the title of missing image.
         */
        public void printMissingImageCount(){
            IReadOnlyCollection<IWebElement> missingImages = driver.FindElements(By.CssSelector(missingImage));
            fileOperation.writeInFile("Number of missing images are: " +missingImages.Count,filePath);
            IReadOnlyCollection<IWebElement> missingImageReportTitles = driver.FindElements(By.XPath(missingImageReportName));
            fileOperation.writeInFile("Missing Images for: ",filePath);

            int count=1;
            foreach( IWebElement missingImageReportTitle in missingImageReportTitles )
             {
                fileOperation.writeInFile(count + " : " + missingImageReportTitle.Text,filePath);
                count++;
             }
        }

        /*
         * it prints the missing image link count.
         */
        public void printMissingLinkCount(){
            int totalReportCount = getTotalReportCount();
            IReadOnlyCollection<IWebElement> missingLinks = driver.FindElements(By.XPath(missingLink));
            fileOperation.writeInFile("Number of missing Link are: " +(totalReportCount - missingLinks.Count),filePath);
        }

        /*
         * it prints the total report count.
         */
        public void printTotalReportCount(){	
            fileOperation.writeInFile("Total number of reports are : "+getTotalReportCount(),filePath);
        }

        /*
         * it selects all filters by platform one by one and print the total report count for each.
         */
        public void selectFiltersByPlatformAndPrintCount(){

            for (int i=1;i<=20;i++){
                IWebElement filterByPlatformElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(filterByPlatformLocator.Replace("index",i+""))));
                 fileOperation.writeInFile("Report Count for filer : "+filterByPlatformElement.Text,filePath);
                 filterByPlatformElement.Click();
                 Thread.Sleep(1000);
                printTotalReportCount();
            }
        }
    }
}
