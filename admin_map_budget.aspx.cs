using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_map_budget : System.Web.UI.Page
{
    private static int nMenuID = 13;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                SetControl();
                SystemFunction.BindDdlPageSize(ddlPageSize);
                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
            }
        }
    }

    public void SetControl()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        int nYear = DateTime.Now.Year;
        var lstYear = db.TB_Map_Budget.Where(w => !w.IsDel).OrderByDescending(o => o.nYear).Select(s => s.nYear).ToList();
        if (lstYear.Any())
        {
            int nIndex = 0;
            foreach (var item in lstYear)
            {
                ddlYear.Items.Insert(nIndex, new ListItem(item + "", item + ""));
                nIndex++;
            }
        }
        else
        {
            ddlYear.Items.Insert(0, new ListItem(nYear + "", nYear + ""));
        }

        ddlYear.SelectedValue = nYear + "";
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sYear)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            var lstMap = db.TB_Map_Budget.Where(w => !w.IsDel).ToList();
            var lstMapID = lstMap.Select(s => s.nID).ToList();
            var lstMap_Sub = db.TB_Map_Budget_Sub.Where(w => lstMapID.Contains(w.nID)).ToList();
            var lstBudget = db.TB_Budget_Sub.ToList();

            Func<int, int, bool> GetCanDel = (nID, nYear) =>
             {
                 var lstMap_Sub_ = lstMap_Sub.Where(w => w.nID == nID).ToList();
                 foreach (var item in lstMap_Sub_)
                 {
                     if (lstBudget.Any(a => a.dPostingDate.Year == nYear && a.sIOID == item.sIOID && a.sGLID == item.sGLID))
                     {
                         return false;
                     }
                 }
                 return true;
             };

            var lstData = (from a in lstMap
                           select new c_data
                           {
                               nID = a.nID,
                               nYear = a.nYear,
                               nRow = a.nRow,
                               sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nID + "")),
                               IsCanDel = GetCanDel(a.nID, a.nYear)
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
                var lstData = db.TB_Map_Budget.Where(w => lstID.Contains(w.nID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                CSR_Function.UpdateLog(nMenuID, "", "Delete TB_Map_Budget = " + string.Join(", ", lstID));
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
        public IEnumerable<c_data> lstData { get; set; }
    }

    [Serializable]
    public class c_data
    {
        public int nID { get; set; }
        public int nYear { get; set; }
        public int nRow { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public bool IsCanDel { get; set; }
    }
    #endregion
}