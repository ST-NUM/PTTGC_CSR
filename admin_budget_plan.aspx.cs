using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_budget_plan : System.Web.UI.Page
{
    private static int nMenuID = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                SetControl();
                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
            }
        }
    }

    public void SetControl()
    {
        int nYearStart = 2019;
        int nYearNow = DateTime.Now.Year;
        int nIndex = 0;
        for (int nYear = nYearStart; nYear <= nYearNow; nYear++)
        {
            ddlYear.Items.Insert(nIndex, new ListItem(nYear + "", nYear + ""));
            nIndex++;
        }
        ddlYear.SelectedValue = nYearNow + "";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SyncBudget()
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAccount.IsExpired)
        {
            try
            {
                CSR_Function.Sync_Organization();
                CSR_Function.Sync_CCTR_Structure();

                PTTGC_CSREntities db = new PTTGC_CSREntities();
                db.TB_DateSync.Add(new TB_DateSync() { dUpdate = DateTime.Now, nUpdateBy = UserAccount.SessionInfo.nUserID });
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                CSR_Function.UpdateLog(9, "Click ซิงค์งบประมาณจาก SAP", ex.Message);
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sCostCenter, string sOrder, string sGL, int nYear)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            sCostCenter = sCostCenter.ToLower().Replace(" ", "");
            sOrder = sOrder.ToLower().Replace(" ", "");
            sGL = sGL.ToLower().Replace(" ", "");

            var lstCCID = new List<string>();
            var lstIOID = new List<string>();
            var lstGL = db.TB_GLAccount.Where(w => w.nYear == nYear && (w.sGLID.Contains(sGL) || w.sGLName.ToLower().Replace(" ", "").Contains(sGL))).OrderBy(o => o.sGLID).ToList();
            if (sGL != "")
            {
                lstCCID = lstGL.Select(s => s.sCostCenterID).Distinct().ToList();
                lstIOID = lstGL.Select(s => s.sIOID).Distinct().ToList();
            }

            var lstIO = db.TB_InternalOrder.Where(w => w.nYear == nYear && (lstCCID.Any() ? lstCCID.Contains(w.sCostCenterID) : true) && (lstIOID.Any() ? lstIOID.Contains(w.sIOID) : true) && (w.sIOID.Contains(sOrder) || w.sIOName.ToLower().Replace(" ", "").Contains(sOrder))).OrderBy(o => o.sIOID).ToList();
            if (sOrder != "")
            {
                lstCCID = lstIO.Select(s => s.sCostCenterID).Distinct().ToList();
            }

            var lstCC = db.TB_CostCenter.Where(w => w.nYear == nYear && (lstCCID.Any() ? lstCCID.Contains(w.sCostCenterID) : true) && (w.sCostCenterID.Contains(sCostCenter) || w.sCostCenterName.ToLower().Replace(" ", "").Contains(sCostCenter))).OrderBy(o => o.sCostCenterID).ToList();

            int nID = 1;
            var lstData = new List<c_Budget>();
            foreach (var qCC in lstCC)
            {
                #region Add Cost Center     
                int nCostCenter = nID;

                lstData.Add(new c_Budget()
                {
                    nID = nCostCenter,
                    nLevel = 1,

                    sName = qCC.sCostCenterID + " - " + qCC.sCostCenterName,
                    nBudget = qCC.nBudget,
                    nBudgetUsed = qCC.nBudgetUsed
                });
                nID++;
                #endregion

                var lstIO_ = lstIO.Where(w => w.sCostCenterID == qCC.sCostCenterID).ToList();
                foreach (var qIO in lstIO_)
                {
                    #region Add Order     
                    int nOrder = nID;
                    lstData.Add(new c_Budget()
                    {
                        nID = nOrder,
                        nParentID = nCostCenter,
                        nLevel = 2,

                        sName = qIO.sIOID + " - " + qIO.sIOName,
                        nBudget = qIO.nBudget,
                        nBudgetUsed = qIO.nBudgetUsed
                    });
                    nID++;
                    #endregion

                    var lstGL_ = lstGL.Where(w => w.sCostCenterID == qCC.sCostCenterID && w.sIOID == qIO.sIOID).ToList();
                    if (lstGL_.Any())
                    {
                        #region Add GL
                        foreach (var qGL in lstGL_)
                        {
                            lstData.Add(new c_Budget()
                            {
                                nID = nID,
                                nParentID = nOrder,
                                nLevel = 3,

                                sName = qGL.sGLID + " - " + qGL.sGLName,
                                nBudget = qGL.nBudget,
                                nBudgetUsed = qGL.nBudgetUsed
                            });
                            nID++;
                        }
                        #endregion
                    }
                }
            }

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
        public IEnumerable<c_Budget> lstData { get; set; }
    }

    [Serializable]
    public class c_Budget
    {
        public int nID { get; set; }
        public int? nParentID { get; set; }
        public int nLevel { get; set; }

        public string sName { get; set; }
        public decimal nBudget { get; set; }
        public decimal nBudgetUsed { get; set; }
    }
    #endregion
}