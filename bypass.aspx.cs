using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassExecute;

using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class bypass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserAccount.Logout();
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod Login(string sUserName, string sPassword)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        result.Msg = SystemFunction.sMsgUsernamePasswordWrong;

        sUserName = sUserName.Trim().Replace(" ", "");

        var Pwd = SystemFunction.GetPasswordBypass();

        var qUser = db.TB_User.FirstOrDefault(w => w.sUserID == sUserName && w.IsActive && !w.IsDel);
        if (qUser != null && sPassword == Pwd)
        {
            UserAccount ua = new UserAccount();
            ua.nUserID = qUser.nUserID;
            ua.sName = qUser.sFirstname + "  " + qUser.sLastname;

            int nRole = qUser.nRole;
            ua.nRole = nRole;

            var qRole = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == nRole);
            ua.sRoleName = qRole != null ? qRole.sName : "";

            UserAccount.SetObjUser(ua);

            CSR_Function.UpdateLog(0, "Login Bypass", "Login Bypass " + ua.nUserID);

            result.Msg = "";
        }

        return result;
    }
}