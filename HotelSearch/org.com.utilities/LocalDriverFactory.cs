using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotelSearch.org.com.utilities
{
    class LocalDriverFactory
    {
        private static LocalDriverFactory instance = new LocalDriverFactory();

        public static LocalDriverFactory getInstance()
        {
            return instance;
        }

        public BrowserWindow CreateNewDriver(String browser)
        {
            BrowserWindow driver = null;

            if (browser.ToUpper() == "FIREFOX")
            {
                BrowserWindow.CurrentBrowser = "firefox";

            }
            else if (browser.ToUpper() == "CHROME")
            {
                BrowserWindow.CurrentBrowser = "chrome";
            }
            else if (browser.ToUpper() == "IE")
            {
                BrowserWindow.CurrentBrowser = "ie";
            }
            driver = BrowserWindow.Launch(new System.Uri(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["Region"]]));
            driver.Maximized = true;
            return driver;
        }
    }
}
