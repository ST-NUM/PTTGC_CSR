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

public partial class admin_permission_edit : System.Web.UI.Page
{
    private static int nMenuID = 11;

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
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                string str = Request.QueryString["str"];
                string sUserID = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";

                SetControl();

                string sPageType = "เพิ่ม";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (sUserID != "")
                {
                    SetData(sUserID);
                    sPageType = sPer == "A" ? "แก้ไข" : "ดูรายละเอียด";
                }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
            }
        }
    }

    public void SetControl()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        var lstRole = db.TM_MasterData_Sub.Where(w => w.nMainID == 1 && w.IsActive && !w.IsDel).ToList();
        ddlRole.DataSource = lstRole;
        ddlRole.DataValueField = "nSubID";
        ddlRole.DataTextField = "sName";
        ddlRole.DataBind();
        ddlRole.Items.Insert(0, new ListItem("- กลุ่มผู้ใช้งาน  -", ""));

        txtPassword.Attributes.Add("type", "password");
    }

    public void SetData(string sUserID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID + "" == sUserID && !w.IsDel);
        if (qUser != null)
        {
            rdlUserType.SelectedValue = qUser.IsGC ? "1" : "0";
            hddnUserID.Value = sUserID;

            if (qUser.IsGC)
            {
                txtEmpName.Enabled = false;
                txtEmpName.Text = qUser.sUserID + " - " + qUser.sFirstname + "  " + qUser.sLastname;
                txtEmpID.Text = qUser.sUserID;
                txtFName.Text = qUser.sFirstname;
                txtLName.Text = qUser.sLastname;

                var lstOrgCSR = db.TM_Organization.Select(s => s.sOrgID).ToList();
                hddCSR.Value = qUser.nRole == 2 || qUser.nRole == 3 ? "Y" : (HR_WebService.CheckEmployeeInOrg(qUser.sUserID, string.Join(",", lstOrgCSR)) ? "Y" : "N");
            }
            else
            {
                txtName.Text = qUser.sFirstname;
                txtSurname.Text = qUser.sLastname;
                txtUsername.Text = qUser.sUserID;
                txtPassword.Text = STCrypt.Decrypt(qUser.sPasswordEncrypt);
            }

            txtEmail.Text = qUser.sEmail;
            ddlRole.SelectedValue = qUser.nRole + "";
            rdlActive.SelectedValue = qUser.IsActive ? "1" : "0";

            //ddlRole.Enabled = false;
            txtEmail.Enabled = !qUser.IsGC;
        }
        else
        {
            Response.Redirect("admin_permission.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_Return GetMenu(int? nUserID)
    {
        c_Return result = new c_Return();

        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            var lstMenu = db.TM_Menu.Where(w => w.IsActive).ToList();
            var lstPermission = db.TB_User_Permission.Where(w => w.nUserID == nUserID).ToList();

            var lstTM_Menu = db.TM_Menu.Where(w => w.IsActive).OrderBy(o => o.nMenuOrder).ToList();
            var lstMenuLV1 = lstTM_Menu.Where(w => w.nLevel == 1).ToList();

            var lstData = new List<c_Menu>();

            foreach (var item in lstMenuLV1)
            {
                var lstTM_MenuSub = lstTM_Menu.Where(w => w.nMenuHead == item.nMenuID).ToList();
                if (lstTM_MenuSub.Any())
                {
                    lstData.Add(new c_Menu() { nMenuID = item.nMenuID, sMenuName = item.sMenuName, nPermission = 0, nLevel = (item.nLevel ?? 0), IsHead = true });

                    foreach (var item1 in lstTM_MenuSub)
                    {
                        var qPer = lstPermission.FirstOrDefault(w => w.nMenuID == item1.nMenuID);
                        int? nPer = qPer != null ? qPer.nPermission : null;
                        lstData.Add(new c_Menu() { nMenuID = item1.nMenuID, sMenuName = item1.sMenuName, nPermission = nPer, nLevel = (item1.nLevel ?? 0), IsHead = false });
                    }
                }
                else
                {
                    var qPer = lstPermission.FirstOrDefault(w => w.nMenuID == item.nMenuID);
                    int? nPer = qPer != null ? qPer.nPermission : null;
                    lstData.Add(new c_Menu() { nMenuID = item.nMenuID, sMenuName = item.sMenuName, nPermission = nPer, nLevel = (item.nLevel ?? 0), IsHead = false });
                }
            }

            result.lstData = lstData;
        }

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnEmp GetEmp(string sSearch, bool IsCSR)
    {
        c_ReturnEmp TReturn = new c_ReturnEmp();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            var lstEmpNotCode = new List<string>();//db.Database.SqlQuery<TB_User>("select * from TB_User where IsActive = 1 and IsDel = 0").Select(s => s.sUserID).ToList();
            var sOrg = IsCSR ? string.Join(",", db.TM_Organization.Select(w => w.sOrgID).ToList()) : "";
            TReturn.lstData = HR_WebService.EmployeeService_Search(sSearch, sOrg, lstEmpNotCode);
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ResetPassword(int nUserID)
    {
        sysGlobalClass.CResutlWebMethod TReturn = new sysGlobalClass.CResutlWebMethod();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && !w.IsGC && !w.IsDel);
            if (qUser != null)
            {
                string sPasswordDefault = SystemFunction.sPasswordDefault;
                qUser.sPassword = STCrypt.encryptMD5(sPasswordDefault);
                qUser.sPasswordEncrypt = STCrypt.Encrypt(sPasswordDefault);
                db.SaveChanges();

                string _to = qUser.sEmail;

                string subject = "PTTGC-CSR | รหัสผ่านของคุณถูกรีเซ็ต";
                string message = string.Format(CSR_Function.GET_TemplateEmail(),
                        "เรียน คุณ " + CSR_Function.GetFirstNameNotAbbr(qUser.sFirstname) + ' ' + qUser.sLastname,
                        "รหัสผ่านใหม่ของคุณคือ " + sPasswordDefault,
                        "",
                        "",
                        "",
                        "");

                CSR_Function.SendNetMail("", _to, "", subject, message, new List<string>());
            }
            else
            {
                TReturn.Msg = SystemFunction.sMsgSaveInNotStep;
                TReturn.Status = SystemFunction.process_SaveFail;
            }
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(int nUserID, string sUserID, string sPassword, string sFirstname, string sLastname, string sEmail, bool IsGC, int nRole, List<c_Permission> lstPermission, bool IsActive)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var IsNew = nUserID == 0;
            var qDup = db.TB_User.FirstOrDefault(w => (!IsNew ? w.nUserID != nUserID : true) && w.sUserID == sUserID && !w.IsDel);
            if (qDup == null)
            {
                #region Add/Update TB_User
                var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID);
                if (!IsNew && qUser.IsDel)
                {
                    result.Msg = SystemFunction.sMsgSaveInNotStep;
                    result.Status = SystemFunction.process_SaveFail;
                    return result;
                }

                var nUserThis = UserAccount.SessionInfo.nUserID;

                if (IsNew)
                {
                    qUser = new TB_User();
                    qUser.nCreateBy = nUserThis;
                    qUser.dCreate = DateTime.Now;
                    db.TB_User.Add(qUser);
                }

                qUser.sUserID = sUserID;
                qUser.sPassword = !IsGC ? STCrypt.encryptMD5(sPassword.Trim()) : null;
                qUser.sPasswordEncrypt = !IsGC ? STCrypt.Encrypt(sPassword) : null;
                qUser.sFirstname = sFirstname;
                qUser.sLastname = sLastname;
                qUser.sEmail = sEmail;
                qUser.IsGC = IsGC;
                qUser.nRole = nRole;
                qUser.IsActive = IsActive;
                qUser.IsDel = false;
                qUser.nUpdateBy = nUserThis;
                qUser.dUpdate = DateTime.Now;
                db.SaveChanges();
                #endregion

                #region Delete/Add
                if (!IsNew) CommonFunction.ExecuteNonQuery("delete TB_User_Permission where nUserID = '" + nUserID + "'");

                foreach (var item in lstPermission)
                {
                    db.TB_User_Permission.Add(new TB_User_Permission() { nUserID = qUser.nUserID, nMenuID = item.nMenuID, nPermission = item.nPermission });
                }

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

    #region Class
    [Serializable]
    public class c_Return : sysGlobalClass.CResutlWebMethod
    {
        public List<c_Menu> lstData { get; set; }
    }

    [Serializable]
    public class c_Menu
    {
        public int nMenuID { get; set; }
        public string sMenuName { get; set; }
        public int? nPermission { get; set; }
        public int nLevel { get; set; }
        public bool IsHead { get; set; }
    }

    [Serializable]
    public class c_ReturnEmp : sysGlobalClass.CResutlWebMethod
    {
        public HR_WebService.ObjectData lstData { get; set; }
    }

    [Serializable]
    public class c_User
    {
        public string sUserID { get; set; }
        public string sName { get; set; }
    }

    [Serializable]
    public class c_Permission
    {
        public int nMenuID { get; set; }
        public int? nPermission { get; set; }
    }
    #endregion
}