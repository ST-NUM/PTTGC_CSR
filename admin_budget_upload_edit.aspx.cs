using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_budget_upload_edit : System.Web.UI.Page
{
    private static int nMenuID = 10;

    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnload(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                string str = Request.QueryString["str"];
                if (!string.IsNullOrEmpty(str))
                {
                    ((_MP_Front)Master).MenuID_Selected = nMenuID;

                    int nID = CommonFunction.GetIntNullToZero(STCrypt.Decrypt(str));
                    hddnID.Value = nID + "";
                    string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                    string sPageType = sPer == "A" ? "แก้ไข" : "ดูรายละเอียด";

                    SystemFunction.BindDdlPageSize(ddlPageSize);
                    hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                    lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);

                    SetControl();
                }
                else
                {
                    Response.Redirect("admin_budget_upload.aspx");
                }
            }
        }
    }

    public void SetControl()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        var lstMasterSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive).ToList();

        #region Objective
        var lstObj = lstMasterSub.Where(w => w.nMainID == 8).ToList();
        ddlObjective.DataSource = lstObj;
        ddlObjective.DataValueField = "nSubID";
        ddlObjective.DataTextField = "sName";
        ddlObjective.DataBind();
        ddlObjective.Items.Insert(0, new ListItem("- Objective  -", ""));
        #endregion

        #region Area
        var lstArea = lstMasterSub.Where(w => w.nMainID == 6).ToList();
        ddlArea.DataSource = lstArea;
        ddlArea.DataValueField = "nSubID";
        ddlArea.DataTextField = "sName";
        ddlArea.DataBind();
        ddlArea.Items.Insert(0, new ListItem("- Area  -", ""));
        #endregion

        #region Philanthropic Activities
        var lstPA = lstMasterSub.Where(w => w.nMainID == 2).ToList();
        ddlPA.DataSource = lstPA;
        ddlPA.DataValueField = "nSubID";
        ddlPA.DataTextField = "sName";
        ddlPA.DataBind();
        ddlPA.Items.Insert(0, new ListItem("- Philanthropic Activities  -", ""));
        #endregion      
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(int nID)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            var lstBudget = db.TB_Budget_Sub.Where(w => w.nID == nID).OrderByDescending(o => o.dUpdate).ToList();
            var lstProID = lstBudget.Select(s => s.nProjectID).Distinct().ToList();
            var lstPro = db.T_Project.Where(w => lstProID.Contains(w.nProjectID)).ToList();
            var lstGL = db.TB_GLAccount.ToList();
            var lstMasterSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive).ToList();
            var lstObjective = lstMasterSub.Where(w => w.nMainID == 8).ToList();
            var lstArea = lstMasterSub.Where(w => w.nMainID == 6).ToList();
            var lstPA = lstMasterSub.Where(w => w.nMainID == 2).ToList();
            var lstProjectType = lstMasterSub.Where(w => w.nMainID == 7).ToList();

            #region Data
            var lstData = (from a in lstBudget
                           from b in lstGL.Where(w => w.nYear == a.dPostingDate.Year && w.sGLID == a.sGLID && w.sIOID == a.sIOID)
                           from c in lstObjective.Where(w => w.nSubID == a.nObjective).DefaultIfEmpty()
                           from d in lstArea.Where(w => w.nSubID == a.nArea)
                           from e in lstPA.Where(w => w.nSubID == a.nPA)
                           from f in lstPro.Where(w => w.nProjectID == a.nProjectID)
                           from g in lstProjectType.Where(w => w.nSubID == f.nProjectType)
                           orderby f.sProjectName ascending
                           select new c_budget
                           {
                               nItem = a.nItem,
                               nProjectID = a.nProjectID,
                               sProjectName = f.sProjectName,
                               nPeriod = a.nPeriod,
                               sIOID = a.sIOID,
                               sPostingDate = a.dPostingDate.ToString("dd/MM/yyyy"),
                               sDescription = a.sDescription,
                               nValInRepCur = a.nValInRepCur,
                               sNameOffsetting = a.sNameOffsetting,
                               sGLID = a.sGLID,
                               sGLName = b.sGLName,
                               nObjective = a.nObjective,
                               sObjective = c != null ? c.sName : "-",
                               nArea = a.nArea,
                               sArea = d.sName,
                               sInternal = a.sInternal,
                               sExternal = a.sExternal,
                               nPA = a.nPA,
                               sPA = e.sName,
                               nProjectType = f.nProjectType ?? 0,
                               sProjectType = g.sName
                           }).ToList();
            #endregion

            result.lstProject = db.T_Project.Where(w => !w.IsDel && w.IsPassApprove).Select(s => new c_project { nProjectID = s.nProjectID, sProjectName = s.sProjectName }).ToList();
            result.lstOrder = db.TB_InternalOrder.OrderBy(o => o.sCostCenterID).ThenBy(o => o.sIOID).ToList();
            result.lstGL = lstGL;

            result.lstData = lstData;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(c_budgetSave itemSave)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            #region Add/Update TB_Budget_Sub
            var nUserThis = UserAccount.SessionInfo.nUserID;

            var qBudget = db.TB_Budget_Sub.FirstOrDefault(w => w.nID == itemSave.nID && w.nItem == itemSave.nItem);
            if (qBudget != null)
            {
                qBudget.sDescription = itemSave.sDescription;
                qBudget.sInternal = itemSave.sInternal;
                qBudget.sExternal = itemSave.sExternal;
                qBudget.nObjective = itemSave.nObjective;
                qBudget.nArea = itemSave.nArea;
                qBudget.nPA = itemSave.nPA;
                qBudget.nUpdateBy = nUserThis;
                qBudget.dUpdate = DateTime.Now;
                db.SaveChanges();
            }
            #endregion
        }
        return result;
    }

    #region Class 
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_budget> lstData { get; set; }
        public IEnumerable<c_project> lstProject { get; set; }
        public IEnumerable<TB_InternalOrder> lstOrder { get; set; }
        public IEnumerable<TB_GLAccount> lstGL { get; set; }
    }

    [Serializable]
    public class c_budget
    {
        public int nItem { get; set; }
        public int nProjectID { get; set; }
        public string sProjectName { get; set; }
        public int nPeriod { get; set; }
        public string sIOID { get; set; }
        public string sPostingDate { get; set; }
        public string sDescription { get; set; }
        public decimal nValInRepCur { get; set; }
        public string sNameOffsetting { get; set; }
        public string sGLID { get; set; }
        public string sGLName { get; set; }
        public int? nObjective { get; set; }
        public string sObjective { get; set; }
        public int nArea { get; set; }
        public string sArea { get; set; }
        public string sInternal { get; set; }
        public string sExternal { get; set; }
        public int nPA { get; set; }
        public string sPA { get; set; }
        public int nProjectType { get; set; }
        public string sProjectType { get; set; }
    }

    [Serializable]
    public class c_budgetSave
    {
        public int nID { get; set; }
        public int nItem { get; set; }
        public string sDescription { get; set; }
        public int nObjective { get; set; }
        public int nArea { get; set; }
        public string sInternal { get; set; }
        public string sExternal { get; set; }
        public int nPA { get; set; }
    }

    [Serializable]
    public class c_project
    {
        public int nProjectID { get; set; }
        public string sProjectName { get; set; }
    }
    #endregion
}