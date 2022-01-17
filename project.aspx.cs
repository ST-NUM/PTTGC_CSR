using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project : System.Web.UI.Page
{
    private static int nMenuID = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;
                SetControl();
                SystemFunction.BindDdlPageSize(ddlPageSize);

                var IsApprover = CSR_Function.IsApprover(null);
                hddISApprover.Value = IsApprover ? "Y" : "N";
                hddPermission.Value = IsApprover ? "V" : SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
            }
        }
    }

    public void SetControl()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        #region Project Type
        var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
        ddlProjectType.DataSource = lstProjectType;
        ddlProjectType.DataValueField = "nSubID";
        ddlProjectType.DataTextField = "sName";
        ddlProjectType.DataBind();
        ddlProjectType.Items.Insert(0, new ListItem("- ประเภท  -", ""));
        #endregion

        #region Dimension
        var lstDimension = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && !w.IsDel && w.IsActive).ToList();
        ddlDimension.DataSource = lstDimension;
        ddlDimension.DataValueField = "nSubID";
        ddlDimension.DataTextField = "sName";
        ddlDimension.DataBind();
        ddlDimension.Items.Insert(0, new ListItem("- Dimension  -", ""));
        #endregion

        #region Status
        var lstStatus = db.TM_ProjectStatus.ToList();
        ddlStatus.DataSource = lstStatus;
        ddlStatus.DataValueField = "nStatusID";
        ddlStatus.DataTextField = "sStatusName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("- สถานะ  -", ""));
        #endregion

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
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sProjectName, string sProjectType, string sDimension, string sYear, string sStatus)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            sProjectName = sProjectName.ToLower().Replace(" ", "");

            int nUserID = UserAccount.SessionInfo.nUserID;
            var IsAdmin = CSR_Function.IsAdmin(nUserID);
            var sUserThis = CSR_Function.GetUserID(nUserID);

            var lstProject = db.T_Project.Where(w => !w.IsDel && (IsAdmin || w.nOwnerID == nUserID || w.nCreateBy == nUserID || (w.nStatusID == 1 ? w.sApproverID == sUserThis : false)) &&
            (!string.IsNullOrEmpty(sProjectName) ? (w.sProjectCode.ToLower().Replace(" ", "").Contains(sProjectName) || w.sProjectName.ToLower().Replace(" ", "").Contains(sProjectName)) : true) &&
            (!string.IsNullOrEmpty(sProjectType) ? w.nProjectType + "" == sProjectType : true) &&
            (!string.IsNullOrEmpty(sDimension) ? w.nDimensionID + "" == sDimension : true) &&
            (!string.IsNullOrEmpty(sYear) ? w.nYear + "" == sYear : true) &&
            (!string.IsNullOrEmpty(sStatus) ? w.nStatusID + "" == sStatus : true)
            ).ToList();
            var lstProID = lstProject.Select(s => s.nProjectID).ToList();
            var lstBudget = db.TB_Budget_Sub.Where(w => lstProID.Contains(w.nProjectID)).ToList();
            var lstGLMaster = db.TB_GLAccount.OrderByDescending(o => o.nYear).ToList();

            var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
            var lstDimension = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && !w.IsDel && w.IsActive).ToList();
            var lstStatus = db.TM_ProjectStatus.ToList();

            string sPer = SystemFunction.GetPMS(nMenuID);
            Func<bool, bool, int, string> GetButtonAction = (IsOwner_, IsApprover_, nStatusID) =>
            {
                var hasPerAll = sPer == "A";
                var sRet = "ดูรายละเอียด";
                if (hasPerAll)
                {
                    switch (nStatusID)
                    {
                        case 0: if (IsOwner_ || IsAdmin) { sRet = "แก้ไข"; } break;//บันทึก
                        case 1: if (IsApprover_) { sRet = "อนุมัติ"; } break;//ส่งอนุมัติ
                        case 2: if (IsOwner_) { sRet = "แก้ไข"; } break;//อนุมัติ
                        case 4: if (IsOwner_ || IsAdmin) { sRet = "แก้ไข"; } break;//ส่งกลับแก้ไข
                        default: sRet = "ดูรายละเอียด"; break;
                    }
                }
                return sRet;
            };

            Func<int, List<c_gl>> GetGL = (nProID) =>
            {
                List<c_gl> lstGL = new List<c_gl>();
                var lstGL_ = lstBudget.Where(w => w.nProjectID == nProID).Select(s => s.sGLID).Distinct().ToList();
                foreach (var item in lstGL_)
                {
                    var qGL = lstGLMaster.FirstOrDefault(w => w.sGLID == item);
                    lstGL.Add(new c_gl() { sGLID = item, sGLName = qGL != null ? qGL.sGLName : "", sBudget = lstBudget.Where(w => w.sGLID == item).Sum(s => s.nValInRepCur).ToString("#,#") });
                }

                return lstGL;
            };

            Func<decimal, decimal, string> GetBudgetStatus = (nBudget, nBudgetUsed) =>
            {
                string sRet = "";
                var nPer = (nBudgetUsed * 100) / nBudget;
                if (nPer < 80) { sRet = "bg-success"; }
                else if (nPer > 100) { sRet = "bg-danger"; }
                else { sRet = "bg-warning"; }

                return "<div class='circle " + sRet + "'></div>";
            };

            var lstData = (from a in lstProject
                           from b in lstDimension.Where(w => w.nSubID == a.nDimensionID).DefaultIfEmpty()
                           from c in lstStatus.Where(w => w.nStatusID == a.nStatusID)
                           from d in lstProjectType.Where(w => w.nSubID == a.nProjectType).DefaultIfEmpty()
                           orderby a.dUpdate descending
                           select new c_Project
                           {
                               nProjectID = a.nProjectID,
                               sProjectCode = a.sProjectCode + "",
                               sProjectName = a.sProjectName,
                               nYear = a.nYear,
                               sType = d != null ? d.sName : "-",
                               sDimension = b != null ? b.sName : "-",
                               nBudget = a.nBudget ?? 0,
                               nBudgetUsed = a.nBudgetUsed ?? 0,
                               sBudget = a.nBudget.HasValue ? a.nBudget.Value.ToString("#,#") : "",
                               sBudgetUsed = a.nBudgetUsed.HasValue ? a.nBudgetUsed.Value.ToString("#,#") : "",
                               sBudgetStatus = a.IsPassApprove ? GetBudgetStatus(a.nBudget.Value, (a.nBudgetUsed ?? 0)) : "",

                               lstGL = a.nBudget.HasValue ? GetGL(a.nProjectID) : new List<c_gl>(),

                               nStatusID = a.nStatusID,
                               sStatus = c.sStatusName,
                               sButtonAction = GetButtonAction(a.nOwnerID == nUserID, a.sApproverID == sUserThis, a.nStatusID),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nProjectID + "")),

                               IsAdmin = IsAdmin,
                               IsOwner = a.nOwnerID == nUserID,
                               IsPassApprove = a.IsPassApprove
                           }).ToList();

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
    public static string Delete(List<int> lstID)
    {
        string sRet = "";
        if (!UserAccount.IsExpired)
        {
            int nUserID = UserAccount.SessionInfo.nUserID;
            if (lstID.Any())
            {
                PTTGC_CSREntities db = new PTTGC_CSREntities();
                var lstData = db.T_Project.Where(w => lstID.Contains(w.nProjectID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                CSR_Function.UpdateLog(nMenuID, "", "Delete ProjectID = " + string.Join(", ", lstID));
            }
        }
        else
        {
            sRet = SystemFunction.process_SessionExpired;
        }
        return sRet;
    }

    #region Class 
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_Project> lstData { get; set; }
    }

    [Serializable]
    public class c_Project
    {
        public int nProjectID { get; set; }
        public string sProjectCode { get; set; }
        public string sProjectName { get; set; }
        public int nYear { get; set; }
        public string sBudget { get; set; }
        public string sBudgetUsed { get; set; }
        public decimal? nBudget { get; set; }
        public decimal? nBudgetUsed { get; set; }
        public string sType { get; set; }
        public string sDimension { get; set; }
        public string sBudgetStatus { get; set; }
        public string sStatus { get; set; }
        public int nStatusID { get; set; }
        public string sButtonAction { get; set; }
        public string sIDEncrypt { get; set; }

        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPassApprove { get; set; }

        public IEnumerable<c_gl> lstGL { get; set; }
    }

    [Serializable]
    public class c_gl
    {
        public string sGLID { get; set; }
        public string sGLName { get; set; }
        public string sBudget { get; set; }
    }
    #endregion
}