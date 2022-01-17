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

public partial class report_3 : System.Web.UI.Page
{
    private static int nMenuID = 8;

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

        #region Year
        //ddlYear.Items.Insert(0, new ListItem("- ปี -", ""));
        int nYearStart = 2018;
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
    public static TReturnGetData GetData(string sYear)
    {
        TReturnGetData result = new TReturnGetData();
        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            result.lstCC = db.TB_CostCenter.Where(w => w.nYear + "" == sYear).Select(s => new c_data { nYear = s.nYear, sCCID = s.sCostCenterID, sName = s.sCostCenterID + " - " + s.sCostCenterName }).ToList();
            result.lstIO = db.TB_InternalOrder.Where(w => w.nYear + "" == sYear).Select(s => new c_data { nYear = s.nYear, sCCID = s.sCostCenterID, sIOID = s.sIOID, sName = s.sIOID + " - " + s.sIOName }).ToList();
            result.lstGL = db.TB_GLAccount.Where(w => w.nYear + "" == sYear).Select(s => new c_data { nYear = s.nYear, sCCID = s.sCostCenterID, sIOID = s.sIOID, sGLID = s.sGLID, sName = s.sGLID + " - " + s.sGLName }).ToList();
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sProjectName, string sCCID, string sIOID, string sGLID, string sYear)
    {
        TReturnData result = new TReturnData();
        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            sProjectName = sProjectName.ToLower().Replace(" ", "");

            var lstProject = db.T_Project.Where(w => !w.IsDel && (!string.IsNullOrEmpty(sYear) ? w.nYear + "" == sYear : true) &&
            (!string.IsNullOrEmpty(sProjectName) ? (w.sProjectCode.ToLower().Replace(" ", "").Contains(sProjectName) || w.sProjectName.ToLower().Replace(" ", "").Contains(sProjectName)) : true) &&
            (!string.IsNullOrEmpty(sYear) ? w.nYear + "" == sYear : true)).ToList();

            var lstProID = lstProject.Select(s => s.nProjectID).ToList();
            var lstBudget = db.TB_Budget_Sub.Where(w => lstProID.Contains(w.nProjectID) &&
            (!string.IsNullOrEmpty(sIOID) ? w.sIOID == sIOID : true) &&
            (!string.IsNullOrEmpty(sGLID) ? w.sGLID == sGLID : true)).ToList();

            var lstCC = db.TB_CostCenter.Where(w => w.nYear + "" == sYear && (!string.IsNullOrEmpty(sCCID) ? w.sCostCenterID == sCCID : true)).ToList();
            var lstIO = db.TB_InternalOrder.Where(w => w.nYear + "" == sYear && (!string.IsNullOrEmpty(sCCID) ? w.sCostCenterID == sCCID : true) && (!string.IsNullOrEmpty(sIOID) ? w.sCostCenterID == sIOID : true)).ToList();
            var lstGL = db.TB_GLAccount.Where(w => w.nYear + "" == sYear).ToList();
            var lstMasterSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive).ToList();
            var lstObjective = lstMasterSub.Where(w => w.nMainID == 8).ToList();
            var lstArea = lstMasterSub.Where(w => w.nMainID == 6).ToList();
            var lstPA = lstMasterSub.Where(w => w.nMainID == 2).ToList();
            var lstProjectType = lstMasterSub.Where(w => w.nMainID == 7).ToList();

            var lstData = (from a in lstBudget
                           from b in lstGL.Where(w => w.sGLID == a.sGLID && w.sIOID == a.sIOID)
                           from c in lstObjective.Where(w => w.nSubID == a.nObjective).DefaultIfEmpty()
                           from d in lstArea.Where(w => w.nSubID == a.nArea)
                           from e in lstPA.Where(w => w.nSubID == a.nPA)
                           from f in lstProject.Where(w => w.nProjectID == a.nProjectID)
                           from g in lstIO.Where(w => w.sIOID == a.sIOID)
                           from h in lstCC.Where(w => w.sCostCenterID == g.sCostCenterID)
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
                               sCostCenter = h.sCostCenterID + " - " + h.sCostCenterName
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
    public class TReturnGetData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_data> lstCC { get; set; }
        public IEnumerable<c_data> lstIO { get; set; }
        public IEnumerable<c_data> lstGL { get; set; }
    }

    [Serializable]
    public class c_data
    {
        public int nYear { get; set; }
        public string sCCID { get; set; }
        public string sIOID { get; set; }
        public string sGLID { get; set; }
        public string sName { get; set; }
    }

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
        public string sCostCenter { get; set; }
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
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
            #endregion

            #region Header    
            SetTbl(ws1, nRow, 1, 11, true, hCenter, vCenter, true, null, 7, sColorHeadTb, "ลำดับ");
            SetTbl(ws1, nRow, 2, 11, true, hCenter, vCenter, true, null, 8, sColorHeadTb, "Period");
            SetTbl(ws1, nRow, 3, 11, true, hCenter, vCenter, true, null, 14, sColorHeadTb, "Posting Date");
            SetTbl(ws1, nRow, 4, 11, true, hCenter, vCenter, true, null, 40, sColorHeadTb, "ชื่อโครงการ");
            SetTbl(ws1, nRow, 5, 11, true, hCenter, vCenter, true, null, 40, sColorHeadTb, "Description");
            SetTbl(ws1, nRow, 6, 11, true, hCenter, vCenter, true, null, 15, sColorHeadTb, "Amount");
            SetTbl(ws1, nRow, 7, 11, true, hCenter, vCenter, true, null, 22, sColorHeadTb, "Pay To");
            SetTbl(ws1, nRow, 8, 11, true, hCenter, vCenter, true, null, 20, sColorHeadTb, "Cost Center");
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
            string sProjectName = hddProjectName.Value.ToLower().Replace(" ", "");
            string sYear = hddYear.Value;
            string sCCID = hddCCID.Value;
            string sIOID = hddIOID.Value;
            string sGLID = hddGLID.Value;

            var lstProject = db.T_Project.Where(w => !w.IsDel && (!string.IsNullOrEmpty(sYear) ? w.nYear + "" == sYear : true) &&
            (!string.IsNullOrEmpty(sProjectName) ? (w.sProjectCode.ToLower().Replace(" ", "").Contains(sProjectName) || w.sProjectName.ToLower().Replace(" ", "").Contains(sProjectName)) : true) &&
            (!string.IsNullOrEmpty(sYear) ? w.nYear + "" == sYear : true)).ToList();

            var lstProID = lstProject.Select(s => s.nProjectID).ToList();
            var lstBudget = db.TB_Budget_Sub.Where(w => lstProID.Contains(w.nProjectID) &&
            (!string.IsNullOrEmpty(sIOID) ? w.sIOID == sIOID : true) &&
            (!string.IsNullOrEmpty(sGLID) ? w.sGLID == sGLID : true)).ToList();

            var lstCC = db.TB_CostCenter.Where(w => w.nYear + "" == sYear && (!string.IsNullOrEmpty(sCCID) ? w.sCostCenterID == sCCID : true)).ToList();
            var lstIO = db.TB_InternalOrder.Where(w => w.nYear + "" == sYear && (!string.IsNullOrEmpty(sCCID) ? w.sCostCenterID == sCCID : true) && (!string.IsNullOrEmpty(sIOID) ? w.sCostCenterID == sIOID : true)).ToList();
            var lstGL = db.TB_GLAccount.Where(w => w.nYear + "" == sYear).ToList();
            var lstMasterSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && w.IsActive).ToList();
            var lstObjective = lstMasterSub.Where(w => w.nMainID == 8).ToList();
            var lstArea = lstMasterSub.Where(w => w.nMainID == 6).ToList();
            var lstPA = lstMasterSub.Where(w => w.nMainID == 2).ToList();
            var lstProjectType = lstMasterSub.Where(w => w.nMainID == 7).ToList();

            var lstData = (from a in lstBudget
                           from b in lstGL.Where(w => w.sGLID == a.sGLID && w.sIOID == a.sIOID)
                           from c in lstObjective.Where(w => w.nSubID == a.nObjective).DefaultIfEmpty()
                           from d in lstArea.Where(w => w.nSubID == a.nArea)
                           from ee in lstPA.Where(w => w.nSubID == a.nPA)
                           from f in lstProject.Where(w => w.nProjectID == a.nProjectID)
                           from g in lstIO.Where(w => w.sIOID == a.sIOID)
                           from h in lstCC.Where(w => w.sCostCenterID == g.sCostCenterID)
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
                               sCostCenter = h.sCostCenterID + " - " + h.sCostCenterName
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
                    SetTbl(ws1, nRow, 5, 11, false, hLeft, vTop, false, null, 40, null, item.sDescription);
                    SetTbl(ws1, nRow, 6, 11, false, hRight, vTop, false, null, 15, null, item.sValInRepCur);
                    SetTbl(ws1, nRow, 7, 11, false, hLeft, vTop, false, null, 22, null, item.sNameOffsetting);
                    SetTbl(ws1, nRow, 8, 11, false, hCenter, vTop, false, null, 20, null, item.sCostCenter);
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

            string sName = "Costcenter_trans_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

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