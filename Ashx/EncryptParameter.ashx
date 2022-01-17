<%@ WebHandler Language="C#" Class="EncryptParameter" %>

using System;
using System.Web;

public class EncryptParameter : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string str = context.Request.QueryString["str"];

        str = CommonFunction.EncryptParameter(str);
            
        context.Response.Expires = -1;
        context.Response.ContentType = "text/plain";
        context.Response.Write(str);
        context.Response.End();
    }

    public bool IsReusable { get { return false; } }
}