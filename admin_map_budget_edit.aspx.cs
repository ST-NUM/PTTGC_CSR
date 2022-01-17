using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_map_budget_edit : System.Web.UI.Page
{
    private static int nMenuID = 13;

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

                string mode = Request.QueryString["mode"];
                var IsView = !string.IsNullOrEmpty(mode) ? mode == "v" : false;

                string sPageType = "เพิ่ม";
                string sPer = hddPermission.Value = IsView ? "V" : SystemFunction.GetPMS(nMenuID);
                SetControl();

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

        #region Project Type
        var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
        ddlProjectType.DataSource = lstProjectType;
        ddlProjectType.DataValueField = "nSubID";
        ddlProjectType.DataTextField = "sName";
        ddlProjectType.DataBind();
        ddlProjectType.Items.Insert(0, new ListItem("- ประเภท  -", ""));
        #endregion     

        SystemFunction.BindDdlPageSize(ddlPageSize);
    }

    public void SetData(string sID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qMap = db.TB_Map_Budget.FirstOrDefault(w => w.nID + "" == sID && !w.IsDel);
        if (qMap != null)
        {
            txtYear.Text = qMap.nYear + "";
            txtYear.Enabled = false;

            #region Order
            var lstOrder = db.TB_InternalOrder.Where(w => w.nYear == qMap.nYear).Select(s => new
            {
                sID = s.sIOID,
                sName = s.sIOID + " - " + s.sIOName
            }).ToList();
            ddlOrder.DataSource = lstOrder;
            ddlOrder.DataValueField = "sID";
            ddlOrder.DataTextField = "sName";
            ddlOrder.DataBind();
            ddlOrder.Items.Insert(0, new ListItem("- Order  -", ""));
            #endregion

            ddlGL.Items.Insert(0, new ListItem("- GL  -", ""));

        }
        else
        {
            Response.Redirect("admin_map_budget.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData GetData(int nID)
    {
        TReturnData result = new TReturnData();
        result.lstData = new List<c_data>();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            if (nID != 0)
            {
                var qMain = db.TB_Map_Budget.FirstOrDefault(w => w.nID == nID);
                var nYear = qMain != null ? qMain.nYear : 0;
                var lstMap = db.TB_Map_Budget_Sub.Where(w => w.nID == nID).ToList();
                var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
                var lstIO = db.TB_InternalOrder.Where(w => w.nYear == nYear).ToList();
                var lstGL = db.TB_GLAccount.Where(w => w.nYear == nYear).ToList();

                var lstBudget = db.Database.SqlQuery<TB_Budget_Sub>("select * from TB_Budget_Sub where YEAR(dPostingDate) = " + nYear).ToList();

                var lstData = (from a in lstMap
                               from b in lstProjectType.Where(w => w.nSubID == a.nProjectType)
                               from c in lstIO.Where(w => w.sIOID == a.sIOID)
                               from d in lstGL.Where(w => w.sIOID == a.sIOID && w.sGLID == a.sGLID)
                               select new c_data
                               {
                                   nItem = a.nItem,
                                   nProjectType = a.nProjectType,
                                   sProjectType = b.sName,
                                   sIOID = a.sIOID,
                                   sIOName = a.sIOID + " - " + c.sIOName,
                                   sGLID = a.sGLID,
                                   sGLName = a.sGLID + " - " + d.sGLName,
                                   IsCanDel = !lstBudget.Any(w => w.sIOID == a.sIOID && w.sGLID == a.sGLID)
                               }).ToList();

                result.lstData = lstData;

                result.lstGL = lstGL;
            }

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
    public static sysGlobalClass.CResutlWebMethod SaveData(int nID, int nYear, List<c_data> lstData)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var nUserThis = UserAccount.SessionInfo.nUserID;

            var IsNew = nID == 0;
            var qDup = db.TB_Map_Budget.FirstOrDefault(w => (!IsNew ? w.nID != nID : true) && w.nYear == nYear && !w.IsDel);
            if (qDup == null)
            {
                var qMap = db.TB_Map_Budget.FirstOrDefault(w => w.nID == nID);
                if (nID > 0 && qMap.IsDel)
                {
                    result.Msg = SystemFunction.sMsgSaveInNotStep;
                    result.Status = SystemFunction.process_SaveFail;
                    return result;
                }

                #region Add/Update TB_Map_Budget
                if (IsNew)
                {
                    qMap = new TB_Map_Budget();
                    qMap.nYear = nYear;
                    qMap.nCreateBy = nUserThis;
                    qMap.dCreate = DateTime.Now;
                    db.TB_Map_Budget.Add(qMap);
                }

                qMap.nRow = lstData.Count();
                qMap.IsDel = false;
                qMap.nUpdateBy = nUserThis;
                qMap.dUpdate = DateTime.Now;
                db.SaveChanges();
                #endregion

                #region TB_Map_Budget_Sub

                #region Remove Old
                CommonFunction.ExecuteNonQuery("delete TB_Map_Budget_Sub where nID = " + qMap.nID);
                #endregion

                #region Add TB_Map_Budget_Sub
                if (lstData.Any())
                {
                    int nItem = 1;
                    foreach (var item in lstData)
                    {
                        db.TB_Map_Budget_Sub.Add(new TB_Map_Budget_Sub() { nID = qMap.nID, nItem = nItem, sIOID = item.sIOID, sGLID = item.sGLID, nProjectType = item.nProjectType });
                        nItem++;
                    }

                    db.SaveChanges();
                }
                #endregion

                #endregion

                result.Msg = HttpUtility.UrlEncode(STCrypt.Encrypt(qMap.nID + ""));
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
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_data> lstData { get; set; }
        public IEnumerable<TB_GLAccount> lstGL { get; set; }
    }

    [Serializable]
    public class c_data
    {
        public int nItem { get; set; }
        public int nProjectType { get; set; }
        public string sProjectType { get; set; }
        public string sIOID { get; set; }
        public string sIOName { get; set; }
        public string sGLID { get; set; }
        public string sGLName { get; set; }
        public bool IsCanDel { get; set; }
    }
    #endregion
}