using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using SignalR;

namespace Website
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapConnection<MyConnection>("url2img", "url2img/{*operation}");
        }
    }
}