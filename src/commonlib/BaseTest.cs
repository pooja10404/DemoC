using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnitTestProject1.PageObject;
using NUnitTestProject1.dataProviders;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using NUnitTestProject1.Common;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.IO;
using System.Collections.Generic;

namespace NUnitTestProject1.Common
{
    [SetUpFixture]
    public abstract class BaseTest
    {

      protected ExtentReports _extent;
      protected ExtentTest _test;
      public IWebDriver _driver;
      Properties properties;

      [OneTimeSetUp]
      protected void Setup()
      {
        String reportPath = @AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Reports"+Path.DirectorySeparatorChar+"ExtentReport.html";
        var htmlReporter = new ExtentHtmlReporter(reportPath);
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);
        _extent.AddSystemInfo("Host Name", "Weforum");
        _extent.AddSystemInfo("Environment", "QA");
        _extent.AddSystemInfo("UserName", "Pooja");
      }

      [OneTimeTearDown]
      protected void TearDown()
      {
        _extent.Flush();
      }

      [SetUp]
      public void BeforeTest()
      {
        String service =  @AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"ChromeDriver";
        _driver = new ChromeDriver(service);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        _driver.Manage().Window.Maximize();
        _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
        properties = new  Properties(AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"src"+Path.DirectorySeparatorChar+"InputData"+Path.DirectorySeparatorChar+"input.properties");
        _driver.Navigate().GoToUrl(properties.get("url"));
      }

      public IWebDriver GetDriver()
      {
        return _driver;
      }

      [TearDown]
      public void AfterTest()
      {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)? ""
                        : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
        Status logstatus;
        switch (status)
        {
          case TestStatus.Failed:
            logstatus = Status.Fail;
            DateTime time = DateTime.Now;
            String fileName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            String screenShotPath = Capture(_driver, fileName);
            _test.Log(Status.Fail, "Fail");
            _test.Log(Status.Fail, "Snapshot below: "+ _test.AddScreenCaptureFromPath("Screenshots\\" + fileName));
            break;
          case TestStatus.Inconclusive:
            logstatus = Status.Warning;
            break;
          case TestStatus.Skipped:
            logstatus = Status.Skip;
            break;
          default:
            logstatus = Status.Pass;
            break;
        }
        _test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
        _extent.Flush();
        _driver.Quit();
      }

      public static string Capture(IWebDriver driver, String screenShotName)
      {
        ITakesScreenshot ts = (ITakesScreenshot)driver;
        Screenshot screenshot = ts.GetScreenshot();
        String reportPath = @AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"Reports";
        Directory.CreateDirectory(reportPath);
        screenshot.SaveAsFile(reportPath+Path.DirectorySeparatorChar+"screenShot", ScreenshotImageFormat.Png);
        return reportPath;
      }
    }
}

