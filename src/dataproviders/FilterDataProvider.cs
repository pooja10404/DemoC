using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NUnitTestProject1.dataProviders
{
    class FilterDataProvider
    {
        private static IEnumerable<TestCaseData> filterByYearTestData()
        {
            yield return new TestCaseData(2017).
                SetName("Missing links/Images and count of reports in year 2017");
            yield return new TestCaseData(2016).
                SetName("Missing links/Images and count of reports in year 2016");
            yield return new TestCaseData(2015).
                SetName("Missing links/Images and count of reports in year 2015");
        }
    }
}