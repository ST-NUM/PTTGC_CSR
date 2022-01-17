using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    private static int nMenuID = 1;

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

                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                if (sPer != "N")
                {
                    lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");

                    PTTGC_CSREntities db = new PTTGC_CSREntities();
                    var dUpdate = db.TB_DateSync.Max(o => o.dUpdate);
                    lbUpdate.Text = dUpdate.HasValue ? dUpdate.Value.ToString("ปรับปรุงล่าสุด : dd/MM/yyyy HH:mm น.") : "";

                    lbYear.Text = DateTime.Now.Year + "";
                }
                else
                {
                    SetBodyEventOnLoad("PopupNoPermission('" + GetPathRedirect() + "');");
                }
            }
        }
    }

    public static string GetPathRedirect()
    {
        string sRet = "";
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        int nUserID = UserAccount.SessionInfo.nUserID;
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.IsActive && !w.IsDel);
        var lstTM_Menu = db.TM_Menu.Where(w => w.IsActive).OrderBy(o => o.nMenuOrder).ToList();
        var lstMenuID = lstTM_Menu.Select(s => s.nMenuID).ToList();
        var qHasPer = db.TB_User_Permission.FirstOrDefault(w => w.nUserID == nUserID && lstMenuID.Contains(w.nMenuID) && w.nPermission > 0);
        if (qHasPer != null)
        {
            var qMenu = lstTM_Menu.FirstOrDefault(w => w.nMenuID == qHasPer.nMenuID);
            sRet = qMenu != null ? qMenu.sMenuLink : "unauthorize.aspx";
        }
        else
        {
            sRet = "unauthorize.aspx";
        }

        return sRet;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData GetData()
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            int nYear = DateTime.Now.Year;

            var lstBudget = db.TB_Budget_Sub.Where(w => w.dPostingDate.Year == nYear).ToList();

            #region รวมทั้งหมด
            var lstIO = db.TB_InternalOrder.Where(w => w.nYear == nYear).ToList();
            result.nOrder = lstBudget.Select(s => s.sIOID).Distinct().Count();

            var lstProject = db.T_Project.Where(w => w.nYear == nYear && !w.IsDel && w.IsPassApprove).ToList();
            result.nProject = lstProject.Count();

            var nBudget = lstIO.Sum(s => s.nBudget);
            var nBudgetUsed = lstBudget.Sum(s => s.nValInRepCur);
            var nAvailable = nBudget - nBudgetUsed;
            var nPerUsed = nBudget > 0 ? (int)Math.Ceiling(((nBudgetUsed * 100) / nBudget)) : 0;
            var nPerAvailable = 100 - nPerUsed;

            result.sBudget = nBudget.ToString("#,#");
            result.sBudgetUsed = nBudgetUsed.ToString("#,#");
            result.sAvailable = nAvailable.ToString("#,#");
            result.sBudgetUsedPer = nPerUsed + "";
            result.sAvailablePer = nPerAvailable + "";

            var lstOrder = new List<c_detail>();
            for (int i = 1; i <= 12; i++)
            {
                string sMonthName = new DateTime(nYear, i, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("th"));
                lstOrder.Add(new c_detail() { month = sMonthName, nValue = (int)lstBudget.Where(w => w.dPostingDate.Year == nYear && w.dPostingDate.Month == i).Sum(s => s.nValInRepCur) });
            }
            result.lstOrder = lstOrder;
            #endregion

            #region GL Total Used
            var lstGL = new List<c_gl>();
            var lstGLID = lstBudget.Select(s => s.sGLID).Distinct().OrderBy(o => o).ToList();
            var lstGLMaster = db.TB_GLAccount.Where(w => w.nYear == nYear).ToList();
            foreach (var sGLID in lstGLID)
            {
                var qGL = lstGLMaster.FirstOrDefault(w => w.sGLID == sGLID);
                var lstGLThis = lstGLMaster.Where(w => w.sGLID == sGLID);

                var lstGLSub = new List<c_detail>();
                for (int i = 1; i <= 12; i++)
                {
                    string sMonthName = new DateTime(nYear, i, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("th"));
                    lstGLSub.Add(new c_detail() { month = sMonthName, nValue = (int)lstBudget.Where(w => w.dPostingDate.Month == i && w.sGLID == sGLID).Sum(s => s.nValInRepCur) });
                }

                var nGLTotal = (int)lstGLThis.Sum(s => s.nBudget);
                var nGLUsed = (int)lstGLThis.Sum(s => s.nBudgetUsed);
                var nGLAvailable = nGLTotal - nGLUsed;

                lstGL.Add(new c_gl()
                {
                    sGLID = sGLID,
                    sName = "(" + sGLID + ") " + (qGL != null ? qGL.sGLName : ""),
                    sBudget1 = nGLUsed.ToString("#,#"),
                    sBudget2 = nGLAvailable.ToString("#,#"),//lstBudget.Where(w => w.sGLID == sGLID).Sum(s => s.nValInRepCur).ToString("C0").Replace("$", ""),
                    lstData = lstGLSub
                });
            }

            result.lstGL = lstGL;
            #endregion

            #region Group
            var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).OrderBy(o => o.nOrder).ToList();
            result.lstHeadGroup = lstProjectType.Select(s => s.sName).ToList();

            var lstGroup = new List<c_group>();

            var qMap = db.TB_Map_Budget.FirstOrDefault(w => !w.IsDel && w.nYear == nYear);
            var nID = qMap != null ? qMap.nID : 0;
            var lstMap = db.TB_Map_Budget_Sub.Where(w => w.nID == nID).ToList();
            var lstCC = db.TB_CostCenter.Where(w => w.nYear == nYear).OrderBy(o => o.sCostCenterID).ToList();
            foreach (var qCC in lstCC)
            {
                var lstIOID_ = lstIO.Where(w => w.sCostCenterID == qCC.sCostCenterID).Select(s => s.sIOID).ToList();
                var lstBudget_ = new List<string>();
                foreach (var qType in lstProjectType)
                {
                    decimal nVal = 0;
                    int nProjectType = qType.nSubID;
                    var lstMap_ = lstMap.Where(w => lstIOID_.Contains(w.sIOID) && w.nProjectType == nProjectType).ToList();
                    foreach (var qMap_ in lstMap_)
                    {
                        nVal += lstBudget.Where(w => w.sIOID == qMap_.sIOID && w.sGLID == qMap_.sGLID).Sum(s => s.nValInRepCur);
                    }

                    lstBudget_.Add(nVal.ToString("#,#"));
                }

                lstGroup.Add(new c_group() { sName = qCC.sCostCenterName, lstBudget = lstBudget_ });
            }

            result.lstGroup = lstGroup;
            #endregion

            #region Dimension Group
            var lstDM = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive && w.nMainID == 3).ToList();
            var lstDimension = new List<c_dimension>();
            foreach (var itemD in lstDM)
            {
                var lstPro = lstProject.Where(w => w.nDimensionID == itemD.nSubID).ToList();
                var lstProID_ = lstPro.Select(s => s.nProjectID).ToList();
                int nValue = (int)lstBudget.Where(w => lstProID_.Contains(w.nProjectID)).Sum(s => s.nValInRepCur);
                if (nValue > 0)
                {
                    var lstDimensionSub = new List<c_dimension_sub>();
                    if (itemD.nSubID == 9 || itemD.nSubID == 10)
                    {
                        var nMain = itemD.nSubID == 9 ? 4 : 5;
                        var lstDMSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive && w.nMainID == nMain).ToList();
                        foreach (var itemDMSub in lstDMSub)
                        {
                            var lstProID__ = lstPro.Where(w => w.nDimensionSubID == itemDMSub.nSubID).Select(s => s.nProjectID).ToList();
                            lstDimensionSub.Add(new c_dimension_sub()
                            {
                                sName = itemDMSub.sName,
                                nValue = (int)lstBudget.Where(w => lstProID__.Contains(w.nProjectID)).Sum(s => s.nValInRepCur)
                            });
                        }
                    }

                    lstDimension.Add(new c_dimension()
                    {
                        nDimensionID = itemD.nSubID,
                        sName = itemD.sName,
                        nValue = nValue,
                        sColor = "",
                        lstDimensionSub = lstDimensionSub
                    });
                }
            }
            result.lstDimension = lstDimension;
            #endregion

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
        public int nOrder { get; set; }
        public int nProject { get; set; }

        public string sBudget { get; set; }
        public string sBudgetUsed { get; set; }
        public string sAvailable { get; set; }
        public string sBudgetUsedPer { get; set; }
        public string sAvailablePer { get; set; }
        public IEnumerable<c_detail> lstOrder { get; set; }

        public IEnumerable<c_gl> lstGL { get; set; }

        public List<string> lstHeadGroup { get; set; }
        public IEnumerable<c_group> lstGroup { get; set; }
        public IEnumerable<c_dimension> lstDimension { get; set; }
    }

    [Serializable]
    public class c_gl
    {
        public string sGLID { get; set; }
        public string sName { get; set; }
        public string sBudget1 { get; set; }
        public string sBudget2 { get; set; }
        public IEnumerable<c_detail> lstData { get; set; }
    }

    [Serializable]
    public class c_detail
    {
        public string month { get; set; }
        public int nValue { get; set; }
    }

    [Serializable]
    public class c_group
    {
        public string sName { get; set; }
        public List<string> lstBudget { get; set; }
    }

    [Serializable]
    public class c_dimension
    {
        public int nDimensionID { get; set; }
        public string sName { get; set; }
        public int nValue { get; set; }
        public string sColor { get; set; }
        public IEnumerable<c_dimension_sub> lstDimensionSub { get; set; }
    }

    [Serializable]
    public class c_dimension_sub
    {
        public string sName { get; set; }
        public int nValue { get; set; }
    }
    #endregion
}