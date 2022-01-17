using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Prevent unusable PostBack on IE11
        {
            string sUA = Request.UserAgent;
            int nVersion = Request.Browser.MajorVersion;
            if (sUA.Contains("like Gecko") && sUA.Contains("Trident") && nVersion == 0) Page.ClientTarget = "uplevel";
        }
        #endregion

        #region Prevent death-loop WebMethod when securiy scanning
        {
            var lstLink = Request.RawUrl.Split('/').ToList();
            var arrUrl = Request.Url;
            var sLink = lstLink.FirstOrDefault(w => w.Contains(".aspx"));

            if (sLink != null)
            {
                int nSubPath = lstLink.IndexOf(sLink);
                if (nSubPath < lstLink.Count - 1)
                {
                    var lstLinkUse = new List<string>();
                    for (int i = 0; i < nSubPath + 1; i++) lstLinkUse.Add(lstLink[i]);

                    if (lstLink.Count > nSubPath)
                    {
                        string sUrl = arrUrl.OriginalString.Replace(arrUrl.PathAndQuery, "") + "" + string.Join("/", lstLinkUse);
                        Response.Redirect(sUrl);
                    }
                }
            }
        }
        #endregion

        if (!IsPostBack)
        {
            GetAcitveDirectory();
        }
    }

    private void GetAcitveDirectory()
    {
        string sUrl = "~/login.aspx";
        string sLink = Request.QueryString["link"];
        if (!string.IsNullOrEmpty(sLink))
        {
            sLink = CommonFunction.Encrypt_UrlEncrypt(STCrypt.Decrypt(sLink));
        }

        string[] userindname = HttpContext.Current.User.Identity.Name.Split('\\');//new string[] { "", "26007098" };
        if (userindname.Length >= 2)
        {
            string strID = userindname[1] + "";
            sUrl = "~/login.aspx?strad=" + Server.UrlEncode(STCrypt.Encrypt(strID)) + "&&smod=" + Server.UrlEncode(STCrypt.Encrypt("AD")) + (!string.IsNullOrEmpty(sLink) ? "&link=" + sLink : "");
        }
        else
        {
            sUrl = "~/login.aspx" + (!string.IsNullOrEmpty(sLink) ? "?link=" + sLink : "");
        }

        Response.Redirect(sUrl);
    }
}