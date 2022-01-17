using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Web.UI;
/// <summary>
/// Summary description for SOAPLog
/// </summary>
public class SOAPLog
{


    public SOAPLog()
    {
        //
        // TODO: Add constructor logic here
        //

    }
    public void SaveSOAPToTextFile(Object req_input)
    {
        //TODO 
        string errorText = "";///Paths File
        string strPathName = "./UploadFiles/PILog/" + DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("th-TH")) + "/";
        XmlSerializer xmlSerializer = new XmlSerializer(req_input.GetType());
        string _xml = "";
        using (StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, req_input);
            _xml = textWriter.ToString();
        }
        #region Create Directory (เช็ค และสร้างไดเร็คเตอรี)
        if (!Directory.Exists(HttpContext.Current.Server.MapPath(strPathName)))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPathName));
        }
        #endregion
        string strFileName = DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("th-TH")) + ".txt";
        string logMessage = String.Format(@"[{0}] :: {1} ", DateTime.Now.ToString("HH:mm:ss", new System.Globalization.CultureInfo("th-TH")) + " ", _xml);
        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(strPathName + strFileName));
        StreamWriter _sw = file.AppendText();
        _sw.Write(logMessage);
        _sw.Write(_sw.NewLine);
        _sw.WriteLine(("*").PadLeft(logMessage.Length, '*'));
        _sw.Write(_sw.NewLine);
        _sw.Close();
    }
    public void SaveSOAPToTextFile(Object req_input, string SubPath)
    {
        //TODO 
        string errorText = "";///Paths File
        string strPathName = "./UploadFiles/PILog/" + DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("th-TH")) + "/" + (SubPath != "" ? SubPath + "/" : "");
        if (req_input != null)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(req_input.GetType());
            string _xml = "";
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, req_input);
                _xml = textWriter.ToString();
            }
            #region Create Directory (เช็ค และสร้างไดเร็คเตอรี)
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(strPathName)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPathName));
            }
            #endregion
            string strFileName = DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("th-TH")) + ".txt";
            string logMessage = String.Format(@"[{0}] :: {1} ", DateTime.Now.ToString("HH:mm:ss", new System.Globalization.CultureInfo("th-TH")) + " ", _xml);
            FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(strPathName + strFileName));
            StreamWriter _sw = file.AppendText();
            _sw.Write(logMessage);
            _sw.Write(_sw.NewLine);
            _sw.WriteLine(("*").PadLeft(logMessage.Length, '*'));
            _sw.Write(_sw.NewLine);
            _sw.Close();
        }
    }
}