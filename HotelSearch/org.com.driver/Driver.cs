using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using System.Threading;
using System.Configuration;
using HotelSearch.org.com.utilities;

namespace HotelSearch.org.com.driver
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class Driver
    {
        private BrowserWindow driver;
        private String browser;
        private String executionType;

        public static Queue<string> testCaseIDorGroup = new Queue<string>();
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Boolean status = false;
            HTML.fnSummaryInitialization("Execution Summary Report");
            status = XlsxReader.getInstance().addTestCasesFromDataSheetName(testCaseIDorGroup);
        }

        [TestMethod]
        public void Driver1()
        {
            Boolean isExitLoop = false;
            String threadId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            Console.WriteLine(threadId);

            int x = Convert.ToInt32(ConfigurationManager.AppSettings["ShortSyncTime"]);
            for (int i = 1; i < x; i++)
            {
                if (testCaseIDorGroup.Count > 0)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(2000);
                }
            }

            String strTCID = null;
            try
            {
                strTCID = testCaseIDorGroup.Dequeue();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isExitLoop = true;
            }
            if (strTCID == null)
            {
                isExitLoop = true;
            }

            while (!isExitLoop)
            {
                browser = ConfigurationManager.AppSettings["Browsers"];
                driver = LocalDriverFactory.getInstance().CreateNewDriver(browser);

                ManagerDriver.getInstance().SetDriver(driver);
                Common common = new Common();
                CommonManager.getInstance().SetCommon(common);

                ConfigManager cm = new ConfigManager();
                ThreadCache.getInstance().setConfigManager(cm);

                common.RunTestCase(strTCID);

                try
                {
                    strTCID = testCaseIDorGroup.Dequeue();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    isExitLoop = true;
                    testCaseIDorGroup.Enqueue("Done");
                }
                if (strTCID == null || strTCID == "Done")
                {
                    isExitLoop = true;
                }

            }
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            HTML.fnSummaryCloseHtml(ConfigurationManager.AppSettings["Release"]);
        }
    }
}

