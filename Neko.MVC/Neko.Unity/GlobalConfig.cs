using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Unity
{
    public static class GlobalConfig
    {
        public static Dictionary<string,object> CacheCookies { get; set; }

        static GlobalConfig()
        {
            CacheCookies = new Dictionary<string, object>();
        }
    }
}
