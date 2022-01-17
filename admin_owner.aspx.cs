using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_owner : System.Web.UI.Page
{
    private static int nMenuID = 14;

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
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnGetData GetData()
    {
        c_ReturnGetData TReturn = new c_ReturnGetData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            var lstStatus = new List<int>() { 0, 1, 2, 4 };//บันทึก,อนุมัติ,ส่งกลับแก้ไข
            var lstPro = db.T_Project.Where(w => !w.IsDel && lstStatus.Contains(w.nStatusID)).ToList();
            var lstOwnerID = lstPro.Select(s => s.nOwnerID).Distinct().ToList();
            var lstOwner = db.TB_User.Where(w => lstOwnerID.Contains(w.nUserID)).ToList();

            var lstProjectName = (from a in lstPro
                                  from b in lstOwner.Where(w => w.nUserID == a.nOwnerID)
                                  select new c_project
                                  {
                                      nProjectID = a.nProjectID,
                                      sProjectName = a.sProjectName,
                                      sOwner = b.sFirstname + "  " + b.sLastname
                                  }).ToList();
            TReturn.lstProjectName = lstProjectName;
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnEmp GetEmp(string sSearch, int nProjectID)
    {
        c_ReturnEmp TReturn = new c_ReturnEmp();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            var qPro = db.T_Project.FirstOrDefault(w => w.nProjectID == nProjectID);
            if (qPro != null)
            {
                var sOrgID = qPro.sOrgID;
                var nOwnerOld = qPro.nOwnerID;

                var lstUser = db.TB_User.Where(w => w.IsActive && !w.IsDel && w.nRole == 2 && (w.nUserID != nOwnerOld) &&
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
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search()
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();

            var lstChange = db.TB_Change_Owner.ToList();
            var lstProID = lstChange.Select(s => s.nProjectID).Distinct().ToList();
            var lstProject = db.T_Project.Where(w => lstProID.Contains(w.nProjectID)).ToList();
            var lstID = lstChange.Select(s => s.nOwnerID).Distinct().ToList().Concat(lstChange.Select(s => s.nCreateBy).Distinct()).ToList();
            var lstUser = db.TB_User.Where(w => lstID.Contains(w.nUserID)).ToList();

            var lstData = (from a in lstChange
                           from b in lstUser.Where(w => w.nUserID == a.nOwnerID)
                           from c in lstProject.Where(w => w.nProjectID == a.nProjectID)
                           from d in lstUser.Where(w => w.nUserID == a.nCreateBy)
                           orderby a.dCreate descending
                           select new c_change
                           {
                               sProjectName = c.sProjectName,
                               sOwner = b.sFirstname + " " + b.sLastname,
                               sUpdate = d.sFirstname + " " + d.sLastname,
                               sUpdateDate = a.dCreate.ToString("dd/MM/yyyy")
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
    public static sysGlobalClass.CResutlWebMethod SaveData(int nProjectID, int nOwnerID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var qPro = db.T_Project.FirstOrDefault(w => w.nProjectID == nProjectID);
            if (qPro != null)
            {
                int nUserNew = nOwnerID;
                int nUserOld = (qPro.nOwnerID ?? 0);
                qPro.nOwnerID = nOwnerID;

                db.TB_Change_Owner.Add(new TB_Change_Owner()
                {
                    nProjectID = nProjectID,
                    nOwnerID = nOwnerID,
                    dCreate = DateTime.Now,
                    nCreateBy = UserAccount.SessionInfo.nUserID
                });

                db.SaveChanges();

                SendMailProject(qPro, nUserNew, nUserOld);
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = SystemFunction.sMsgSaveInNotStep;
                return result;
            }
        }
        return result;
    }

    public static void SendMailProject(T_Project qPro, int nNew, int nOld)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        string _to = "", _cc = "", subject = "", sTitleName = "", message = "", sText = "";
        string sTemplate = CSR_Function.GET_TemplateEmail();

        //Assign New Project Owner     
        var qUserNew = db.TB_User.FirstOrDefault(w => w.nUserID == nNew);
        _to = qUserNew != null ? qUserNew.sEmail : "";
        sTitleName = qUserNew != null ? CSR_Function.GetFirstNameNotAbbr(qUserNew.sFirstname) + " " + qUserNew.sLastname : "";

        //Send Mail CC to Old Owner and Clicker
        var lstCC = new List<string>();
        var qUserOld = db.TB_User.FirstOrDefault(w => w.nUserID == nOld);
        lstCC.Add(qUserOld != null ? qUserOld.sEmail : "");
        string sOldName = qUserOld != null ? CSR_Function.GetFirstNameNotAbbr(qUserOld.sFirstname) + " " + qUserOld.sLastname : "";

        int nUserID = UserAccount.SessionInfo.nUserID;
        var qUserClick = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID);
        lstCC.Add(qUserClick != null ? qUserClick.sEmail : "");
        _cc = string.Join(",", lstCC.Where(w => w != "").ToList());

        subject = "PTTGC-CSR | ผู้ดูแลระบบได้เพิ่มคุณเป็นผู้รับผิดชอบโครงการ";
        sText = "ผู้ดูแลระบบได้เปลี่ยนผู้รับผิดชอบโครงการจากคุณ " + sOldName + " เป็น คุณ " + sTitleName + " ในโครงการ " + qPro.sProjectName;

        string Applicationpath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath != "/" ? HttpContext.Current.Request.ApplicationPath + "/" : HttpContext.Current.Request.ApplicationPath);
        string sURL = Applicationpath + "AD/index.aspx?link=" + CommonFunction.Encrypt_UrlEncrypt("project_edit.aspx?str=" + CommonFunction.Encrypt_UrlEncrypt(qPro.nProjectID + ""));
        string sTRLink = @"<tr>
                            <td style='word-break: break-word; font-size: 0px;' align='center'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='border-collapse: separate' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='border: none; border-radius: 3px; color: white; padding: 15px 19px' align='center' valign='middle' bgcolor='#7289DA'><a href='" + sURL + @"' style='text-decoration: none; line-height: 100%; background: #7289da; color: white; font-family: Ubuntu,Helvetica,Arial,sans-serif; font-size: 15px; font-weight: normal; text-transform: none; margin: 0px' target='_blank'>คลิก</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>";

        message = string.Format(sTemplate,
                            "เรียน คุณ " + sTitleName,
                            sText,
                            sTRLink,
                            "",
                            "",
                            "");

        CSR_Function.SendNetMail("", _to, _cc, subject, message, new List<string>());
    }

    #region Class 
    [Serializable]
    public class c_ReturnGetData : sysGlobalClass.CResutlWebMethod
    {
        public List<c_project> lstProjectName { get; set; }
    }

    [Serializable]
    public class c_project
    {
        public int nProjectID { get; set; }
        public string sProjectName { get; set; }
        public string sOwner { get; set; }
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
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_change> lstData { get; set; }
    }

    [Serializable]
    public class c_change
    {
        public string sProjectName { get; set; }
        public string sOwner { get; set; }
        public string sUpdate { get; set; }
        public string sUpdateDate { get; set; }
    }
    #endregion
}