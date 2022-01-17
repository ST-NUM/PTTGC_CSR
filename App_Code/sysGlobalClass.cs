using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for sysGlobalClass
/// </summary>
public class sysGlobalClass
{
    public sysGlobalClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class CResutlWebMethod
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public string Content { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }
        public string Content3 { get; set; }
        public string Content4 { get; set; }
        public string Content5 { get; set; }
        public string[] ArrContent { get; set; }
        public string[] ArrContent1 { get; set; }
        public string[] ArrContent2 { get; set; }

    }
    public class TUserSmall
    {
        public string CODE { get; set; }
        public string INAME { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string FULLNAMETH { get; set; }
        public string EmailAbbr { get; set; }
        public string unitcode { get; set; }
        public string unitname { get; set; }
        public string longname { get; set; }
        public string unitabbr { get; set; }
        public int? site_id { get; set; }
        public decimal? location_id { get; set; }
        public string sPositionName { get; set; }
        public string PhoneDirect { get; set; }
        public string POSCODE { get; set; }
        public string REP_CODE { get; set; }
        public string EmailAddr { get; set; }
        public string JOBGROUP { get; set; }
    }
    public class FileUploadMaster
    {
        public int nID { get; set; }
        public string sPath { get; set; }
        public string sSysFileName { get; set; }
        public string sFileName { get; set; }
        public string sDescription { get; set; }
        public bool isDel { get; set; }
        public bool isNew { get; set; }
    }
    public class TBindDropdown
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string CeoInfo { get; set; }
        public string GroupDivisionInfo { get; set; }
        public string DivisionInfo { get; set; }
        public string DepartmentInfo { get; set; }
        public string SectionInfo { get; set; }
    }
    public class CT_Project
    {
        public int nProjectID { get; set; }
        public int nPhaseID { get; set; }
        public string sRefNo { get; set; }
        public string sGate { get; set; }
        public string sProjectName { get; set; }
        public string sProjectOwnerID { get; set; }
        public string sProjectOwnerName { get; set; }
        public int? nCurrentPhase { get; set; }
        public int nStatusID { get; set; }
        public int? nStepApprove { get; set; }
        public string sDeptABBR { get; set; }
        public string sDepartmentInfo { get; set; }
        public string sTechnicalDate { get; set; }
        public int? nRequestType { get; set; }
        public string sRequestTypeName { get; set; }
        public int? nResult { get; set; }
        public string sResult { get; set; }
        public int? nStatus { get; set; }
        public bool IsDisabled { get; set; }
        public bool? IsNonResult { get; set; }
        public string sNonResult { get; set; }
        public bool? IsDel { get; set; }
        public bool? IsNew { get; set; }
        public bool IsCanDel { get; set; }
        public string sFileName { get; set; }
        public string sSysFileName { get; set; }
        public string sPath { get; set; }
        public string sMeetingName { get; set; }
        public string sMeetingNo { get; set; }
        public string sMeetingDate { get; set; }
        public bool? IsHasResults { get; set; }
    }
}