using System;
using System.Collections.Generic;
using System.Configuration;

namespace HotelSearch.org.com.utilities
{
    public class ConfigManager
    {
        Dictionary<string, string> hash = new Dictionary<string, string>();

        public void setProperty(String key, String value)
        {
            this.hash[key]= value;
            //this.hash.Add(key, value);
        }

        public String getProperty(String key)
        {
            String returnValue = null;
            if (hash.ContainsKey(key))
            {
                hash.TryGetValue(key, out returnValue);
            }
            return returnValue;

        }
    }
}