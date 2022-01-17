<%@ WebHandler Language="C#" Class="Fileuploader" %>

using System;
using System.Web;

public class Fileuploader : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string Function = context.Request["funcname"] + "";
        string savetopath = context.Request["savetopath"] + "";
        string savetoname = context.Request["savetoname"] + "";
        string delonpath = context.Request["delpath"] + "";
        string delfilename = context.Request["delfilename"] + "";
        string userId = context.Request["userId"] + "";

        if (Function == "UPLOAD")
        {
            FileUpload data = new FileUpload();
            if (context.Request.Files.Count > 0)
            {
                HttpPostedFile file = null;
                string filepath = savetopath;
                string sFileName = "";
                string sSysFileName = savetoname;
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    file = context.Request.Files[i];
                    if (file.ContentLength >= 0)
                    {
                        sFileName = file.FileName;
                        string[] arrfilename = (sFileName + "").Split('.');

                        string sFilenameOld = arrfilename[0];
                        bool IsOver = sFilenameOld.Length > 229;
                        sSysFileName = (IsOver ? sFilenameOld.Substring(0, 229) : sFilenameOld) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + "." + arrfilename[arrfilename.Length - 1];

                        if (!System.IO.Directory.Exists(context.Server.MapPath("./") + filepath.Replace("/", "\\")))
                        {
                            System.IO.Directory.CreateDirectory(context.Server.MapPath("./") + filepath.Replace("/", "\\"));
                        }

                        file.SaveAs(context.Server.MapPath(filepath + sSysFileName));

                        data.ID = 0;
                        data.IsCompleted = true;
                        data.SaveToFileName = sSysFileName;
                        data.FileName = sFileName;
                        data.SaveToPath = filepath;
                        data.url = filepath + sSysFileName;
                        data.IsNewFile = true;
                        data.sUpdate = DateTime.Now.ToString("dd/MM/yyyy HH:mm น.");
                    }
                }
            }
            context.Response.Expires = -1;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
            string res = serializer.Serialize(data);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

            context.Response.Write(res);
            context.Response.End();
        }
        else if (Function == "DEL")
        {
            if (System.IO.File.Exists(context.Server.MapPath("./") + delonpath.Replace("/", "\\") + delfilename))
            {
                System.IO.File.Delete(context.Server.MapPath("./") + delonpath.Replace("/", "\\") + delfilename);
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Structure for jquery
    /// </summary>
    public class DataFile
    {
        public string name { get; set; }
        /// <summary>
        /// unit Kb
        /// </summary>
        public decimal? size { get; set; }
        /// <summary>
        /// file type
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// for open file case not custom
        /// </summary>
        public string file { get; set; }
        /// <summary>
        /// for custom
        /// </summary>
        public FileUpload data { get; set; }
    }

    public class FileUpload
    {
        public int ID { get; set; }
        public string SaveToFileName { get; set; }
        public string FileName { get; set; }
        public string SaveToPath { get; set; }
        public string url { get; set; }
        public bool IsNewFile { get; set; }
        public bool IsCompleted { get; set; }
        public string sUpdate { get; set; }
    }
}