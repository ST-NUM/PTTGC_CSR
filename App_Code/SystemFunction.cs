using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Globalization;
//using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Reflection;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
/// <summary>
/// Summary description for SystemFunction
/// </summary>
public class SystemFunction
{
    static string conn = WebConfigurationManager.ConnectionStrings["PTTGC_CSR_ConnectionString"].ConnectionString.ToString();
    //public static string sConnectionString_CSR = ConfigurationManager.ConnectionStrings["PTTGC_CSR_ConnectionString"].ConnectionString;

    public static string sSessionName_UserInfo = "SS_UserInfo";
    public static string process_SessionExpired = "SSEXP";
    public static string sPageRedirectSessionExpired = "Login.aspx";
    public static string sHomePage = "f_Home.aspx";

    public static string process_Success = "Success";
    public static string process_Failed = "Failed";
    public static string process_FileOversize = "OverSize";
    public static string process_FileInvalidType = "InvalidType";
    public static string process_Duplicate = "DUP";
    public static string process_SaveFail = "SaveFail";
    public static string sMsgUsernamePasswordWrong = "ระบุ ชื่อผู้ใช้ หรือ รหัสผ่าน ไม่ถูกต้อง";
    public static string sMsgDontPRMSLogin = "คุณไม่ได้รับอนุญาตให้เข้าสู่ระบบ โปรดติดต่อผู้ดูแลระบบของคุณ";
    public static string sMsgSaveInNotStep = "ไม่สามารถทำรายการได้";

    public static bool IsADMode = (ConfigurationManager.AppSettings["IsADMode"] + "") == "Y";
    public static bool IsWebServiceMode = (ConfigurationManager.AppSettings["WebServiceMode"] + "") == "Y";
    public static string sPasswordBypass = ConfigurationManager.AppSettings["PasswordBypass"] + "";
    public static string sOrgCSR = ConfigurationManager.AppSettings["sOrgID"] + "";
    public static string sCostCenterCSR = ConfigurationManager.AppSettings["sCCID"] + "";
    public static string sPasswordDefault = ConfigurationManager.AppSettings["PasswordDefault"] + "";

    public SystemFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetPasswordBypass()
    {
        return ConfigurationSettings.AppSettings["PasswordBypass"].ToString();
    }

    public string ParseObjectToJson(object ob)
    {
        try
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
            string res = serializer.Serialize(ob);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

            return res;
        }
        catch
        {
            return "";
        }
    }

    public T JsonDeserialize<T>(string jsonString)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }

    public static List<ClassExecute.ST_CCTR> GetCCTR(string sCCTRCode, string sCompanyCode)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        List<ClassExecute.ST_CCTR> lstCCTR = new List<ClassExecute.ST_CCTR>();
        string sql = @"SELECT [CostCenter] AS sCCTRCode ,GeneralName AS sCCTRShortName ,FullDesc AS sCCTRFullName , ComCode AS sCompanyCode ,[CostCenter] AS sValue,[CostCenter]+' - '+ GeneralName  AS sText
                       FROM [MT_CostCenter]
                       WHERE CostCenter = '" + CommonFunction.ReplaceInjection(sCCTRCode) + "' AND ComCode = '" + CommonFunction.ReplaceInjection(sCompanyCode) + @"' AND CONVERT(DATE,[ValidFrom]) <= CONVERT(DATE,GETDATE()) AND [ValidTo] >= CONVERT(DATE,GETDATE())";
        lstCCTR = db.Database.SqlQuery<ClassExecute.ST_CCTR>(sql).ToList();
        return lstCCTR;
    }

    public static List<ClassExecute.ST_CCTR> GetCCTR_Under(string sCCTRShortName, string sCompanyCode)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        List<ClassExecute.ST_CCTR> lstCCTR = new List<ClassExecute.ST_CCTR>();
        string sql = @"SELECT [CostCenter] AS sCCTRCode ,GeneralName AS sCCTRShortName ,FullDesc AS sCCTRFullName , ComCode AS sCompanyCode ,[CostCenter] AS sValue,[CostCenter]+' - '+ GeneralName  AS sText
                       FROM [MT_CostCenter]
                       WHERE (GeneralName ='" + CommonFunction.ReplaceInjection(sCCTRShortName) + @"'  OR GeneralName LIKE '" + CommonFunction.ReplaceInjection(sCCTRShortName) + @"%') AND ComCode = '" + CommonFunction.ReplaceInjection(sCompanyCode) + @"' AND CONVERT(DATE,[ValidFrom]) <= CONVERT(DATE,GETDATE()) AND [ValidTo] >= CONVERT(DATE,GETDATE())";
        lstCCTR = db.Database.SqlQuery<ClassExecute.ST_CCTR>(sql).ToList();
        return lstCCTR;
    }

    public static List<ClassExecute.ST_CCTR> GetCCTR_InLine(string sCCTRNo, string sCompanyCode)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        List<ClassExecute.ST_CCTR> lstCCTR = new List<ClassExecute.ST_CCTR>();
        string sql_H1 = @"SELECT  H1_APPROVAL
                                FROM MT_CCTR_Structure 
                                WHERE ISNULL(H1_APPROVAL,'') != '' AND PERFORM_FLAG = 'C' AND [ID] = '" + sCCTRNo + @"'";
        //        string sql_H1 = @"SELECT  H1_APPROVAL
        //                        FROM MT_CCTR_Structure 
        //                        WHERE ISNULL(H1_APPROVAL,'') != '' AND PERFORM_FLAG = 'A' AND [ID] = '" + sCCTRNo + @"A'";
        string H1_APPROVAL = CommonFunction.Get_Value(conn, sql_H1);
        if (!string.IsNullOrEmpty(H1_APPROVAL))
        {
            // AND SUBSTRING(REPLACE(cctr_str.[ID],'CC_',''),1,2) ='" + sCompanyCode + @"'
            //AND cctr.ComCode = SUBSTRING(REPLACE(cctr_str.[ID],'CC_',''),1,2)
            string sCCTR_H1_APPROVAL = H1_APPROVAL.Substring(0, 8);
            string sql_inline_H1 = @"SELECT cctr.[CostCenter] AS sCCTRCode ,cctr.GeneralName AS sCCTRShortName ,cctr.FullDesc AS sCCTRFullName , cctr.ComCode AS sCompanyCode ,cctr.[CostCenter] AS sValue,cctr.[CostCenter]+' - '+ cctr.GeneralName  AS sText
                                    FROM [MT_CCTR_Structure] cctr_str
                                    INNER JOIN [MT_CostCenter] cctr ON REPLACE(REPLACE(cctr_str.[ID],'CC_',''),'A','') = cctr.CostCenter 
                                    WHERE  cctr_str.H1_APPROVAL = '" + H1_APPROVAL + @"' "
                                                                     + (sCCTRNo == sCCTR_H1_APPROVAL ? "" : @" AND cctr_str.ID not LIKE '%" + sCCTR_H1_APPROVAL + @"%' ") +
                                    @" AND cctr_str.ID LIKE '" + CommonFunction.ReplaceInjection(sCompanyCode) + @"%'
                                    GROUP BY cctr.[CostCenter],cctr.GeneralName,cctr.FullDesc, cctr.ComCode ";
            lstCCTR = db.Database.SqlQuery<ClassExecute.ST_CCTR>(sql_inline_H1).ToList();
        }
        return lstCCTR;
    }

    public static List<ClassExecute.ST_CCTR> GetCCTR_ByComCode(string sCompanyCode)
    {
        //Where เพิ่ม เอาเฉพาะ CSR
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        List<ClassExecute.ST_CCTR> lstCCTR = new List<ClassExecute.ST_CCTR>();
        string sql = @"SELECT c.[CostCenter] AS sCCTRCode ,c.[GeneralName] AS sCCTRShortName ,c.FullDesc AS sCCTRFullName , c.ComCode AS sCompanyCode ,c.[CostCenter] AS sValue,cc.Description  AS sText
                               FROM [MT_CostCenter] c
                               INNER JOIN MT_CCTR_Structure cc ON c.CostCenter = cc.ID AND cc.PERFORM_FLAG ='C'
                               WHERE ComCode = '" + CommonFunction.ReplaceInjection(sCompanyCode) + @"' AND CONVERT(DATE,[ValidFrom]) <= CONVERT(DATE,GETDATE()) AND [ValidTo] >= CONVERT(DATE,GETDATE())";
        lstCCTR = db.Database.SqlQuery<ClassExecute.ST_CCTR>(sql).ToList();
        return lstCCTR;
    }

    public static List<ClassExecute.ST_CCTR> GetCCTRAll()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        List<ClassExecute.ST_CCTR> lstCCTR = new List<ClassExecute.ST_CCTR>();
        string sql = @"SELECT c.[CostCenter] AS sCCTRCode ,c.[GeneralName] AS sCCTRShortName ,c.FullDesc AS sCCTRFullName , c.ComCode AS sCompanyCode ,c.[CostCenter] AS sValue,cc.Description  AS sText
                               FROM [MT_CostCenter] c
                               INNER JOIN MT_CCTR_Structure cc ON c.CostCenter = cc.ID AND cc.PERFORM_FLAG ='C'
                               WHERE CONVERT(DATE,[ValidFrom]) <= CONVERT(DATE,GETDATE()) AND [ValidTo] >= CONVERT(DATE,GETDATE())";

        lstCCTR = db.Database.SqlQuery<ClassExecute.ST_CCTR>(sql).ToList();

        return lstCCTR;
    }

    public static string GetMenuName(int nMenuID, bool IsEdit, string sPer)
    {
        string sRet = "";
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qMenu = db.TM_Menu.FirstOrDefault(w => w.nMenuID == nMenuID);
        if (qMenu != null)
        {
            string sIconR = " <i class='fa fa-chevron-right'></i> ";

            sRet = "<i class='" + qMenu.sIcon + "'></i> ";
            if (qMenu.nMenuHead.HasValue)
            {
                var qMenuHead = db.TM_Menu.FirstOrDefault(w => w.nMenuID == (qMenu.nMenuHead ?? 0));
                if (qMenuHead != null)
                {
                    sRet += qMenuHead.sMenuName + sIconR;
                }
            }

            string sMenuName = IsEdit ? ("<a href='" + qMenu.sMenuLink + "' class='aMenu'>" + qMenu.sMenuName + "</a>") : qMenu.sMenuName;
            sRet += sMenuName + (sPer != "" ? sIconR + sPer : "");
        }

        return sRet;
    }

    public static void BindDdlPageSize(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 10, 30, 50, 100 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static void BindDdlPageSize20(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 20, 30, 50, 100 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static void BindDdlPageSizeSmall(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 5, 10, 20 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static string GetPMS(int nMenuID)
    {
        string result = "N";

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            int nUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0;
            var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && !w.IsDel && w.IsActive);
            if (qUser != null)
            {
                var qPer = db.TB_User_Permission.FirstOrDefault(w => w.nUserID == nUserID && w.nMenuID == nMenuID);
                if (qPer != null)
                {
                    result = (qPer.nPermission == 2 ? "A" : (qPer.nPermission == 1 ? "V" : "N"));
                }
            }
        }

        return result;
    }

    public static string GetSysCon()
    {
        string sCon = WebConfigurationManager.ConnectionStrings["PTTGC_CSR_ConnectionString"].ConnectionString.ToString();
        string sConBE = WebConfigurationManager.ConnectionStrings["ConnectionString_BE"].ConnectionString.ToString();

        string smtp = WebConfigurationManager.AppSettings["smtpmail"].ToString();

        string SAP_SYNCIN_USR = WebConfigurationManager.AppSettings["SAP_SYNCIN_USR"].ToString();
        string SAP_SYNCIN_PWD = WebConfigurationManager.AppSettings["SAP_SYNCIN_PWD"].ToString();

        return "Con : " + sCon + Environment.NewLine +
               "Con Be : " + sConBE + Environment.NewLine +
               "SMTP : " + smtp + Environment.NewLine +
               "SAP_SYNCIN_USR : " + SAP_SYNCIN_USR + Environment.NewLine +
               "SAP_SYNCIN_PWD : " + SAP_SYNCIN_PWD + Environment.NewLine;
    }

    #region File
    public static void CheckPathAndMoveFile(string sysFileName, string FileName, string sUploadPath, string sUploadPath_Temp)
    {
        HttpContext context = HttpContext.Current;
        string sMapPath = context.Server.MapPath("./");
        string sPathSave = sMapPath + sUploadPath;
        if (!Directory.Exists(sPathSave))
        {
            Directory.CreateDirectory(sPathSave);
        }
        if (File.Exists(sMapPath + sUploadPath_Temp + sysFileName))
        {
            string currentPath = context.Server.MapPath("./" + sUploadPath_Temp);
            File.Move(currentPath + "/" + sysFileName, sPathSave + "/" + sysFileName);
        }
    }

    public static void RemoveFile(string sPathAndFileName)
    {
        HttpContext context = HttpContext.Current;
        string sMapPath = context.Server.MapPath("./");
        if (File.Exists(sPathAndFileName))
        {
            File.Delete(sPathAndFileName);
        }
    }

    public static void DeleteAllFile(string _pathFile)
    {
        if (Directory.Exists(_pathFile.Replace("/", "\\")))
        {
            DirectoryInfo di = new DirectoryInfo(_pathFile.Replace("/", "\\"));
            FileInfo[] fi = di.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in fi)
            {
                try
                {
                    f.Delete();
                }
                finally
                {

                }
            }
        }
    }

    public static void RemoveFileAllInFolfer(string sPath)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(sPath);
        if (di.Exists)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
    #endregion
}