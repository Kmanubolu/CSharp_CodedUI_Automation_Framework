using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSearch.org.com.elements
{
    public class Elements
    {
        Dictionary<string, string> hash = new Dictionary<string, string>();

        public Elements()
        {

            //Login Page
            hash.Add("edtUserName", "Id:username");
            hash.Add("edtPassword", "Id:password");
            hash.Add("btnLogin", "Id:login");

            //Search Form
            hash.Add("lstLocation", "Id:location");
            hash.Add("lstHotels", "Id:hotels");
            hash.Add("lstRoomType", "Id:room_type");
            hash.Add("lstNoOfRooms", "Id:room_nos");
            hash.Add("edtDatePickIn", "Id:datepick_in");
            hash.Add("edtDatePickOut", "Id:datepick_out");
            hash.Add("lstAdult_Rom", "Id:adult_room");
            hash.Add("lstChildRoom", "Id:child_room");
            hash.Add("eleSubmit", "Id:Submit");

            //LogOut Page
            hash.Add("btnLogOut", "XPath:/html/body/table[2]/tbody/tr[1]/td[2]/a[4]");

        }
        public String getObject(string element)
        {
            String returnValue = null;
            if (hash.ContainsKey(element))
            {
                hash.TryGetValue(element, out returnValue);
            }
            return returnValue;
        }
    }
}
