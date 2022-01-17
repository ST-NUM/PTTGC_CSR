using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_info : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnload(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            if (!UserAccount.IsExpired)
            {
                SetControl();
                SetData(UserAccount.SessionInfo.nUserID);
            }
        }
    }

    public void SetControl()
    {
        txtPassword.Attributes.Add("type", "password");
        txtPassword1.Attributes.Add("type", "password");
    }

    public void SetData(int nUserID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && !w.IsDel && !w.IsGC);
        if (qUser != null)
        {
            txtName.Text = qUser.sFirstname;
            txtSurname.Text = qUser.sLastname;
            txtUsername.Text = qUser.sUserID;
            txtEmail.Text = qUser.sEmail;
            hddnUserID.Value = nUserID + "";
        }
        else
        {
            Response.Redirect("index.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(int nUserID, string sUserID, string sPassword, string sFirstname, string sLastname, string sEmail)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var qDup = db.TB_User.FirstOrDefault(w => w.nUserID != nUserID && w.sUserID == sUserID && !w.IsDel);
            if (qDup == null)
            {
                #region Update TB_User
                var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID);
                if (qUser.IsDel)
                {
                    result.Msg = SystemFunction.sMsgSaveInNotStep;
                    result.Status = SystemFunction.process_SaveFail;
                    return result;
                }

                qUser.sUserID = sUserID;
                qUser.sFirstname = sFirstname;
                qUser.sLastname = sLastname;
                qUser.sEmail = sEmail;

                if (!string.IsNullOrEmpty(sPassword))
                {
                    qUser.sPassword = STCrypt.encryptMD5(sPassword.Trim());
                    qUser.sPasswordEncrypt = STCrypt.Encrypt(sPassword);
                }

                qUser.nUpdateBy = nUserID;
                qUser.dUpdate = DateTime.Now;
                db.SaveChanges();
                #endregion
            }
            else
            {
                result.Status = SystemFunction.process_Duplicate;
            }
        }
        return result;
    }
}