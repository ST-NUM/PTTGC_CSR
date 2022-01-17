using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_masterdata : System.Web.UI.Page
{
    private static int nMenuID = 12;

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

        var lstType = db.TM_MasterData.Where(w => w.IsManage).ToList();
        ddlType.DataSource = lstType;
        ddlType.DataValueField = "nMainID";
        ddlType.DataTextField = "sName";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("- ประเภท  -", ""));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sType, string sActive)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            sName = sName.ToLower().Replace(" ", "");

            var lstMainID = new List<int> { 4, 5, 8 };
            var lstMain = db.TM_MasterData.Where(w => w.IsManage).ToList();
            var lstSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && lstMainID.Contains(w.nMainID) &&
                             (!string.IsNullOrEmpty(sName) ? w.sName.ToLower().Replace(" ", "").Contains(sName) : true) &&
                             (!string.IsNullOrEmpty(sType) ? (w.nMainID + "" == sType) : true) &&
                             (!string.IsNullOrEmpty(sActive) ? (w.IsActive == (sActive == "1")) : true)
                             ).OrderByDescending(o => o.dUpdate).ToList();

            var lstDimensionSubID = db.T_Project.Where(w => !w.IsDel && w.nDimensionSubID.HasValue).Select(s => s.nDimensionSubID.Value).Distinct().ToList();
            var lstObjectiveID = db.TB_Budget_Sub.Where(w => w.nObjective.HasValue).Select(s => s.nObjective.Value).Distinct().ToList();
            var lstUsedID = lstDimensionSubID.Concat(lstObjectiveID).ToList();

            var lstData = (from a in lstSub
                           from b in lstMain.Where(w => w.nMainID == a.nMainID).DefaultIfEmpty()
                           select new c_Master
                           {
                               nSubID = a.nSubID,
                               sName = a.sName,
                               sType = b != null ? b.sName : "",
                               sActive = a.IsActive ? "ใช้งาน" : "ไม่ใช้งาน",
                               sUpdateDate = a.dUpdate.HasValue ? a.dUpdate.Value.ToString("dd/MM/yyyy") : "-",
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nSubID + "")),
                               IsCanDel = (lstMainID.Contains(a.nMainID)) ? !lstUsedID.Any(aa => aa == a.nSubID) : true
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
                var lstData = db.TM_MasterData_Sub.Where(w => lstID.Contains(w.nSubID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                CSR_Function.UpdateLog(nMenuID, "", "Delete Master Data = " + string.Join(", ", lstID));
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
        public IEnumerable<c_Master> lstData { get; set; }
    }

    [Serializable]
    public class c_Master
    {
        public int nSubID { get; set; }
        public string sName { get; set; }
        public string sType { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public bool IsCanDel { get; set; }
    }
    #endregion
}