using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;


namespace HotelSearch.org.com.utilities
{
    public class ThreadCache
    {
        private static ThreadCache instance = new ThreadCache();
        ThreadLocal<ConfigManager> TC = new ThreadLocal<ConfigManager>();

        public static ThreadCache getInstance()
        {
            return instance;
        }

        public void setConfigManager(ConfigManager cm)
        {
            this.TC.Value = cm;
        }

        public ConfigManager getConfigManager()
        {
            return TC.Value;

        }
    }
}
