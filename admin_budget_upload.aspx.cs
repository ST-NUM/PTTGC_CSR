using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;

public partial class admin_budget_upload : System.Web.UI.Page
{
    private static int nMenuID = 10;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                SystemFunction.BindDdlPageSize(ddlPageSize);
                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
                hddUserID.Value = UserAccount.SessionInfo.nUserID + "";
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search()
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            var lstBudget = db.TB_Budget.OrderByDescending(o => o.dCreate).ToList();
            var lstData = (from a in lstBudget
                           from b in db.TB_User.Where(w => w.nUserID == a.nCreateBy).DefaultIfEmpty()
                           select new c_budget
                           {
                               nID = a.nID,
                               sFileName = a.sFilename,
                               nRow = a.nRow,
                               sUpdateDate = a.dCreate.ToString("dd/MM/yyyy"),
                               sUpdateBy = b != null ? b.sFirstname + " - " + b.sLastname : "",
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nID + ""))
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
    public static TReturnDataFile CheckFile(string sPath, string sSysFileName, string sFileName)
    {
        TReturnDataFile result = new TReturnDataFile();

        var lstBudget = new List<c_budgetfile>();
        var sPathFile = (HostingEnvironment.MapPath(sPath.Replace("..", "~")) + sSysFileName);

        if (File.Exists(sPathFile))
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            var lstIO = db.TB_InternalOrder.ToList();
            var lstGL = db.TB_GLAccount.ToList();
            var sColorError = XLColor.Yellow;
            var sColorDup = XLColor.RedPigment;
            var IsPassHead = true;

            //int nSheet = 1;
            var workbook = new XLWorkbook(sPathFile);
            //foreach (IXLWorksheet dataSheet in workbook.Worksheets)
            //{
            #region Sheet              
            //if (nSheet == 1)
            //{
            IXLWorksheet SheetItem = workbook.Worksheet(1);

            #region Define List
            var lstProject = db.Database.SqlQuery<c_project>("select nProjectID,sProjectCode,nYear,nProjectType,sIOID from T_Project where IsDel = 0 and IsPassApprove = 1").ToList();

            var lstMasterSub = db.TM_MasterData_Sub.Where(w => w.IsActive && !w.IsDel).ToList();
            var lstObjective = lstMasterSub.Where(w => w.nMainID == 8).ToList();
            var lstArea = lstMasterSub.Where(w => w.nMainID == 6).ToList();
            var lstPA = lstMasterSub.Where(w => w.nMainID == 2).ToList();
            var lstBudgetOld = db.TB_Budget_Sub.ToList();

            var lstMap = db.TB_Map_Budget.Where(w => !w.IsDel).ToList();
            var lstMapID = lstMap.Select(s => s.nID).ToList();
            var lstMapSub = db.TB_Map_Budget_Sub.Where(w => lstMapID.Contains(w.nID)).ToList();
            #endregion

            var IsOrderNotMatch = false;

            int nRow = 1;
            foreach (IXLRow row in SheetItem.Rows())
            {
                #region Row                       
                if (nRow == 1)
                {
                    #region Head
                    if (row.Cell(1).Value + "" != "Period" || row.Cell(2).Value + "" != "Order" || row.Cell(3).Value + "" != "Posting Date" ||
                        row.Cell(4).Value + "" != "Description" || row.Cell(5).Value + "" != "Val.in rep.cur." || row.Cell(6).Value + "" != "Name of offsetting account" ||
                        row.Cell(7).Value + "" != "Cost Element" || row.Cell(8).Value + "" != "Cost element name" || row.Cell(9).Value + "" != "Objective" ||
                        row.Cell(10).Value + "" != "Area" || row.Cell(11).Value + "" != "Internal" || row.Cell(12).Value + "" != "External" || row.Cell(13).Value + "" != "Philanthropic Activities")
                    {
                        if (row.Cell(1).Value + "" != "Period") row.Cell(1).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(2).Value + "" != "Order") row.Cell(2).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(3).Value + "" != "Posting Date") row.Cell(3).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(4).Value + "" != "Description") row.Cell(4).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(5).Value + "" != "Val.in rep.cur.") row.Cell(5).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(6).Value + "" != "Name of offsetting account") row.Cell(6).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(7).Value + "" != "Cost Element") row.Cell(7).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(8).Value + "" != "Cost element name") row.Cell(8).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(9).Value + "" != "Objective") row.Cell(9).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(10).Value + "" != "Area") row.Cell(10).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(11).Value + "" != "Internal") row.Cell(11).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(12).Value + "" != "External") row.Cell(12).Style.Fill.BackgroundColor = sColorError;
                        if (row.Cell(13).Value + "" != "Philanthropic Activities") row.Cell(13).Style.Fill.BackgroundColor = sColorError;

                        IsPassHead = false;
                        //result.Msg = "รูปแบบเอกสารไม่ถูกต้อง";
                        //result.Status = SystemFunction.process_Failed;
                        //workbook.SaveAs(HostingEnvironment.MapPath(sPath.Replace("..", "~")) + "Error_" + sSysFileName);
                        //return result;
                    }
                    nRow++;
                    #endregion
                }
                else
                {
                    #region Define Variable
                    string sPeriod = (row.Cell(1).Value + "").Trim().Replace(" ", "");
                    if (sPeriod == "") { break; }
                    string sIOID = (row.Cell(2).Value + "").Trim().Replace(" ", "");
                    string sPostingDate = (row.Cell(3).Value + "").Trim().Replace(" ", "");
                    string Description = row.Cell(4).Value + "";
                    string sValInRepCur = (row.Cell(5).Value + "").Trim().Replace(" ", "");
                    string sNameOffsetting = row.Cell(6).Value + "";
                    string sGLID = (row.Cell(7).Value + "").Trim().Replace(" ", "");
                    string sGLName = (row.Cell(8).Value + "").Trim().Replace(" ", "");
                    string sObjective = (row.Cell(9).Value + "").ToLower().Trim().Replace(" ", "");
                    string sArea = (row.Cell(10).Value + "").ToLower().Trim().Replace(" ", "");
                    string sInternal = row.Cell(11).Value + "";
                    string sExternal = row.Cell(12).Value + "";
                    string sPA = (row.Cell(13).Value + "").ToLower().Trim().Replace(" ", "");
                    #endregion

                    #region Check Data + Add Data to Class

                    var cBudget = new c_budgetfile();
                    var lstError = new List<string>();

                    cBudget.nRow = nRow;
                    cBudget.IsOrderNotMatch = false;

                    var nPeriod = CommonFunction.ParseIntNull(sPeriod);
                    if (nPeriod.HasValue) { cBudget.nPeriod = nPeriod; } else { row.Cell(1).Style.Fill.BackgroundColor = sColorError; lstError.Add("Period"); }

                    var sProjectCode = Description.Length >= 8 ? Description.Substring(1, 8) : Description;
                    var qPro = lstProject.FirstOrDefault(w => w.sProjectCode == sProjectCode);
                    var IsObjective = false;
                    if (qPro != null && Description.Length <= 400)
                    {
                        cBudget.nProjectID = qPro.nProjectID;
                        cBudget.sDescription = Description;
                        IsObjective = qPro.nProjectType == 22 || qPro.nProjectType == 24;
                    }
                    else { row.Cell(4).Style.Fill.BackgroundColor = sColorError; lstError.Add("Description"); }

                    var nYear = qPro != null ? qPro.nYear : 0;
                    var dPostingDate = CommonFunction.ConvertStringToDateTimeExcel(sPostingDate);
                    if (dPostingDate.HasValue && dPostingDate.Value.Year == nYear) { cBudget.dPostingDate = dPostingDate; } else { row.Cell(3).Style.Fill.BackgroundColor = sColorError; lstError.Add("Posting Date"); }

                    if (sIOID != "" && lstIO.Any(a => a.nYear == nYear && a.sIOID == sIOID))
                    {
                        cBudget.sIOID = sIOID;

                        if (qPro != null && qPro.sIOID != sIOID)
                        {
                            row.Cell(2).Style.Fill.BackgroundColor = sColorError;
                            cBudget.IsOrderNotMatch = IsOrderNotMatch = true;
                        }
                    }
                    else
                    {
                        row.Cell(2).Style.Fill.BackgroundColor = sColorError; lstError.Add("Order");
                    }

                    if (sGLID != "" && lstGL.Any(a => a.nYear == nYear && a.sIOID == sIOID && a.sGLID == sGLID))
                    {
                        cBudget.sGLID = sGLID;
                    }
                    else
                    {
                        row.Cell(7).Style.Fill.BackgroundColor = row.Cell(8).Style.Fill.BackgroundColor = sColorError;
                        lstError.Add("Cost Element");
                    }

                    var qMap = lstMap.FirstOrDefault(w => w.nYear == nYear);
                    var nID = qMap != null ? qMap.nID : 0;
                    var lstMapSub_ = lstMapSub.Where(w => w.nID == nID).ToList();

                    if (!lstMapSub_.Any(a => a.sIOID == sIOID && a.sGLID == sGLID))
                    {
                        row.Cell(2).Style.Fill.BackgroundColor = row.Cell(7).Style.Fill.BackgroundColor = row.Cell(8).Style.Fill.BackgroundColor = sColorError;//Order, GL
                        lstError.Add("ไม่พบ Order และ Cost Element ในการกำหนดประเภทงบประมาณรายปี");
                    }

                    var nValInRepCur = CommonFunction.ParseDecimalNull(sValInRepCur);
                    if (nValInRepCur.HasValue) { cBudget.nValInRepCur = nValInRepCur; } else { row.Cell(5).Style.Fill.BackgroundColor = sColorError; lstError.Add("Val.in rep.cur."); }

                    if (sNameOffsetting != "" && sNameOffsetting.Length <= 400) { cBudget.sNameOffsetting = sNameOffsetting; } else { row.Cell(6).Style.Fill.BackgroundColor = sColorError; lstError.Add("Name of offsetting account"); }

                    var qObj = (qPro != null && IsObjective && sObjective != "") ? lstObjective.FirstOrDefault(a => a.sName.ToLower().Replace(" ", "") == sObjective) : null;
                    if (qPro != null && (IsObjective ? qObj != null : true))
                    {
                        if (IsObjective)
                        {
                            cBudget.nObjective = qObj.nSubID;
                        }
                    }
                    else { row.Cell(9).Style.Fill.BackgroundColor = sColorError; lstError.Add("Objective"); }


                    var qArea = sArea != "" ? lstArea.FirstOrDefault(a => a.sName.ToLower().Replace(" ", "") == sArea) : null;
                    if (qArea != null) { cBudget.nArea = qArea.nSubID; } else { row.Cell(10).Style.Fill.BackgroundColor = sColorError; lstError.Add("Area"); }

                    if (sInternal != "" && sInternal.Length <= 250) { cBudget.sInternal = sInternal; } else { row.Cell(11).Style.Fill.BackgroundColor = sColorError; lstError.Add("Internal"); }

                    if (sExternal != "" && sExternal.Length <= 250) { cBudget.sExternal = sExternal; } else { row.Cell(12).Style.Fill.BackgroundColor = sColorError; lstError.Add("External"); }

                    var qPA = sPA != "" ? lstPA.FirstOrDefault(a => a.sName.ToLower().Replace(" ", "") == sPA) : null;
                    if (qPA != null) { cBudget.nPA = qPA.nSubID; } else { row.Cell(13).Style.Fill.BackgroundColor = sColorError; lstError.Add("Philanthropic Activities"); }
                    #endregion

                    #region Check Dup
                    var qDupOld = lstBudgetOld.Any(a => a.sIOID == sIOID && a.dPostingDate == dPostingDate && a.sDescription == Description && a.nValInRepCur == nValInRepCur);
                    if (qDupOld)
                    {
                        row.Cell(2).Style.Fill.BackgroundColor = sColorDup;
                        row.Cell(3).Style.Fill.BackgroundColor = sColorDup;
                        row.Cell(4).Style.Fill.BackgroundColor = sColorDup;
                        row.Cell(5).Style.Fill.BackgroundColor = sColorDup;
                        lstError.Add("ข้อมูลซ้ำ");
                    }
                    #endregion

                    cBudget.IsPass = cBudget.IsPassMatch = !lstError.Any();
                    cBudget.lstError = lstError;

                    lstBudget.Add(cBudget);
                    nRow++;
                }
                #endregion
            }
            //}
            //else { break; }
            #endregion
            //}

            if (lstBudget.Any())
            {
                #region Check Dup in file        
                foreach (var item in lstBudget)
                {
                    if (lstBudget.Any(a => a.nRow != item.nRow && a.sIOID == item.sIOID && a.dPostingDate == item.dPostingDate && a.sDescription == item.sDescription && a.nValInRepCur == a.nValInRepCur))
                    {
                        if (!item.lstError.Contains("ข้อมูลซ้ำ"))
                        {
                            var qRow = SheetItem.Row(item.nRow);
                            qRow.Cell(2).Style.Fill.BackgroundColor = sColorDup;
                            qRow.Cell(3).Style.Fill.BackgroundColor = sColorDup;
                            qRow.Cell(4).Style.Fill.BackgroundColor = sColorDup;
                            qRow.Cell(5).Style.Fill.BackgroundColor = sColorDup;

                            item.IsPassMatch = false;
                            item.lstError.Add("ข้อมูลซ้ำ");
                        }
                    }
                    item.sMsg = string.Join(", ", item.lstError);
                }
                #endregion

                if (IsOrderNotMatch)
                {
                    foreach (var item in lstBudget)
                    {
                        if (item.IsOrderNotMatch)
                        {
                            item.sMsg += (item.sMsg != "" ? ", " : "") + "Order ไม่ตรงกับ Project";
                            item.IsPassMatch = false;
                        }
                    }
                    workbook.SaveAs(HostingEnvironment.MapPath(sPath.Replace("..", "~")) + "Error_" + sSysFileName);
                }

                result.IsOrderNotMatch = IsOrderNotMatch;

                if (lstBudget.Any(a => !a.IsPass) || !IsPassHead)
                {
                    result.Msg = "ไม่สามารถบันทึกรายการได้";
                    result.Status = SystemFunction.process_Failed;
                    if (!IsOrderNotMatch) { workbook.SaveAs(HostingEnvironment.MapPath(sPath.Replace("..", "~")) + "Error_" + sSysFileName); }
                }
                else
                {
                    #region Save Data && Move File                   
                    int nUserID = UserAccount.SessionInfo.nUserID;
                    DateTime dNow = DateTime.Now;
                    string sUploadPath = "UploadFiles/" + nUserID + "/Budget/";

                    #region Save TB_Budget
                    var qMain = new TB_Budget();
                    qMain.sFilename = sFileName;
                    qMain.sSysFileName = sSysFileName;
                    qMain.sPath = sUploadPath;
                    qMain.nRow = lstBudget.Count;
                    qMain.nCreateBy = nUserID;
                    qMain.dCreate = dNow;
                    db.TB_Budget.Add(qMain);
                    db.SaveChanges();
                    #endregion

                    int nID = qMain.nID;

                    #region TB_Budget_Sub    
                    int nItem = 1;
                    foreach (var item in lstBudget)
                    {
                        db.TB_Budget_Sub.Add(new TB_Budget_Sub()
                        {
                            nID = nID,
                            nItem = nItem,
                            nProjectID = item.nProjectID.Value,
                            nPeriod = item.nPeriod.Value,
                            sIOID = item.sIOID,
                            dPostingDate = item.dPostingDate.Value,
                            sDescription = item.sDescription,
                            nValInRepCur = item.nValInRepCur.Value,
                            sNameOffsetting = item.sNameOffsetting,
                            sGLID = item.sGLID,
                            nObjective = item.nObjective.HasValue ? item.nObjective : null,
                            nArea = item.nArea.Value,
                            sInternal = item.sInternal,
                            sExternal = item.sExternal,
                            nPA = item.nPA.Value,
                            nUpdateBy = nUserID,
                            dUpdate = DateTime.Now
                        });

                        nItem++;
                    }
                    db.SaveChanges();
                    #endregion

                    #region Update Budget in Project
                    var lstProID = lstBudget.Select(s => s.nProjectID).Distinct().ToList();
                    foreach (var item in lstProID)
                    {
                        db.T_Project.Where(w => w.nProjectID == item).ForEach(f =>
                        {
                            f.nBudgetUsed = (f.nBudgetUsed ?? 0) + lstBudget.Where(w => w.nProjectID == item).Sum(s => s.nValInRepCur);
                        });
                    }
                    db.SaveChanges();
                    #endregion

                    #region Update Budget Used in TB_InternalOrder, TB_GLAccount
                    var lstYear = lstBudget.Select(s => s.dPostingDate.Value.Year).Distinct().ToList();
                    var lstIOID = lstBudget.Select(s => s.sIOID).Distinct().ToList();
                    var lstIOMaster = lstIO.Where(w => lstIOID.Contains(w.sIOID)).ToList();
                    foreach (var item in lstIOMaster)
                    {
                        #region IO
                        string sIOID = item.sIOID;

                        foreach (var nYear in lstYear)
                        {
                            var qIO = db.TB_InternalOrder.FirstOrDefault(w => w.sIOID == sIOID && w.nYear == nYear);
                            if (qIO != null)
                            {
                                var lstBudgetIO = lstBudget.Where(w => w.sIOID == sIOID && w.dPostingDate.Value.Year == nYear).ToList();
                                qIO.nBudgetUsed += lstBudgetIO.Sum(s => (s.nValInRepCur ?? 0));

                                #region GL
                                var lstGLID = lstBudgetIO.Select(s => s.sGLID).Distinct().ToList();
                                foreach (var sGLID in lstGLID)
                                {
                                    var qGL = db.TB_GLAccount.FirstOrDefault(w => w.sIOID == sIOID && w.sGLID == sGLID && w.nYear == nYear);
                                    if (qGL != null)
                                    {
                                        qGL.nBudgetUsed += lstBudgetIO.Where(w => w.sGLID == sGLID).Sum(s => (s.nValInRepCur ?? 0));
                                    }
                                }
                                #endregion                              
                            }
                        }
                        #endregion
                    }
                    db.SaveChanges();
                    #endregion

                    #region Update Budget Used in TB_CostCenter
                    foreach (var nYear in lstYear)
                    {
                        var lstCCID = lstIO.Where(w => lstIOID.Contains(w.sIOID)).Select(s => s.sCostCenterID).Distinct().ToList();
                        foreach (var sCCID in lstCCID)
                        {
                            var qCC = db.TB_CostCenter.FirstOrDefault(w => w.sCostCenterID == sCCID && w.nYear == nYear);
                            if (qCC != null)
                            {
                                qCC.nBudgetUsed += db.TB_InternalOrder.Where(w => w.nYear == nYear && w.sCostCenterID == sCCID).Sum(s => s.nBudgetUsed);
                            }
                        }
                    }
                    db.SaveChanges();
                    #endregion

                    #region Move File
                    SystemFunction.CheckPathAndMoveFile(sSysFileName, sFileName, sUploadPath, sPath.Replace("../", ""));
                    #endregion

                    #region Clear File in Temp
                    if (!IsOrderNotMatch)
                    {
                        string sMapPath = HttpContext.Current.Server.MapPath("./");
                        SystemFunction.RemoveFileAllInFolfer(sMapPath + "UploadFiles/" + nUserID + "/Temp/");
                    }
                    #endregion

                    result.Status = SystemFunction.process_Success;
                    #endregion
                }
            }
            else
            {
                result.Msg = "ไม่พบไฟล์";
                result.Status = SystemFunction.process_Failed;
            }
        }
        else
        {
            result.Msg = "ไม่พบไฟล์";
            result.Status = SystemFunction.process_Failed;
        }


        result.lstData = lstBudget;
        return result;
    }

    #region Class    
    [Serializable]
    public class TReturnDataFile : sysGlobalClass.CResutlWebMethod
    {
        public bool IsOrderNotMatch { get; set; }
        public IEnumerable<c_budgetfile> lstData { get; set; }
    }

    [Serializable]
    public class c_project
    {
        public int nProjectID { get; set; }
        public string sProjectCode { get; set; }
        public int nYear { get; set; }
        public int nProjectType { get; set; }
        public string sIOID { get; set; }
    }

    [Serializable]
    public class c_budgetfile
    {
        public int nRow { get; set; }
        public int? nProjectID { get; set; }
        public int? nPeriod { get; set; }
        public string sIOID { get; set; }
        public Nullable<System.DateTime> dPostingDate { get; set; }
        public string sDescription { get; set; }
        public Nullable<decimal> nValInRepCur { get; set; }
        public string sNameOffsetting { get; set; }
        public string sGLID { get; set; }
        public int? nObjective { get; set; }
        public Nullable<int> nArea { get; set; }
        public string sInternal { get; set; }
        public string sExternal { get; set; }
        public Nullable<int> nPA { get; set; }
        public bool IsPass { get; set; }
        public bool IsPassMatch { get; set; }
        public List<string> lstError { get; set; }
        public string sMsg { get; set; }
        public bool IsOrderNotMatch { get; set; }
    }

    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_budget> lstData { get; set; }
    }

    [Serializable]
    public class c_budget
    {
        public int nID { get; set; }
        public string sFileName { get; set; }
        public int nRow { get; set; }
        public string sUpdateDate { get; set; }
        public string sUpdateBy { get; set; }
        public string sIDEncrypt { get; set; }
    }
    #endregion
}