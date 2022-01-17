using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class budget_transaction : System.Web.UI.Page
{
    private static int nMenuID = 3;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;
                SetControl();
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                SystemFunction.BindDdlPageSize(ddlPageSize);
                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
            }
        }
    }

    public void SetControl()
    {
        #region Year
        int nYearStart = 2019;
        int nYearNow = DateTime.Now.Year;
        int nIndex = 0;
        for (int nYear = nYearStart; nYear <= nYearNow; nYear++)
        {
            ddlYear.Items.Insert(nIndex, new ListItem(nYear + "", nYear + ""));
            nIndex++;
        }
        ddlYear.SelectedValue = nYearNow + "";
        #endregion

        #region Project Type
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
        ddlProjectType.DataSource = lstProjectType;
        ddlProjectType.DataValueField = "nSubID";
        ddlProjectType.DataTextField = "sName";
        ddlProjectType.DataBind();
        ddlProjectType.Items.Insert(0, new ListItem("- ประเภท  -", ""));
        #endregion
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sProjectName, string sProjectType, string sYear)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            sProjectName = sProjectName.ToLower().Replace(" ", "");

            var lstProject = db.T_Project.Where(w => !w.IsDel &&
            (!string.IsNullOrEmpty(sProjectName) ? (w.sProjectCode.ToLower().Replace(" ", "").Contains(sProjectName) || w.sProjectName.ToLower().Replace(" ", "").Contains(sProjectName)) : true) &&
            (!string.IsNullOrEmpty(sProjectType) ? w.nProjectType + "" == sProjectType : true) &&
            (!string.IsNullOrEmpty(sYear) ? w.nYear + "" == sYear : true)).ToList();
            var lstProID = lstProject.Select(s => s.nProjectID).Distinct().ToList();
            var lstBudget = db.TB_Budget_Sub.Where(w => lstProID.Contains(w.nProjectID)).ToList();

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
                           from f in lstProject.Where(w => w.nProjectID == a.nProjectID)
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

            result.lstData = lstData;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }

        return result;
    }

    #region Class 
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_budget> lstData { get; set; }
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
    #endregion
}