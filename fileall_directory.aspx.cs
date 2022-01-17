using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fileall_directory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    private void ReadFile()
    {
        List<TData> lstData = new List<TData>();

        //string sPathServer = "EPIServiceXML/";
        //string sPathFile = "24-05-2560/";
        string sPathServer = txtPath.Text + "/";
        if (Directory.Exists(HttpContext.Current.Server.MapPath("./") + sPathServer.Replace("/", "\\")))
        {
            DirectoryInfo d = new DirectoryInfo(Server.MapPath("./") + sPathServer.Replace("/", "\\"));

            FileInfo[] Files = d.GetFiles("*");
            foreach (FileInfo file in Files)
            {
                DateTime dTimeFile = file.CreationTime;
                string filename = file.Name;
                lstData.Add(new TData { FileName = LinkOpenFile(sPathServer, filename, filename), sDate = dTimeFile.ToString("dd/MM/yyyy HH:mm:ss"), dFile = dTimeFile });
            }
            gvw.DataSource = lstData.OrderByDescending(o => o.dFile).ToList();
            gvw.DataBind();
        }
        else
        {
            gvw.DataSource = lstData.OrderByDescending(o => o.dFile).ToList();
            gvw.DataBind();
        }
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        ReadFile();
    }

    private string LinkOpenFile(string pathFile, string _Filename, string NameFileShow)
    {
        string strEncrypt = "";
        string sReturn = "";
        #region แสดงไฟล์
        strEncrypt = pathFile + _Filename;
        sReturn = "<a href=" + strEncrypt + " target=_blank style=color:#666;text-decoration:none>" + NameFileShow + "<img border='0' height='16' src='images/ic_search.gif' width='16'/></a>";
        return sReturn;
        #endregion
    }

    [Serializable]
    public class TData
    {
        public string FileName { get; set; }
        public string sDate { get; set; }
        public DateTime dFile { get; set; }
    }
}