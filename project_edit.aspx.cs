using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassExecute;

public partial class project_edit : System.Web.UI.Page
{
    private static int nMenuID = 2;
    public string sPageType = "เพิ่ม";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                string str = Request.QueryString["str"];
                string sProjectID = hddProjectID.Value = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";

                SetControl(sProjectID);

                string sPer = hddPermission.Value = CSR_Function.IsApprover(null) ? "A" : SystemFunction.GetPMS(nMenuID);

                if (sProjectID != "")
                {
                    SetData(sProjectID, sPer);
                }
                else
                {
                    hddUserID.Value = UserAccount.SessionInfo.nUserID + "";
                    txtYear.Text = DateTime.Now.Year + "";
                    hddStatusID.Value = "0";
                }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
            }
        }
    }

    public void SetControl(string sProjectID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        CommonFunction.ListMonth(ddlMonthStart, "- เดือนเริ่มต้น -");
        CommonFunction.ListMonth(ddlMonthEnd, "- เดือนสิ้นสุด -");

        #region Project Type
        var lstProjectType = db.TM_MasterData_Sub.Where(w => w.nMainID == 7 && !w.IsDel && w.IsActive).ToList();
        ddlProjectType.DataSource = lstProjectType;
        ddlProjectType.DataValueField = "nSubID";
        ddlProjectType.DataTextField = "sName";
        ddlProjectType.DataBind();
        ddlProjectType.Items.Insert(0, new ListItem("- ประเภท  -", ""));
        #endregion

        #region Org
        var lstOrg = db.TM_Organization.OrderBy(o => o.sOrgNameAbbr).ToList();
        ddlUnit.DataSource = lstOrg;
        ddlUnit.DataValueField = "sOrgID";
        ddlUnit.DataTextField = "sOrgNameAbbr";
        ddlUnit.DataBind();
        ddlUnit.Items.Insert(0, new ListItem("- หน่วยงาน  -", ""));
        #endregion

        #region Order       
        int nYear = DateTime.Now.Year;
        if (sProjectID != "")
        {
            var qPro = db.T_Project.FirstOrDefault(w => w.nProjectID + "" == sProjectID);
            if (qPro != null) nYear = qPro.nYear;
        }
        var lstOrder = db.TB_InternalOrder.Where(w => w.nYear == nYear).OrderBy(o => o.sCostCenterID).ThenBy(o => o.sIOName).Select(s => new
        {
            sIOID = s.sIOID,
            sIOName = s.sIOID + " - " + s.sIOName
        }).ToList();

        ddlOrder.DataSource = lstOrder;
        ddlOrder.DataValueField = "sIOID";
        ddlOrder.DataTextField = "sIOName";
        ddlOrder.DataBind();
        ddlOrder.Items.Insert(0, new ListItem("- Order  -", ""));
        #endregion

        #region Dimension
        var lstDimension = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && w.IsActive && !w.IsDel).ToList();
        ddlDimension.DataSource = lstDimension;
        ddlDimension.DataValueField = "nSubID";
        ddlDimension.DataTextField = "sName";
        ddlDimension.DataBind();
        ddlDimension.Items.Insert(0, new ListItem("- CSR Dimension  -", ""));
        #endregion

        var IsAdmin = CSR_Function.IsAdmin(null);
        hddIsAdmin.Value = IsAdmin ? "Y" : "N";

        if (sProjectID == "" && CSR_Function.IsStaff(null))
        {
            int nUserID = UserAccount.SessionInfo.nUserID;
            var sUserID = CSR_Function.GetUserID(nUserID);
            var qEmp = HR_WebService.EmployeeService_EmployeeID(sUserID).d.results.FirstOrDefault();
            ddlUnit.SelectedValue = ddlUnit.SelectedValue = qEmp != null ? qEmp.OrgID : "";
            //ddlUnit.SelectedValue = "50000675";

            txtEmpID.Text = nUserID + "";
            txtEmpName.Text = UserAccount.SessionInfo.sName;

            hddIsOwner.Value = "Y";
        }
    }

    public void SetData(string sProjectID, string sPer)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qProject = db.T_Project.FirstOrDefault(w => w.nProjectID + "" == sProjectID);
        if (qProject != null)
        {
            #region ข้อมูลทั่วไป
            txtYear.Text = qProject.nYear + "";
            txtProjectCode.Text = qProject.sProjectCode + "";
            txtProjectName.Text = qProject.sProjectName;
            ddlProjectType.SelectedValue = qProject.nProjectType.HasValue ? qProject.nProjectType.Value + "" : "";
            ddlUnit.SelectedValue = qProject.sOrgID;
            ddlOrder.SelectedValue = qProject.sIOID;
            ddlMonthStart.SelectedValue = qProject.nMonthStart + "";
            ddlMonthEnd.SelectedValue = qProject.nMonthEnd + "";
            txtBudget.Text = qProject.nBudget + "";

            txtInternalD.Text = qProject.sInternalD;
            txtExternalD.Text = qProject.sExternalD;
            txtObjectiveD.Text = qProject.sObjectiveD;

            int? nOwnerID = qProject.nOwnerID;
            var qOwner = db.TB_User.FirstOrDefault(w => w.nUserID == nOwnerID);
            txtEmpName.Text = qOwner != null ? qOwner.sFirstname + "  " + qOwner.sLastname : "";
            txtEmpID.Text = nOwnerID.HasValue ? nOwnerID.Value + "" : "";
            #endregion

            #region รายละเอียดเพิ่มเติม
            txtObjective.Text = qProject.sObjective;
            txtBenefit.Text = qProject.sBenefit;
            txtRemark.Text = qProject.sRemark;
            #endregion

            hddUserID.Value = qProject.nCreateBy + "";

            var nUserID = UserAccount.SessionInfo.nUserID;
            hddStatusID.Value = qProject.nStatusID + "";

            var IsOwner = qProject.nOwnerID == nUserID;
            hddIsOwner.Value = IsOwner ? "Y" : "N";

            var IsApprover = qProject.sApproverID == CSR_Function.GetUserID(nUserID);
            hddIsApprover.Value = IsApprover ? "Y" : "N";

            var IsAdmin = hddIsAdmin.Value == "Y";

            hddPassApprove.Value = qProject.IsPassApprove ? "Y" : "N";

            var hasPerAll = sPer == "A";
            sPageType = "ดูรายละเอียด";
            if (hasPerAll)
            {
                switch (qProject.nStatusID)
                {
                    case 0: if (IsOwner || IsAdmin) { sPageType = "แก้ไข"; } break;//บันทึก
                    case 1: if (IsApprover) { sPageType = "อนุมัติ"; } break;//ส่งอนุมัติ
                    case 2: if (IsOwner) { sPageType = "แก้ไข"; } break;//อนุมัติ
                    case 4: if (IsOwner || IsAdmin) { sPageType = "แก้ไข"; } break;//ส่งกลับแก้ไข
                    default: sPageType = "ดูรายละเอียด"; break;
                }
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnEmp GetEmp(string sSearch, string sOrgID)
    {
        c_ReturnEmp TReturn = new c_ReturnEmp();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            var lstUser = db.TB_User.Where(w => w.IsActive && !w.IsDel && w.nRole == 2 &&
                            (w.sUserID.ToLower().Replace(" ", "").Contains(sSearch) || (w.sFirstname + w.sLastname).ToLower().Replace(" ", "").Contains(sSearch))).ToList();

            var lstEmpID = new List<string>();
            if (lstUser.Any())
            {
                lstEmpID = HR_WebService.GetEmployeeInOrg(string.Join(",", lstUser.Select(s => s.sUserID).ToList()), sOrgID).d.results.Select(s => s.EmployeeID).ToList();
            }

            TReturn.lstData = lstUser.Where(w => lstEmpID.Contains(w.sUserID))
                            .Select(s => new c_User
                            {
                                nUserID = s.nUserID,
                                sName = s.sFirstname + "  " + s.sLastname
                            }).ToList();
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnGetData GetData(string sProjectID, int nYear)
    {
        c_ReturnGetData TReturn = new c_ReturnGetData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            var qPro = db.T_Project.FirstOrDefault(w => w.nProjectID + "" == sProjectID);
            if (qPro != null)
            {
                TReturn.sCostCenterID = qPro.sCostCenterID;
                TReturn.sDimensionID = qPro.nDimensionID + "";
                TReturn.sDimensionSubID = qPro.nDimensionSubID + "";

                #region File
                TReturn.lstFilePic = db.T_Project_File.Where(w => w.nProjectID + "" == sProjectID && w.IsPic).AsEnumerable().Select(s => new CData_File
                {
                    nID = s.nItem,
                    sFilename = s.sFilename,
                    sSysFileName = s.sSysFileName,
                    sPath = s.sPath,
                    nUserID = s.nUpdateBy,
                    sUpdate = s.dUpdate.ToString("dd/MM/yyyy HH:mm น.")
                }).ToList();

                TReturn.lstFileOther = db.T_Project_File.Where(w => w.nProjectID + "" == sProjectID && !w.IsPic).AsEnumerable().Select(s => new CData_File
                {
                    nID = s.nItem,
                    sFilename = s.sFilename,
                    sSysFileName = s.sSysFileName,
                    sPath = s.sPath,
                    nUserID = s.nUpdateBy,
                    sUpdate = s.dUpdate.ToString("dd/MM/yyyy HH:mm น.")
                }).ToList();
                #endregion

                #region Log
                var lstApp = db.T_Project_Approve.Where(w => w.nProjectID + "" == sProjectID).OrderByDescending(o => o.dAction).ToList();
                var lstUserID = lstApp.Select(s => s.nActionBy).Distinct().ToList();
                var lstUser = db.TB_User.Where(w => lstUserID.Contains(w.nUserID)).ToList();
                var lstStatus = db.TM_ProjectStatus.ToList();

                var lstLog = (from a in lstApp
                              from b in lstStatus.Where(w => w.nStatusID == a.nStatusID)
                              from c in lstUser.Where(w => w.nUserID == a.nActionBy).DefaultIfEmpty()
                              select new c_Log
                              {
                                  sAction = b.sStatusName,
                                  sActionBy = c != null ? (c.sFirstname + "  " + c.sLastname) : (a.nActionBy == 0 ? "ระบบ" : ""),
                                  sActionDate = a.dAction.Value.ToString("dd/MM/yyyy <br> HH:mm น."),
                                  sComment = a.sComment
                              }).ToList();

                TReturn.lstLog = lstLog;
                #endregion
            }

            TReturn.lstProjectName = db.T_Project.Where(w => !w.IsDel).Select(s => s.sProjectName).ToList();

            TReturn.lstCostCenter = db.TB_CostCenter.Where(w => w.nYear == nYear).Select(s => new c_Dropdown
            {
                sMainID = s.sCostCenterID,
                sName = s.sCostCenterName
            }).ToList();

            TReturn.lstDimensionSub = db.TM_MasterData_Sub.Where(w => (w.nMainID == 4 || w.nMainID == 5) && !w.IsDel && w.IsActive).Select(s => new c_Dropdown
            {
                sMainID = (s.nMainID + 5) + "",
                sSubID = s.nSubID + "",
                sName = s.sName
            }).ToList();
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(SaveDataItem itemSave, int nStatusID, List<CData_File> lstFilePic, List<CData_File> lstFileOther)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var qDupName = db.T_Project.FirstOrDefault(w => !w.IsDel && w.nYear == itemSave.nYear && w.sProjectName == itemSave.sProjectName && (itemSave.nProjectID > 0 ? w.nProjectID != itemSave.nProjectID : true) && (itemSave.nProjectType.HasValue ? w.nProjectType == itemSave.nProjectType : true));//&&(itemSave.IsDonation ? w.sOrgID == itemSave.sOrgID : true)

            //var qDupDonation = itemSave.IsDonation ? db.T_Project.FirstOrDefault(w => !w.IsDel && w.nYear == itemSave.nYear &&
            //                    (itemSave.nProjectID > 0 ? w.nProjectID != itemSave.nProjectID : true) &&
            //                     w.sOrgID == itemSave.sOrgID && w.nDimensionID == itemSave.nDimensionID && w.nDimensionSubID == itemSave.nDimensionSubID) : null;
            if (qDupName == null)//&& qDupDonation == null
            {
                var nUserThis = UserAccount.SessionInfo.nUserID;
                var IsAdmin = CSR_Function.IsAdmin(nUserThis);
                var qPro = db.T_Project.FirstOrDefault(w => w.nProjectID == itemSave.nProjectID && !w.IsDel);

                string sOwnerID_Old = "";

                #region Add/Update T_Project          
                if (qPro == null)
                {
                    qPro = new T_Project();
                    qPro.nYear = itemSave.nYear;
                    qPro.IsPassApprove = false;
                    qPro.IsCreateByAdmin = IsAdmin;
                    qPro.nCreateBy = nUserThis;
                    qPro.dCreate = DateTime.Now;
                    db.T_Project.Add(qPro);
                }
                else
                {
                    sOwnerID_Old = qPro.nOwnerID + "";

                    var lstStatusSave = new List<int>() { 0, 1, 2, 4 };
                    if (!lstStatusSave.Contains(qPro.nStatusID) || qPro.IsDel || qPro.nStatusID != nStatusID)
                    {
                        result.Status = SystemFunction.process_SaveFail;
                        result.Msg = SystemFunction.sMsgSaveInNotStep;
                        return result;
                    }
                }

                var IsDonation = (itemSave.nProjectType ?? 0) == 22;

                qPro.sProjectName = itemSave.sProjectName;
                qPro.nProjectType = itemSave.nProjectType;
                qPro.sOrgID = itemSave.sOrgID;
                qPro.sCostCenterID = itemSave.sCostCenterID;
                qPro.sIOID = itemSave.sIOID;
                qPro.nMonthStart = itemSave.nMonthStart;
                qPro.nMonthEnd = itemSave.nMonthEnd;
                qPro.nBudget = itemSave.nBudget;
                qPro.nDimensionID = itemSave.nDimensionID;
                qPro.nDimensionSubID = itemSave.nDimensionSubID;
                qPro.sInternalD = IsDonation ? itemSave.sInternalD : "";
                qPro.sExternalD = IsDonation ? itemSave.sExternalD : "";
                qPro.sObjectiveD = IsDonation ? itemSave.sObjectiveD : "";
                qPro.nOwnerID = itemSave.nOwnerID;

                qPro.sObjective = itemSave.sObjective;
                qPro.sBenefit = itemSave.sBenefit;
                qPro.sRemark = itemSave.sRemark;
                qPro.nStatusID = itemSave.nSaveType;

                qPro.sApproverID = itemSave.nOwnerID.HasValue && itemSave.nSaveType == 1 ? CSR_Function.GetAppover(itemSave.nOwnerID.Value) : "";

                qPro.IsDel = false;
                qPro.nUpdateBy = nUserThis;
                qPro.dUpdate = DateTime.Now;

                db.SaveChanges();
                #endregion

                #region File

                int nProjectID = qPro.nProjectID;
                int sUserIDSave = qPro.nCreateBy;
                string sMapPath = HttpContext.Current.Server.MapPath("./");

                #region Clear File in Table
                string sDel = "DELETE T_Project_File WHERE nProjectID=" + nProjectID;
                db.Database.ExecuteSqlCommand(sDel);
                #endregion

                #region Add File Pic
                string sUploadPath = "UploadFiles/" + sUserIDSave + "/Project/" + nProjectID + "/Pic/";

                int nFilePic = 1;
                foreach (var itemFile in lstFilePic)
                {
                    SystemFunction.CheckPathAndMoveFile(itemFile.sSysFileName, itemFile.sFilename, sUploadPath, (itemFile.sPath).Replace("../", ""));

                    var tb_File = new T_Project_File();
                    tb_File.nProjectID = nProjectID;
                    tb_File.nItem = nFilePic;

                    tb_File.sPath = sUploadPath;
                    tb_File.sSysFileName = itemFile.sSysFileName;
                    tb_File.sFilename = itemFile.sFilename;
                    tb_File.IsPic = true;
                    tb_File.nUpdateBy = itemFile.nUserID ?? 0;
                    tb_File.dUpdate = CommonFunction.ConvertStringToDateTime(itemFile.sUpdate.Split(' ')[0], itemFile.sUpdate.Split(' ')[1].Replace(":", "."));

                    db.T_Project_File.Add(tb_File);
                    db.SaveChanges();

                    nFilePic++;
                }
                #endregion

                #region Add File Other
                sUploadPath = "UploadFiles/" + sUserIDSave + "/Project/" + nProjectID + "/Other/";

                int nFileOther = 1;
                foreach (var itemFile in lstFileOther)
                {
                    SystemFunction.CheckPathAndMoveFile(itemFile.sSysFileName, itemFile.sFilename, sUploadPath, (itemFile.sPath).Replace("../", ""));

                    var tb_File = new T_Project_File();
                    tb_File.nProjectID = nProjectID;
                    tb_File.nItem = nFileOther;

                    tb_File.sPath = sUploadPath;
                    tb_File.sSysFileName = itemFile.sSysFileName;
                    tb_File.sFilename = itemFile.sFilename;
                    tb_File.IsPic = false;
                    tb_File.nUpdateBy = itemFile.nUserID ?? 0;
                    tb_File.dUpdate = CommonFunction.ConvertStringToDateTime(itemFile.sUpdate.Split(' ')[0], itemFile.sUpdate.Split(' ')[1].Replace(":", "."));

                    db.T_Project_File.Add(tb_File);
                    db.SaveChanges();

                    nFileOther++;
                }

                #endregion

                #region Clear File in Temp
                SystemFunction.RemoveFileAllInFolfer(sMapPath + "UploadFiles/" + sUserIDSave + "/Temp/");
                #endregion

                #endregion

                if (itemSave.nSaveType == 0 && itemSave.nOwnerID.HasValue && sOwnerID_Old != itemSave.nOwnerID + "" && IsAdmin)
                {
                    //Send Mail to Project Owner
                    CSR_Function.SendMailProject(0, qPro);
                }

                if (itemSave.nSaveType == 1)
                {
                    //Send Mail to Approver        
                    CSR_Function.SendMailProject(1, qPro);
                }
                else if (itemSave.nSaveType == 6)
                {
                    //ระงับโครงการ
                    CSR_Function.SendMailProject(6, qPro);
                }

                CSR_Function.SetLogProject(nProjectID, (itemSave.nSaveType == 2 ? 0 : itemSave.nSaveType), (itemSave.nSaveType == 6 ? itemSave.sComment : ""));
            }
            else
            {
                result.Status = SystemFunction.process_Duplicate;
            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ApproveData(int nProjectID, string sComment, int nSaveType)
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
            var sUserThis = CSR_Function.GetUserID(nUserThis);
            var qPro = db.T_Project.FirstOrDefault(w => w.nProjectID == nProjectID && !w.IsDel && w.nStatusID == 1 && w.sApproverID == sUserThis);
            if (qPro != null)
            {
                qPro.nStatusID = nSaveType;
                qPro.nUpdateBy = nUserThis;
                qPro.dUpdate = DateTime.Now;

                if (string.IsNullOrEmpty(qPro.sProjectCode) && nSaveType == 2)
                {
                    qPro.IsPassApprove = true;

                    string sProjectType = "";
                    switch (qPro.nProjectType)
                    {
                        case 22: sProjectType = "DN"; break;
                        case 23: sProjectType = "PJ"; break;
                        case 24: sProjectType = "PR"; break;
                        case 25: sProjectType = "KV"; break;
                        case 26: sProjectType = "VT"; break;
                        default:
                            break;
                    }
                    db.Database.ExecuteSqlCommand("EXEC dbo.CreateProjectCode @nProjectID=" + nProjectID + ",@sProjectType='" + sProjectType + "',@nProjectType=" + qPro.nProjectType);
                }

                db.SaveChanges();

                CSR_Function.SendMailProject(nSaveType, qPro);

                CSR_Function.SetLogProject(nProjectID, nSaveType, sComment);
            }
            else
            {
                result.Status = SystemFunction.process_SaveFail;
                result.Msg = SystemFunction.sMsgSaveInNotStep;
            }
        }
        return result;
    }

    #region Class
    [Serializable]
    public class c_ReturnGetData : sysGlobalClass.CResutlWebMethod
    {
        public List<string> lstProjectName { get; set; }
        public string sCostCenterID { get; set; }
        //public string sOrderID { get; set; }
        public List<c_Dropdown> lstCostCenter { get; set; }
        //public List<c_Dropdown> lstOrder { get; set; }

        public string sDimensionID { get; set; }
        public string sDimensionSubID { get; set; }
        public List<c_Dropdown> lstDimensionSub { get; set; }

        public List<CData_File> lstFilePic { get; set; }
        public List<CData_File> lstFileOther { get; set; }

        public List<c_Log> lstLog { get; set; }
    }

    [Serializable]
    public class c_Dropdown
    {
        public string sMainID { get; set; }
        public string sSubID { get; set; }
        public string sName { get; set; }
    }

    [Serializable]
    public class c_ReturnEmp : sysGlobalClass.CResutlWebMethod
    {
        public List<c_User> lstData { get; set; }
    }

    [Serializable]
    public class c_User
    {
        public int nUserID { get; set; }
        public string sName { get; set; }
    }

    [Serializable]
    public class c_Log
    {
        public string sAction { get; set; }
        public string sActionBy { get; set; }
        public string sActionDate { get; set; }
        public string sComment { get; set; }
    }

    [Serializable]
    public class SaveDataItem
    {
        public int nProjectID { get; set; }
        public string sProjectName { get; set; }
        public int nYear { get; set; }
        public int? nProjectType { get; set; }
        public string sOrgID { get; set; }
        public string sCostCenterID { get; set; }
        public string sIOID { get; set; }
        public int? nMonthStart { get; set; }
        public int? nMonthEnd { get; set; }
        public decimal? nBudget { get; set; }
        public int? nDimensionID { get; set; }
        public int? nDimensionSubID { get; set; }
        public string sInternalD { get; set; }
        public string sExternalD { get; set; }
        public string sObjectiveD { get; set; }
        public int? nOwnerID { get; set; }

        public string sObjective { get; set; }
        public string sBenefit { get; set; }
        public string sRemark { get; set; }

        public int nSaveType { get; set; }
        public string sComment { get; set; }
    }
    #endregion
}