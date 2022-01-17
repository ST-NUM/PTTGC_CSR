using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ClassExecute;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserAccount.Logout();

        string str = Request.QueryString["strad"];
        string sm = Request.QueryString["smod"];
        if (!string.IsNullOrEmpty(sm) && !string.IsNullOrEmpty(str))
        {
            hddUserAD.Value = txtUsername.Text = STCrypt.Decrypt(str);

        }

        string sPath = Request.QueryString["link"];
        if (!string.IsNullOrEmpty(sPath))
        {
            hddPath.Value = STCrypt.Decrypt(sPath);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod Login(string sUserName, string sPassword, string sMode)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        sUserName = sUserName.Trim().Replace(" ", "");

        var resultLogin = UserAccount.Login(sUserName, sPassword, sMode);
        result.Msg = resultLogin.Msg + "";
        result.Status = resultLogin.Status;

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ForgetPassword(string sEmail, string sUserName)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();

        sEmail = (sEmail + "").Trim().ToLower();
        sUserName = (sUserName + "").Trim().ToLower();

        var qUser = db.TB_User.FirstOrDefault(w => w.IsActive && !w.IsDel && w.sUserID.ToLower() == sUserName && w.sEmail.ToLower() == sEmail);
        if (qUser != null)
        {
            string _to = qUser.sEmail;

            string subject = "PTTGC-CSR | แจ้งรหัสผ่าน";
            string message = string.Format(CSR_Function.GET_TemplateEmail(),
                    "เรียน คุณ" + CSR_Function.GetFirstNameNotAbbr(qUser.sFirstname) + ' ' + qUser.sLastname,
                    "รหัสผ่านของคุณคือ " + STCrypt.Decrypt(qUser.sPasswordEncrypt),
                    "",
                    "",
                    "",
                    "");

            CSR_Function.SendNetMail("", _to, "", subject, message, new List<string>());

            result.Content = "ระบบทำการส่งอีเมล์ถึง " + sEmail + " แล้ว";
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_Failed;
            result.Msg = "ไม่พบข้อมูล";
        }
        return result;
    }
}