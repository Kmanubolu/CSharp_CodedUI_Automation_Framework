using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSearch.org.com.utilities;

namespace HotelSearch.org.com.screen
{
    public class LogIn
    {

        public static String sheetname = "LogIn";
        Common common = CommonManager.getInstance().GetCommon();

        public Boolean SCRLogIn()
        {
            Boolean status = true;
            status = common.RunComponent(sheetname, Common.o);
            if (!status)
            {
                status = false;
            }
            return status;
        }

    }
}
