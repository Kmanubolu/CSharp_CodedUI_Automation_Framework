using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace HotelSearch.org.com.utilities
{
    class ManagerDriver
    {
        private static ManagerDriver instance = new ManagerDriver();
        ThreadLocal<BrowserWindow> driver = new ThreadLocal<BrowserWindow>();

        public static ManagerDriver getInstance()
        {
            return instance;
        }


        public void SetDriver(BrowserWindow d)
        {
           this.driver.Value = d;
        }

        public BrowserWindow GetDriver()
        {
            return driver.Value;
        }
       
    }
}
