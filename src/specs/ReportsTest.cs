using NUnit.Framework;
using NUnitTestProject1.PageObject;
using NUnitTestProject1.dataProviders;
using System;
using System.IO;
using NUnitTestProject1.Common;

namespace NUnitTestProject1
{
    [TestFixture]
    public class ReportsTest:BaseTest
    {
        private String filePath = AppDomain.CurrentDomain.BaseDirectory + ".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+".."+Path.DirectorySeparatorChar+"src"+Path.DirectorySeparatorChar+"OutputData"+Path.DirectorySeparatorChar+"Output.txt";
        private FileOperation fileOperation = new FileOperation();
        private ReportsPage reportsPage;
        private TopNavBarPage topNavBarPage;
       Properties properties;

        [Test]
        public void Capture_report_name_from_report_option_top_menu_bar()
        {
            fileOperation.writeInFile("--------------------- TestCase1 --------------------------",filePath);
            topNavBarPage = new TopNavBarPage(GetDriver());
            topNavBarPage.hoverOnTopNavBar("Reports");
            topNavBarPage.printTitlesOfAvailableReports();
            fileOperation.writeInFile("--------------------- -------- --------------------------",filePath);

        }

        [Test]
        public void Filter_by_platform_for_year_2015()
        {
            fileOperation.writeInFile("--------------------- TestCase2 --------------------------",filePath);
            reportsPage = new ReportsPage(GetDriver());
            reportsPage.selectFilter("2015");
            reportsPage.selectFiltersByPlatformAndPrintCount();
            fileOperation.writeInFile("--------------------- -------- --------------------------",filePath);
        }

        [Test, TestCaseSource(typeof(FilterDataProvider),"filterByYearTestData")]
        public void check_missing_image_link_and_number_of_reports_available(int year)
        {
            fileOperation.writeInFile("--------------------- TestCase3 --------------------------",filePath);
            reportsPage = new ReportsPage(GetDriver());
            fileOperation.writeInFile("For Year "+year,filePath);
            reportsPage.selectFilter(year+"");
            reportsPage.printTotalReportCount();
            reportsPage.scrollToBottomToGetMoreResults();
            reportsPage.printMissingImageCount();
            reportsPage.printMissingLinkCount();
            fileOperation.writeInFile("--------------------- -------- --------------------------",filePath);

        }
    }
    }