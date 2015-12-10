using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public class AppConfig
    {
        public static string Token = System.Configuration.ConfigurationManager.AppSettings["Token"];
        public static string AppId = System.Configuration.ConfigurationManager.AppSettings["appID"];
        public static string AppSecret = System.Configuration.ConfigurationManager.AppSettings["appsecret"];
    }
}