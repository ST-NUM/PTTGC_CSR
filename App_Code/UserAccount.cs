using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserAccount
/// </summary>
public class UserAccount
{
    public int nUserID { get; set; }
    public string sName { get; set; }
    public int nRole { get; set; }
    public string sRoleName { get; set; }

    private static string sSessionName = "UserAccount";

    private static UserAccount SS_UserAccount
    {
        get
        {
            var ssUA = HttpContext.Current.Session[sSessionName];
            return ssUA is UserAccount && ssUA != null ? (UserAccount)ssUA : null;
        }
        set { HttpContext.Current.Session[sSessionName] = value; }
    }

    public static void SetObjUser(UserAccount ua) { SS_UserAccount = ua; }

    public static sysGlobalClass.CResutlWebMethod Login(string sUserName, string sPassword, string sMode)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        if (!string.IsNullOrEmpty(sUserName) && (string.IsNullOrEmpty(sMode) ? !string.IsNullOrEmpty(sPassword) : true))
        {
            string sPassEncypt = STCrypt.encryptMD5(sPassword);
            var qUser = db.TB_User.FirstOrDefault(w => w.sUserID == sUserName && (string.IsNullOrEmpty(sMode) ? w.sPassword == sPassEncypt : true) && !w.IsDel && w.IsActive);
            if (qUser != null)
            {
                UserAccount ua = new UserAccount();
                ua.nUserID = qUser.nUserID;
                ua.sName = qUser.sFirstname + "  " + qUser.sLastname;

                int nRole = qUser.nRole;
                ua.nRole = nRole;

                var qRole = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == nRole);
                ua.sRoleName = qRole != null ? qRole.sName : "";

                SetObjUser(ua);

                result.Status = SystemFunction.process_Success;
            }
            else
            {
                if (!string.IsNullOrEmpty(sMode))
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "ชื่อผู้ใช้ไม่ได้ลงทะเบียน";
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = SystemFunction.sMsgUsernamePasswordWrong; ;
                }
            }
        }

        return result;
    }

    public static void Logout() { SS_UserAccount = null; }

    public static UserAccount SessionInfo { get { return SS_UserAccount; } }

    public static bool IsExpired { get { return SS_UserAccount == null; } }
}