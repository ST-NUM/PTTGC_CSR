using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class report_1 : System.Web.UI.Page
{
    private static int nMenuID = 6;

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
                ((_MP_Front)Master).MenuID_Selected = nMenuID;
                SetControl();
                SystemFunction.BindDdlPageSize(ddlPageSize);

                var IsApprover = CSR_Function.IsApprover(null);
                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
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
        ddlYear.Items.Insert(0, new ListItem("- ปี -", ""));
        int nYearStart = 2019;
        int nYearNow = DateTime.Now.Year;
        int nIndex = 1;
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

            var lstProject = db.T_Project.Where(w => !w.IsDel &&
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

            //Func<decimal, decimal, string> GetBudgetStatus = (nBudget, nBudgetUsed) =>
            //{
            //    string sRet = "";
            //    var nPer = (nBudgetUsed * 100) / nBudget;
            //    if (nPer < 80) { sRet = "bg-success"; }
            //    else if (nPer > 100) { sRet = "bg-danger"; }
            //    else { sRet = "bg-warning"; }

            //    return "<div class='circle " + sRet + "'></div>";
            //};

            var lstData = (from a in lstProject
                           from b in lstDimension.Where(w => w.nSubID == a.nDimensionID).DefaultIfEmpty()
                           from c in lstStatus.Where(w => w.nStatusID == a.nStatusID)
                           from d in lstProjectType.Where(w => w.nSubID == a.nProjectType).DefaultIfEmpty()
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
                               //sBudgetStatus = a.IsPassApprove ? GetBudgetStatus(a.nBudget.Value, (a.nBudgetUsed ?? 0)) : "",

                               lstGL = a.nBudget.HasValue ? GetGL(a.nProjectID) : new List<c_gl>(),

                               nStatusID = a.nStatusID,
                               sStatus = c.sStatusName,
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

    #region Export Excel  
    public static XLColor sColorHeadTb = XLColor.FromHtml("#C0DDF1");
    public static XLBorderStyleValues borderThin = XLBorderStyleValues.Thin;
    public static XLAlignmentHorizontalValues hCenter = XLAlignmentHorizontalValues.Center;
    public static XLAlignmentHorizontalValues hLeft = XLAlignmentHorizontalValues.Left;
    public static XLAlignmentHorizontalValues hRight = XLAlignmentHorizontalValues.Right;
    public static XLAlignmentVerticalValues vCenter = XLAlignmentVerticalValues.Center;
    public static XLAlignmentVerticalValues vTop = XLAlignmentVerticalValues.Top;

    protected void btnExportProject_Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
        {
            var arrProjectID = hddProjectID.Value.Split(',');
            if (arrProjectID.Length > 0)
            {
                HttpResponse httpResponse = Response;
                int nRow = 1;

                #region Define Excel
                XLWorkbook wb = new XLWorkbook();
                IXLWorksheet ws1 = wb.Worksheets.Add("Sheet1");
                ws1.PageSetup.Margins.Top = 0.2;
                ws1.PageSetup.Margins.Bottom = 0.2;
                ws1.PageSetup.Margins.Left = 0.1;
                ws1.PageSetup.Margins.Right = 0;
                ws1.PageSetup.Margins.Footer = 0;
                ws1.PageSetup.Margins.Header = 0;
                ws1.Style.Font.FontName = "Browallia New";
                ws1.SheetView.Freeze(1, 2);
                #endregion

                #region Header    
                SetTbl(ws1, nRow, 1, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "ลำดับ");
                SetTbl(ws1, nRow, 2, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "ชื่อโครงการ");
                SetTbl(ws1, nRow, 3, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "รหัสโครงการ");
                SetTbl(ws1, nRow, 4, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "ปี");
                SetTbl(ws1, nRow, 5, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "ประเภท");
                SetTbl(ws1, nRow, 6, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "หน่วยงาน");
                SetTbl(ws1, nRow, 7, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "ผู้รับผิดชอบโครงการ");
                SetTbl(ws1, nRow, 8, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "Cost Center");
                SetTbl(ws1, nRow, 9, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "Order");
                SetTbl(ws1, nRow, 10, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "เดือนเริ่มต้น - สิ้นสุด");
                SetTbl(ws1, nRow, 11, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "งบโครงการ");
                SetTbl(ws1, nRow, 12, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "งบโครงการที่ใช้ไป");
                SetTbl(ws1, nRow, 13, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "CSR Dimension");
                SetTbl(ws1, nRow, 14, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "CSR Dimension ย่อย");
                SetTbl(ws1, nRow, 15, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "Internal");
                SetTbl(ws1, nRow, 16, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "External");
                SetTbl(ws1, nRow, 17, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "Objective");
                SetTbl(ws1, nRow, 18, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "วัตถุประสงค์");
                SetTbl(ws1, nRow, 19, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "ประโยชน์ที่คาดว่าจะได้รับ");
                SetTbl(ws1, nRow, 20, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "หมายเหตุเพิ่มเติม");
                SetTbl(ws1, nRow, 21, 11, true, hCenter, vCenter, true, null, null, sColorHeadTb, "สถานะ");
                SetTbl(ws1, nRow, 22, 11, true, hCenter, vCenter, true, null, 8, null, "End");
                nRow++;
                #endregion

                #region Query
                PTTGC_CSREntities db = new PTTGC_CSREntities();
                var lstProject = db.T_Project.Where(w => arrProjectID.Contains(w.nProjectID + "")).ToList();
                var lstProID = lstProject.Select(s => s.nProjectID).ToList();
                var lstCC = db.TB_CostCenter.ToList();
                var lstIO = db.TB_InternalOrder.ToList();
                var lstBudget = db.TB_Budget_Sub.Where(w => lstProID.Contains(w.nProjectID)).ToList();

                var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
                var lstDimension = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && !w.IsDel && w.IsActive).ToList();
                var lstDimensionSub = db.TM_MasterData_Sub.Where(w => (w.nMainID == 4 || w.nMainID == 5) && !w.IsDel && w.IsActive).ToList();
                var lstStatus = db.TM_ProjectStatus.ToList();
                var lstOrg = db.TM_Organization.ToList();
                var lstOwnerID = lstProject.Select(s => s.nOwnerID).Distinct().ToList();
                var lstUser = db.TB_User.Where(w => lstOwnerID.Contains(w.nUserID)).ToList();

                var lstData = (from a in lstProject
                               from b in lstDimension.Where(w => w.nSubID == a.nDimensionID).DefaultIfEmpty()
                               from c in lstStatus.Where(w => w.nStatusID == a.nStatusID)
                               from d in lstProjectType.Where(w => w.nSubID == a.nProjectType).DefaultIfEmpty()
                               from ee in lstCC.Where(w => w.nYear == a.nYear && w.sCostCenterID == a.sCostCenterID).DefaultIfEmpty()
                               from f in lstIO.Where(w => w.nYear == a.nYear && w.sIOID == a.sIOID).DefaultIfEmpty()
                               from g in lstDimensionSub.Where(w => w.nSubID == a.nDimensionSubID).DefaultIfEmpty()
                               from h in lstOrg.Where(w => w.sOrgID == a.sOrgID).DefaultIfEmpty()
                               from i in lstUser.Where(w => w.nUserID == a.nOwnerID).DefaultIfEmpty()
                               select new
                               {
                                   sProjectCode = a.sProjectCode + "",
                                   sProjectName = a.sProjectName,
                                   nYear = a.nYear,
                                   sProjectType = d != null ? d.sName : "-",
                                   sUnitName = h != null ? h.sOrgNameAbbr : "",
                                   sOwner = i != null ? i.sFirstname + " " + i.sLastname : "",
                                   sCostCenter = ee != null ? ee.sCostCenterID + " - " + ee.sCostCenterName : "",
                                   sOrder = f != null ? f.sIOID + " - " + f.sIOName : "",
                                   sStartEnd = (a.nMonthStart.HasValue ? new DateTime(2010, a.nMonthStart.Value, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("th")) : "") + " - " +
                                               (a.nMonthEnd.HasValue ? new DateTime(2010, a.nMonthEnd.Value, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("th")) : ""),
                                   sBudget = (a.nBudget ?? 0).ToString("#,#"),
                                   sBudgetUsed = (a.nBudgetUsed ?? 0).ToString("#,#"),
                                   sDimension = b != null ? b.sName : "-",
                                   sDimensionSub = g != null ? g.sName : "-",
                                   sInternalD = a.sInternalD,
                                   sExternalD = a.sExternalD,
                                   sObjectiveD = a.sObjectiveD,
                                   sObjective = a.sObjective,
                                   sBenefit = a.sBenefit,
                                   sRemark = a.sRemark,
                                   sStatus = c.sStatusName
                               }).ToList();
                #endregion

                #region Set Data
                int nCount = 1;
                foreach (var item in lstData)
                {
                    SetTbl(ws1, nRow, 1, 11, false, hCenter, vCenter, true, null, null, null, nCount + ".");
                    SetTbl(ws1, nRow, 2, 11, false, hLeft, vCenter, true, null, 40, null, item.sProjectName);
                    SetTbl(ws1, nRow, 3, 11, false, hCenter, vCenter, true, null, 12, null, item.sProjectCode);
                    SetTbl(ws1, nRow, 4, 11, false, hCenter, vCenter, true, null, null, null, item.nYear + "");
                    SetTbl(ws1, nRow, 5, 11, false, hCenter, vCenter, true, null, 15, null, item.sProjectType);
                    SetTbl(ws1, nRow, 6, 11, false, hCenter, vCenter, true, null, 14, null, item.sUnitName);
                    SetTbl(ws1, nRow, 7, 11, false, hLeft, vCenter, true, null, 25, null, item.sOwner);
                    SetTbl(ws1, nRow, 8, 11, false, hLeft, vCenter, true, null, 22, null, item.sCostCenter);
                    SetTbl(ws1, nRow, 9, 11, false, hLeft, vCenter, true, null, 50, null, item.sOrder);
                    SetTbl(ws1, nRow, 10, 11, false, hCenter, vCenter, true, null, 18, null, item.sStartEnd);
                    SetTbl(ws1, nRow, 11, 11, false, hRight, vCenter, true, 0, 12, null, item.sBudget);
                    SetTbl(ws1, nRow, 12, 11, false, hRight, vCenter, true, 0, 16, null, item.sBudgetUsed);
                    SetTbl(ws1, nRow, 13, 11, false, hCenter, vCenter, true, null, 16, null, item.sDimension);
                    SetTbl(ws1, nRow, 14, 11, false, hCenter, vCenter, true, null, 25, null, item.sDimensionSub);
                    SetTbl(ws1, nRow, 15, 11, false, hLeft, vCenter, true, null, 40, null, item.sInternalD);
                    SetTbl(ws1, nRow, 16, 11, false, hLeft, vCenter, true, null, 40, null, item.sExternalD);
                    SetTbl(ws1, nRow, 17, 11, false, hLeft, vCenter, true, null, 40, null, item.sObjectiveD);
                    SetTbl(ws1, nRow, 18, 11, false, hLeft, vCenter, true, null, 40, null, item.sObjective);
                    SetTbl(ws1, nRow, 19, 11, false, hLeft, vCenter, true, null, 40, null, item.sBenefit);
                    SetTbl(ws1, nRow, 20, 11, false, hLeft, vCenter, true, null, 40, null, item.sRemark);
                    SetTbl(ws1, nRow, 21, 11, false, hCenter, vCenter, true, null, 15, null, item.sStatus);
                    nRow++;
                    nCount++;
                }
                #endregion

                #region CreateEXCEL
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                string sName = "All_Project_Report_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

                httpResponse.AddHeader("content-disposition", "attachment;filename=" + sName + ".xlsx");

                // Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
                #endregion
            }
        }
        else
        {
            SetBodyEventOnLoad("PopupSessionTimeOut();");
        }
    }

    protected void btnExportTransaction_Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
        {
            var sProjectID = hddProjectID.Value;
            HttpResponse httpResponse = Response;
            int nRow = 1;

            #region Define Excel
            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws1 = wb.Worksheets.Add("Sheet1");
            ws1.PageSetup.Margins.Top = 0.2;
            ws1.PageSetup.Margins.Bottom = 0.2;
            ws1.PageSetup.Margins.Left = 0.1;
            ws1.PageSetup.Margins.Right = 0;
            ws1.PageSetup.Margins.Footer = 0;
            ws1.PageSetup.Margins.Header = 0;
            ws1.Style.Font.FontName = "Browallia New";
            #endregion

            #region Header    
            SetTbl(ws1, nRow, 1, 11, true, hCenter, vCenter, true, null, 7, sColorHeadTb, "ลำดับ");
            SetTbl(ws1, nRow, 2, 11, true, hCenter, vCenter, true, null, 8, sColorHeadTb, "Period");
            SetTbl(ws1, nRow, 3, 11, true, hCenter, vCenter, true, null, 14, sColorHeadTb, "Posting Date");
            SetTbl(ws1, nRow, 4, 11, true, hCenter, vCenter, true, null, 40, sColorHeadTb, "ชื่อโครงการ");
            SetTbl(ws1, nRow, 5, 11, true, hCenter, vCenter, true, null, 15, sColorHeadTb, "ประเภท");
            SetTbl(ws1, nRow, 6, 11, true, hCenter, vCenter, true, null, 40, sColorHeadTb, "Description");
            SetTbl(ws1, nRow, 7, 11, true, hCenter, vCenter, true, null, 15, sColorHeadTb, "Amount");
            SetTbl(ws1, nRow, 8, 11, true, hCenter, vCenter, true, null, 22, sColorHeadTb, "Pay To");
            SetTbl(ws1, nRow, 9, 11, true, hCenter, vCenter, true, null, 15, sColorHeadTb, "Order");   
            SetTbl(ws1, nRow, 10, 11, true, hCenter, vCenter, true, null, 12, sColorHeadTb, "Cost Element");
            SetTbl(ws1, nRow, 11, 11, true, hCenter, vCenter, true, null, 30, sColorHeadTb, "Cost element name");
            SetTbl(ws1, nRow, 12, 11, true, hCenter, vCenter, true, null, 20, sColorHeadTb, "Objective");
            SetTbl(ws1, nRow, 13, 11, true, hCenter, vCenter, true, null, 15, sColorHeadTb, "Area");
            SetTbl(ws1, nRow, 14, 11, true, hCenter, vCenter, true, null, 40, sColorHeadTb, "Internal");
            SetTbl(ws1, nRow, 15, 11, true, hCenter, vCenter, true, null, 40, sColorHeadTb, "External");
            SetTbl(ws1, nRow, 16, 11, true, hCenter, vCenter, true, null, 25, sColorHeadTb, "Philanthropic Activities");
            SetTbl(ws1, nRow, 17, 11, true, hCenter, vCenter, true, null, 8, null, "End");
            nRow++;
            #endregion

            #region Query
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            var lstBudget = db.TB_Budget_Sub.Where(w => w.nProjectID + "" == sProjectID).ToList();
            var lstProject = db.T_Project.Where(w => w.nProjectID + "" == sProjectID).ToList();
            var lstGL = db.TB_GLAccount.ToList();
            var lstMasterSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive).ToList();
            var lstObjective = lstMasterSub.Where(w => w.nMainID == 8).ToList();
            var lstArea = lstMasterSub.Where(w => w.nMainID == 6).ToList();
            var lstPA = lstMasterSub.Where(w => w.nMainID == 2).ToList();
            var lstProjectType = lstMasterSub.Where(w => w.nMainID == 7).ToList();
            var qPro = lstProject.FirstOrDefault();
            string sProjectCode = qPro != null ? qPro.sProjectCode : "";

            var lstData = (from a in lstBudget
                           from b in lstGL.Where(w => w.nYear == a.dPostingDate.Year && w.sGLID == a.sGLID && w.sIOID == a.sIOID)
                           from c in lstObjective.Where(w => w.nSubID == a.nObjective).DefaultIfEmpty()
                           from d in lstArea.Where(w => w.nSubID == a.nArea)
                           from ee in lstPA.Where(w => w.nSubID == a.nPA)
                           from f in lstProject.Where(w => w.nProjectID == a.nProjectID)
                           from g in lstProjectType.Where(w => w.nSubID == f.nProjectType)
                           orderby f.sProjectName ascending
                           select new
                           {
                               sProjectName = f.sProjectName,
                               nPeriod = a.nPeriod,
                               sIOID = a.sIOID,
                               sPostingDate = a.dPostingDate.ToString("dd/MM/yyyy"),
                               sDescription = a.sDescription,
                               sValInRepCur = a.nValInRepCur.ToString("C2").Replace("$", ""),
                               sNameOffsetting = a.sNameOffsetting,
                               sGLID = a.sGLID,
                               sGLName = b.sGLName,
                               sObjective = c != null ? c.sName : "-",
                               sArea = d.sName,
                               sInternal = a.sInternal,
                               sExternal = a.sExternal,
                               sPA = ee.sName,
                               sProjectType = g.sName
                           }).ToList();
            #endregion

            #region Set Data
            if (lstData.Any())
            {
                int nCount = 1;
                foreach (var item in lstData)
                {
                    SetTbl(ws1, nRow, 1, 11, false, hCenter, vTop, false, null, 7, null, nCount + ".");
                    SetTbl(ws1, nRow, 2, 11, false, hCenter, vTop, false, null, 8, null, item.nPeriod + "");
                    SetTbl(ws1, nRow, 3, 11, false, hCenter, vTop, false, null, 14, null, item.sPostingDate);
                    SetTbl(ws1, nRow, 4, 11, false, hLeft, vTop, false, null, 40, null, item.sProjectName);
                    SetTbl(ws1, nRow, 5, 11, false, hCenter, vTop, false, null, 15, null, item.sProjectType);
                    SetTbl(ws1, nRow, 6, 11, false, hLeft, vTop, false, null, 40, null, item.sDescription);
                    SetTbl(ws1, nRow, 7, 11, false, hRight, vTop, false, null, 15, null, item.sValInRepCur);
                    SetTbl(ws1, nRow, 8, 11, false, hLeft, vTop, false, null, 22, null, item.sNameOffsetting);
                    SetTblText(ws1, nRow, 9, 11, false, hLeft, vTop, false, null, 15, null, item.sIOID);  
                    SetTbl(ws1, nRow, 10, 11, false, hLeft, vTop, false, null, 12, null, item.sGLID);
                    SetTbl(ws1, nRow, 11, 11, false, hLeft, vTop, false, null, 30, null, item.sGLName);
                    SetTbl(ws1, nRow, 12, 11, false, hLeft, vTop, false, null, 20, null, item.sObjective);
                    SetTbl(ws1, nRow, 13, 11, false, hLeft, vTop, false, null, 15, null, item.sArea);
                    SetTbl(ws1, nRow, 14, 11, false, hLeft, vTop, false, null, 40, null, item.sInternal);
                    SetTbl(ws1, nRow, 15, 11, false, hLeft, vTop, false, null, 40, null, item.sExternal);
                    SetTbl(ws1, nRow, 16, 11, false, hLeft, vTop, false, null, 25, null, item.sPA);

                    nRow++;
                    nCount++;
                }
            }
            else
            {
                ws1.Range(nRow, 1, nRow, 16).Merge();
                ws1.Range(nRow, 1, nRow, 16).Style.Border.OutsideBorder = ws1.Range(nRow, 1, nRow, 16).Style.Border.InsideBorder = borderThin;
                SetTbl(ws1, nRow, 1, 11, false, hCenter, vCenter, false, null, null, null, "ไม่พบข้อมูล");
            }
            #endregion

            #region CreateEXCEL
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string sName = " Trans_" + sProjectCode + "_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

            httpResponse.AddHeader("content-disposition", "attachment;filename=" + sName + ".xlsx");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            #endregion
        }
        else
        {
            SetBodyEventOnLoad("PopupSessionTimeOut();");
        }
    }

    #region Action
    Action<IXLWorksheet, int, int, int, bool, XLAlignmentHorizontalValues, XLAlignmentVerticalValues, bool, int?, double?, XLColor, string> SetTbl =
        (sWorkSheet, row, col, FontSize, Bold, Horizontal, Vertical, wraptext, dec, width, bgColor, sTxt) =>
    {
        sTxt = sTxt + "";
        sWorkSheet.Cell(row, col).Value = sTxt;
        sWorkSheet.Cell(row, col).Style.Font.FontSize = 14;
        sWorkSheet.Cell(row, col).Style.Font.Bold = Bold;
        sWorkSheet.Cell(row, col).Style.Alignment.WrapText = wraptext;
        sWorkSheet.Cell(row, col).Style.Alignment.Horizontal = Horizontal;
        sWorkSheet.Cell(row, col).Style.Alignment.Vertical = Vertical;
        if (sTxt != "End") sWorkSheet.Cell(row, col).Style.Border.OutsideBorder = sWorkSheet.Cell(row, col).Style.Border.InsideBorder = borderThin;
        if (bgColor != null) sWorkSheet.Cell(row, col).Style.Fill.BackgroundColor = bgColor;
        if (width != null) sWorkSheet.Column(col).Width = width.Value;
        if (dec != null || dec == 0)
        {
            string[] arr = sTxt.Split('.');
            if (arr.Length > 1)
            {
                if (arr[1] != "00")
                {
                    string sformate = dec == 0 ? "#,#" : "#,#0.0";
                    //string sformate = "0.#";
                    sWorkSheet.Cell(row, col).Style.NumberFormat.Format = sformate;
                }
            }
        }

        var nIndex = sTxt.Split('/').Length;
        if (nIndex == 3)
        {
            sWorkSheet.Cell(row, col).Style.DateFormat.Format = "dd/MM/yyyy";
        }
    };

    Action<IXLWorksheet, int, int, int, bool, XLAlignmentHorizontalValues, XLAlignmentVerticalValues, bool, int?, double?, XLColor, string> SetTblText =
     (sWorkSheet, row, col, FontSize, Bold, Horizontal, Vertical, wraptext, dec, width, bgColor, sTxt) =>
     {
         sTxt = sTxt + "";
         sWorkSheet.Cell(row, col).Value = sTxt;
         sWorkSheet.Cell(row, col).Style.Font.FontSize = 14;
         sWorkSheet.Cell(row, col).Style.Font.Bold = Bold;
         sWorkSheet.Cell(row, col).Style.Alignment.WrapText = wraptext;
         sWorkSheet.Cell(row, col).Style.Alignment.Horizontal = Horizontal;
         sWorkSheet.Cell(row, col).Style.Alignment.Vertical = Vertical;
         if (sTxt != "End") sWorkSheet.Cell(row, col).Style.Border.OutsideBorder = sWorkSheet.Cell(row, col).Style.Border.InsideBorder = borderThin;
         if (bgColor != null) sWorkSheet.Cell(row, col).Style.Fill.BackgroundColor = bgColor;
         if (width != null) sWorkSheet.Column(col).Width = width.Value;
         if (dec != null || dec == 0)
         {
             string[] arr = sTxt.Split('.');
             if (arr.Length > 1)
             {
                 if (arr[1] != "00")
                 {
                     string sformate = dec == 0 ? "#,#" : "#,#0.0";
                      //string sformate = "0.#";
                      sWorkSheet.Cell(row, col).Style.NumberFormat.Format = sformate;
                 }
             }
         }

         sWorkSheet.Cell(row, col).SetDataType(XLCellValues.Text);

         var nIndex = sTxt.Split('/').Length;
         if (nIndex == 3)
         {
             sWorkSheet.Cell(row, col).Style.DateFormat.Format = "dd/MM/yyyy";
         }
     };
    #endregion
    #endregion    
}