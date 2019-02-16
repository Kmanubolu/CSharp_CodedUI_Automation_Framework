using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSearch.org.com.utilities;

namespace HotelSearch.org.com.screen
{
    public class LogOut
    {

        public static String sheetname = "LogOut";
        Common common = CommonManager.getInstance().GetCommon();

        public Boolean SCRLogOut()
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
