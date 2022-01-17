using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_masterdata_edit : System.Web.UI.Page
{
    private static int nMenuID = 12;

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
                string sID = hddID.Value = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";

                SetControl();

                string sPageType = "เพิ่ม";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (sID != "")
                {
                    SetData(sID);
                    sPageType = sPer == "A" ? "แก้ไข" : "ดูรายละเอียด";
                }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
            }
        }
    }

    public void SetControl()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        var lstType = db.TM_MasterData.Where(w => w.IsManage).ToList();
        ddlType.DataSource = lstType;
        ddlType.DataValueField = "nMainID";
        ddlType.DataTextField = "sName";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("- ประเภท  -", ""));
    }

    public void SetData(string sID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qSub = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID + "" == sID && !w.IsDel);
        if (qSub != null)
        {
            ddlType.Enabled = false;
            ddlType.SelectedValue = qSub.nMainID + "";
            txtName.Text = qSub.sName;
            txtDescription.Text = qSub.sDescription;
            rdlActive.SelectedValue = qSub.IsActive ? "1" : "0";
        }
        else
        {
            Response.Redirect("admin_masterdata.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(int nSubID, int nType, string sName, string sDesciption, bool IsActive)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            #region Add/Update TM_MasterData_Sub
            var nUserThis = UserAccount.SessionInfo.nUserID;

            var IsNew = nSubID == 0;
            var qDup = db.TM_MasterData_Sub.FirstOrDefault(w => w.nMainID == nType && w.sName == sName && (!IsNew ? w.nSubID != nSubID : true) && !w.IsDel);
            if (qDup == null)
            {
                var qSub = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == nSubID);
                if (nSubID > 0 && qSub.IsDel)
                {
                    result.Msg = SystemFunction.sMsgSaveInNotStep;
                    result.Status = SystemFunction.process_SaveFail;
                    return result;
                }

                if (IsNew)
                {
                    qSub = new TM_MasterData_Sub();
                    var qMax = db.TM_MasterData_Sub.OrderByDescending(o => o.nSubID).FirstOrDefault();
                    qSub.nSubID = qMax != null ? qMax.nSubID + 1 : 1;
                    qSub.nMainID = nType;
                    qSub.nCreateBy = nUserThis;
                    qSub.dCreate = DateTime.Now;
                    db.TM_MasterData_Sub.Add(qSub);
                }

                qSub.sName = sName;
                qSub.sDescription = sDesciption;
                qSub.IsActive = IsActive;
                qSub.IsDel = false;
                qSub.nUpdateBy = nUserThis;
                qSub.dUpdate = DateTime.Now;
                db.SaveChanges();
            }
            else
            {
                result.Status = SystemFunction.process_Duplicate;
            }
            #endregion
        }
        return result;
    }
}