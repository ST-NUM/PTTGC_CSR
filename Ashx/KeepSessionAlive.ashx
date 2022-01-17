<%@ WebHandler Language="C#" Class="KeepSessionAlive" %>

using System;
using System.Web;

public class KeepSessionAlive : IHttpHandler, System.Web.SessionState.IRequiresSessionState{
    
    public void ProcessRequest (HttpContext context) {
        /*context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                context.Response.Cache.SetNoStore();
                context.Response.Cache.SetNoServerCaching();*/

        //context.Response.AddHeader("Refresh", Convert.ToString(((context.Session.Timeout * 60) - 10)));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}