using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SAPnBPC_Interface
/// </summary>
public class SAPnBPC_Interface
{
    //#region Data
    private string _SAP_ASYNCOUTMODE;
    private string _SAPClientUserName;
    private string _SAPClientPassword;
    private string _BPC_ASYNCOUTMODE;
    private string _BPCClientUserName;
    private string _BPCClientPassword;
    private string _ORDERNO;
    private string _SYSSTATUSACTION;
    private string _UserAction;
    //#endregion
    //#region Property
    public string SAP_ASYNCOUTMODE
    {
        get { return ConfigurationSettings.AppSettings["SAP_SYNC_IN"] + ""; }
        set { _SAP_ASYNCOUTMODE = value; }
    }
    public string SAPClientUserName
    {
        get { return ConfigurationSettings.AppSettings["SAP_SYNCIN_USR"] + ""; }
        set { _SAPClientUserName = value; }
    }
    public string SAPClientPassword
    {
        get { return ConfigurationSettings.AppSettings["SAP_SYNCIN_PWD"] + ""; }
        set { _SAPClientPassword = value; }
    }
    public string BPC_ASYNCOUTMODE
    {
        get { return ConfigurationSettings.AppSettings["BPC_SYNC_IN"] + ""; }
        set { _BPC_ASYNCOUTMODE = value; }
    }
    public string BPCClientUserName
    {
        get { return ConfigurationSettings.AppSettings["BPC_SYNCIN_USR"] + ""; }
        set { _BPCClientUserName = value; }
    }
    public string BPCClientPassword
    {
        get { return ConfigurationSettings.AppSettings["BPC_SYNCIN_PWD"] + ""; }
        set { _BPCClientPassword = value; }
    }
    public string UserAction
    {

        get { return HttpContext.Current.Session["EmployeeID"] + ""; }
        set { _UserAction = value; }

    }
    //#endregion
    bool IsNullOrEmpty(string _val)
    {
        return string.IsNullOrEmpty(_val);
    }
    public bool CONVBOOL(string _bool)
    {
        bool defBool;
        return bool.TryParse(_bool, out defBool) ? defBool : false;
    }
    public decimal CONVDEC(string _val)
    {
        decimal defValue;
        return decimal.TryParse(_val, out defValue) ? defValue : 0;
    }
    public decimal? CONVDEC(string _val, object output)
    {
        decimal defValue;
        return decimal.TryParse(_val, out defValue) ? defValue : (decimal?)output;
    }
    public byte CONVBYTE(string _val)
    {
        byte defValue;
        return byte.TryParse(_val, out defValue) ? defValue : (byte)0;
    }
    string NewChar(string _val, int _lenght, char? Pfix)
    {
        string[] Injtion = { "<", ">", "	" };
        foreach (string chars in Injtion) { _val = _val.Replace(chars, ""); }

        //string _txt = new System.Text.StringBuilder(((Pfix.HasValue) ? _val.PadLeft(_lenght, Pfix.Value) : _val), _lenght).ToString();
        string _txt = ((Pfix.HasValue) ? _val.PadLeft(_lenght, Pfix.Value) : _val);
        _txt = (_txt.Length > _lenght) ? _txt.Substring(0, _lenght) : _txt;
        return _txt;
    }
    string SplitString(string _val, char __char, int _index)
    {
        //if (_val.IndexOf(__char) > -1)
        //{
        string[] _txt = _val.Split(__char);
        //}
        return _txt.Length > 1 ? _txt[(_txt.Length > _index ? _index : 0)] + "" : _val;
    }
    string IsWarning(string _val, string _type, string _Msg)
    {
        bool _IsWrn = false;
        switch (_type)
        {
            case "W":
                string[] arayW_MsgCde = { "604", "605", "606", "607", "608", "611", "612", "613", "614" };
                _IsWrn = Array.IndexOf(arayW_MsgCde, _val) > -1;
                _Msg = _IsWrn ? _Msg : "";
                break;
            case "S":
                _Msg = "";
                break;
            case "E":
            default:
                _Msg = _Msg;
                break;
        }


        return _Msg;
    }
    public static string IsWarningMSG(string _val, string _type, string _Msg)
    {
        return new SAPnBPC_Interface().IsWarning("" + _val, "" + _type, "" + _Msg);
    }
    public SAPnBPC_Interface()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Client(string PipeLine, string UserID, string CompCode)
    {
        string RFCuser = "";
        //expenseEntities db = new expenseEntities();
        /* 
         */
        //bool IsOverSea = db.MT_RFCUser.Any(w => w.UserID == UserID);
        switch (PipeLine)
        {
            case "ZSO_BUDGET_CHECK":
                RFCuser = "HTTP_Port";
                break;
            case "ZSO_BUDGET_CHECK_GL":
                RFCuser = "HTTP_Port1";
                break;
        }
        return RFCuser;
    }

    #region Class
    /// <summary>
    /// Class eXpense Request mode
    /// </summary> 
    public class BudgetExcellence_Req_Mode
    {
        public bool TestMode { get; set; }
    }
    /// <summary>
    /// Class  Request Data I001
    /// </summary> 
    public class Plan_Req_Data
    {
        public bool TestFrist { get; set; }
        //public eXpense_Req_Head[] IM_HEAD { get; set; }
        public input ZBPC_I010 { get; set; }
        public item[] IT_BDGTPLAN { get; set; }
    }
    public class Plan_Response_Log
    {

        public string ZSO_BDGTPLAN_LOG { get; set; }
        public string MANDT { get; set; }
        public string WEB_INDEX { get; set; }
        public string SAP_COUNT { get; set; }
        public string TRANS_TYPE { get; set; }
        public string ORD_AUFNR { get; set; }
        public string ORD_AUART { get; set; }
        public string FYEAR { get; set; }
        public string BUD { get; set; }
        public string MSG { get; set; }
        public string S_DATE { get; set; }
        public string S_TIME { get; set; }
    }
    public class item
    {
        public double ItemOrder { get; set; }
        /// <summary>
        /// Web-TRANS_TYPE:	"Budget Transaction type:
        /// BGTO: Transfer budget To
        ///BGFR: Transfer budget From" 
        /// </summary>
        public string BudgetTransactionType { get; set; }
        /// <summary>
        /// Web-ORD_AUART: Internal order – Order type(ZSO_IO - AUART)
        /// </summary>
        public string InternalOrderNo { get; set; }

        /// <summary>
        /// Web-ORD_OBJNR	Internal order – Object number(ZSO_IO - OBJNR)
        /// </summary>
        public string InternalOrderID { get; set; }

        /// <summary>
        /// Web-ORD_AUART	Internal order – Order type(ZSO_IO - AUART)
        /// </summary>
        public string InternalOrderType { get; set; }

        /// <summary>
        /// Web-KOSTL	Cost center number(ZSO_IO - KOSTV, ZSO_PSOP - AKSTL)
        /// </summary>
        public string CostCenterNo { get; set; }

        /// <summary>
        /// Web-APPR_POSNR 	Appropriation Request - internal Number(ZSO_APPREQ - POSNR)
        /// </summary>
        public string AppNo { get; set; }

        /// <summary>
        /// Web-APPR_POSID	Appropriation Request Number("ZSO_APPREQ - POSID, ZSO_PSCP - POSID2, ZSO_PSOP - POSID2")
        /// </summary>
        public string AppPosID { get; set; }

        /// <summary>
        /// Web-APPR_OBJNR	Appropriation request - Object number (IQ*)(ZSO_APPREQ - OBJNR)
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// Web-APPR_IVART	Appropriation request type(ZSO_APPREQ - IVART)
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// Web-IM_PRNAM	Investment Program name(ZSO_APPREQ - PRNAM,ZSO_PSCP - PRNAM,ZSO_PSOP - PRNAM)
        /// </summary>
        public string InvestNo { get; set; }

        /// <summary>
        /// Web-IM_POSID	Investment Program position ID(ZSO_APPREQ - POSID2,ZSO_PSCP - POSID3,ZSO_PSOP - POSID3)
        /// </summary>
        public string InvestPosID { get; set; }

        /// <summary>
        /// Web-IM_GJAHR	Investment Program Approval year("ZSO_APPREQ - GJAHR2,ZSO_PSCP - GJAHR,ZSO_PSOP - GJAHR2")
        /// </summary>
        public string InvestYear { get; set; }

        /// <summary>
        /// Web-WBS_POSID	WBS("ZSO_PSCP - POSID,ZSO_PSOP - POSID")
        /// </summary>
        public string WBSNo { get; set; }

        /// <summary>
        /// Web-WBS_PSPID	Project Definition number(ZSO_PSCP - POSID)
        /// </summary>
        public string ProjNo { get; set; }

        /// <summary>
        /// Web-WBS_OBJNR	WBS object number start with PR*("ZSO_PSCP - OBJNR,ZSO_PSOP - OBJNR")
        /// </summary>
        public string WBSID { get; set; }

        /// <summary>
        /// Web-WBS_PRART	WBS type("ZSO_PSCP - PRART,ZSO_PSOP - PRART")
        /// </summary>
        public string WBSType { get; set; }

        /// <summary>
        /// Web-KSTAR	G/L Account(ZSO_CELEM - SAKNR)
        /// </summary>
        public string GLAccount { get; set; }

        /// <summary>
        /// Web-FYEAR	"Fiscal year - Budget or Fiscal year - Plan"()
        /// </summary>
        public string FiscalYear { get; set; }

        /// <summary>
        /// Web-BUDGET_AMT	Budget value()
        /// </summary>
        public string TotalPlanValue { get; set; }


        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue01 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue02 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue03 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue04 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue05 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue06 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue07 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue08 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue09 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue10 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue11 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue12 { get; set; }

        /// <summary>
        /// Web-Currentcy
        /// </summary>
        public string Currentcy { get; set; }
    }
    public class Plan_Req_Items
    {
        public double ItemOrder { get; set; }
        /// <summary>
        /// Web-TRANS_TYPE:	"Budget Transaction type:
        /// BGTO: Transfer budget To
        ///BGFR: Transfer budget From" 
        /// </summary>
        public string BudgetTransactionType { get; set; }
        /// <summary>
        /// Web-ORD_AUART: Internal order – Order type(ZSO_IO - AUART)
        /// </summary>
        public string InternalOrderNo { get; set; }

        /// <summary>
        /// Web-ORD_OBJNR	Internal order – Object number(ZSO_IO - OBJNR)
        /// </summary>
        public string InternalOrderID { get; set; }

        /// <summary>
        /// Web-ORD_AUART	Internal order – Order type(ZSO_IO - AUART)
        /// </summary>
        public string InternalOrderType { get; set; }

        /// <summary>
        /// Web-KOSTL	Cost center number(ZSO_IO - KOSTV, ZSO_PSOP - AKSTL)
        /// </summary>
        public string CostCenterNo { get; set; }

        /// <summary>
        /// Web-APPR_POSNR 	Appropriation Request - internal Number(ZSO_APPREQ - POSNR)
        /// </summary>
        public string AppNo { get; set; }

        /// <summary>
        /// Web-APPR_POSID	Appropriation Request Number("ZSO_APPREQ - POSID, ZSO_PSCP - POSID2, ZSO_PSOP - POSID2")
        /// </summary>
        public string AppPosID { get; set; }

        /// <summary>
        /// Web-APPR_OBJNR	Appropriation request - Object number (IQ*)(ZSO_APPREQ - OBJNR)
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// Web-APPR_IVART	Appropriation request type(ZSO_APPREQ - IVART)
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// Web-IM_PRNAM	Investment Program name(ZSO_APPREQ - PRNAM,ZSO_PSCP - PRNAM,ZSO_PSOP - PRNAM)
        /// </summary>
        public string InvestNo { get; set; }

        /// <summary>
        /// Web-IM_POSID	Investment Program position ID(ZSO_APPREQ - POSID2,ZSO_PSCP - POSID3,ZSO_PSOP - POSID3)
        /// </summary>
        public string InvestPosID { get; set; }

        /// <summary>
        /// Web-IM_GJAHR	Investment Program Approval year("ZSO_APPREQ - GJAHR2,ZSO_PSCP - GJAHR,ZSO_PSOP - GJAHR2")
        /// </summary>
        public string InvestYear { get; set; }

        /// <summary>
        /// Web-WBS_POSID	WBS("ZSO_PSCP - POSID,ZSO_PSOP - POSID")
        /// </summary>
        public string WBSNo { get; set; }

        /// <summary>
        /// Web-WBS_PSPID	Project Definition number(ZSO_PSCP - POSID)
        /// </summary>
        public string ProjNo { get; set; }

        /// <summary>
        /// Web-WBS_OBJNR	WBS object number start with PR*("ZSO_PSCP - OBJNR,ZSO_PSOP - OBJNR")
        /// </summary>
        public string WBSID { get; set; }

        /// <summary>
        /// Web-WBS_PRART	WBS type("ZSO_PSCP - PRART,ZSO_PSOP - PRART")
        /// </summary>
        public string WBSType { get; set; }

        /// <summary>
        /// Web-KSTAR	G/L Account(ZSO_CELEM - SAKNR)
        /// </summary>
        public string GLAccount { get; set; }

        /// <summary>
        /// Web-FYEAR	"Fiscal year - Budget or Fiscal year - Plan"()
        /// </summary>
        public string FiscalYear { get; set; }

        /// <summary>
        /// Web-BUDGET_AMT	Budget value()
        /// </summary>
        public string TotalPlanValue { get; set; }


        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue01 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue02 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue03 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue04 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue05 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue06 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue07 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue08 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue09 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue10 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue11 { get; set; }
        /// <summary>
        /// Web-PLAN    PLAN by month
        /// </summary>
        public string PLANValue12 { get; set; }
        /// <summary>
        /// Web-Currentcy
        /// </summary>
        public string Currentcy { get; set; }
    }
    public class input
    {
        public double ItemOrder { get; set; }

        public string INDEX_CONTROL { get; set; }

        public string TRANSACTION_TYPE { get; set; }

        public string DATA_TYPE { get; set; }

        public string DATA_NAME { get; set; }

        public string METHOD { get; set; }

        public string ID { get; set; }

        public string ASSET_CLASS { get; set; }

        public string GL_ACCOUNT { get; set; }

        public string FISCAL_YEAR { get; set; }

        public string AMOUNT01 { get; set; }

        public string AMOUNT02 { get; set; }

        public string AMOUNT03 { get; set; }

        public string AMOUNT04 { get; set; }

        public string AMOUNT05 { get; set; }

        public string AMOUNT06 { get; set; }

        public string AMOUNT07 { get; set; }

        public string AMOUNT08 { get; set; }

        public string AMOUNT09 { get; set; }

        public string AMOUNT10 { get; set; }

        public string AMOUNT11 { get; set; }

        public string AMOUNT12 { get; set; }

    }
    public class PlanPeroid
    {
        /// <summary>
        /// Case Method
        /// 1. กรณี Internal order type EXJ, OPX0, OPX1
        /// 2. กรณี Internal order type RMA, LES
        /// 3. กรณี WBS - OPEX
        /// 4. กรณี WBS - CAPEX
        /// 5. กรณี WBS - CAPEX (ORP)
        /// </summary> 
        public string CaseMethod { get; set; }
        public GetPlanPeroid PlanPeroidDT { get; set; }
    }
    public class GetPlanPeroid
    {
        /// <summary>
        /// Web-ORD_AUFNR	Internal order number	ZSO_IO - AUFNR
        /// </summary> 
        public string InternalOrderNo { get; set; }
        /// <summary>
        /// Web-ORD_OBJNR	Internal order – Object number	ZSO_IO - OBJNR
        /// </summary> 
        public string InternalOrderID { get; set; }
        /// <summary>
        /// Web-ORD_AUART	Internal order – Order type	ZSO_IO - AUART
        /// </summary> 
        public string InternalOrderType { get; set; }
        /// <summary>
        /// Web-ORD_KOSTV	Internal order – Responsible Cost center	ZSO_IO - KOSTV
        /// </summary> 
        public string ResponeCostcenter { get; set; }
        /// <summary>
        /// Web-WBS_POSID	WBS number 	"ZSO_PSCP - POSID, ZSO_PSOP - POSID"
        /// </summary> 
        public string WBSNo { get; set; }
        /// <summary>
        /// Web-WBS_PSPID	Project Definition number	ZSO_PSCP - POSID
        /// </summary> 
        public string ProjNo { get; set; }
        /// <summary>
        /// Web-WBS_OBJNR	WBS object number 	"ZSO_PSCP - OBJNR ,ZSO_PSOP - OBJNR"
        /// </summary> 
        public string WBSID { get; set; }

        /// <summary>
        /// Web-WBS_PRART	Project type	"ZSO_PSCP - PRART, ZSO_PSOP - PRART"
        /// </summary> 
        public string ProjType { get; set; }
        /// <summary>
        /// Web-WBS_ AKSTL	WBS – Requesting Cost center	ZSO_PSOP - AKSTL
        /// </summary> 
        public string RequestCostcenter { get; set; }
        /// <summary>
        /// Web-APPR_POSNR 	Appropriation Request - internal Number	ZSO_APPREQ - POSNR
        /// </summary> 
        public string AppReqNo { get; set; }
        /// <summary>
        /// Web-APPR_POSID	Appropriation Request Number	ZSO_APPREQ - POSID
        /// </summary> 
        public string AppNo { get; set; }
        /// <summary>
        /// Web-APPR_IVART	Appropriation request type	ZSO_APPREQ - IVART
        /// </summary> 
        public string AppType { get; set; }

        /// <summary>
        /// Web-FYEAR	Fiscal year (Budget)	Fiscal year - Budget
        /// </summary> 
        public string FiscalYear { get; set; }


    }
    public class PlanPeroidData
    {

        /// <summary>
        /// WEB_INDEX	Index No.
        /// </summary> 
        public string IndexNo { get; set; }

        /// <summary>
        /// SAP_COUNT	IF WEB-INDEX duplicate then add 1
        /// </summary> 
        public string ItemNo { get; set; }

        /// <summary>
        /// WEB-ORD_AUFNR	Order Number
        /// </summary> 
        public string OrderNo { get; set; }

        /// <summary>
        /// WEB-KOSTL	Cost center (WBS & Order)
        /// </summary> 
        public string Costcenter { get; set; }

        /// <summary>
        /// Web-WBS_POSID	WBS number
        /// </summary> 
        public string WBSNo { get; set; }

        /// <summary>
        /// Web-WBS_PSPID	Project Definition number
        /// </summary> 
        public string ProjNo { get; set; }

        /// <summary>
        /// SAP-GL	G/L Account
        /// </summary> 
        public string GLAccount { get; set; }

        /// <summary>
        /// SAP-Year	Fiscal Year
        /// </summary> 
        public string FiscalYear { get; set; }

        /// <summary>
        /// SAP-PLAN01	Plan-Jan
        /// </summary> 
        public decimal? PLAN01 { get; set; }

        /// <summary>
        /// SAP-PLAN02	Plan-Feb
        /// </summary> 
        public decimal? PLAN02 { get; set; }

        /// <summary>
        /// SAP-PLAN03	Plan-Mar
        /// </summary> 
        public decimal? PLAN03 { get; set; }

        /// <summary>
        /// SAP-PLAN04	Plan-Apr
        /// </summary> 
        public decimal? PLAN04 { get; set; }

        /// <summary>
        /// SAP-PLAN05	Plan-May
        /// </summary> 
        public decimal? PLAN05 { get; set; }

        /// <summary>
        /// SAP-PLAN06	Plan-Jun
        /// </summary> 
        public decimal? PLAN06 { get; set; }

        /// <summary>
        /// SAP-PLAN07	Plan-Jul
        /// </summary> 
        public decimal? PLAN07 { get; set; }

        /// <summary>
        /// SAP-PLAN08	Plan-Aug
        /// </summary> 
        public decimal? PLAN08 { get; set; }

        /// <summary>
        /// SAP-PLAN09	Plan-Sep
        /// </summary> 
        public decimal? PLAN09 { get; set; }

        /// <summary>
        /// SAP-PLAN10	Plan-Oct
        /// </summary> 
        public decimal? PLAN10 { get; set; }

        /// <summary>
        /// SAP-PLAN11	Plan-Nov
        /// </summary> 
        public decimal? PLAN11 { get; set; }

        /// <summary>
        /// SAP-PLAN12	Plan-Dec
        /// </summary> 
        public decimal? PLAN12 { get; set; }

        /// <summary>
        /// SAP_MSG	Message Type
        /// </summary> 
        public string MSGType { get; set; }
        /// <summary>
        /// SAP_MSG	Message Text
        /// </summary> 
        public string MSG { get; set; }

        /// <summary>
        /// SAP_DATE	Date
        /// </summary> 
        public string TrnsDate { get; set; }

        /// <summary>
        /// SAP_TIME	Time
        /// </summary> 
        public string TrnsTime { get; set; }

    }
    public class CheckGLBudget
    {
        /// <summary>
        /// Case Method 
        /// </summary> 
        public string CaseMethod { get; set; }
        public RequestGLBudget GLBudget { get; set; }
    }
    public class RequestGLBudget
    {
        /// <summary>
        /// Web-ORD_AUFNR	Internal order number	ZSO_IO - AUFNR
        /// </summary> 
        public string InternalOrderNo { get; set; }

        /// <summary>
        /// Web-ORD_OBJNR	Internal order – Object number	ZSO_IO - OBJNR
        /// </summary> 
        public string InternalOrderID { get; set; }

        /// <summary>
        /// Web-ORD_AUART	Internal order – Order type	ZSO_IO - AUART
        /// </summary> 
        public string InternalOrdertype { get; set; }

        /// <summary>
        /// Web-WBS_POSID	WBS number level 1	"ZSO_PSCP - POSID,ZSO_PSOP - POSID"
        /// </summary> 
        public string WBSNo { get; set; }

        /// <summary>
        /// Web-WBS_OBJNR	WBS object number level 1	"ZSO_PSCP - OBJNR,ZSO_PSOP - OBJNR"
        /// </summary> 
        public string WBSID { get; set; }

        /// <summary>
        /// Web-WBS_PRART	Project type	"ZSO_PSCP - PRART,ZSO_PSOP - PRART"
        /// </summary> 
        public string ProjType { get; set; }

        /// <summary>
        /// Web-FYEAR	Fiscal year (Budget)	Fiscal year - Budget
        /// </summary> 
        public string FiscalYear { get; set; }

    }
    public class ResponseGLBudget
    {

        /// <summary>
        /// WEB_INDEX	Index No.
        /// </summary>
        public string IndexNo { get; set; }

        /// <summary>
        /// SAP_COUNT	IF WEB-INDEX duplicate then add 1
        /// </summary>
        public string ItemNo { get; set; }

        /// <summary>
        /// WEB_ORD_AUFNR	Order Number
        public string OrderNo { get; set; }
        /// </summary>

        /// <summary>
        /// WEB_WBS_POSID	WBS Element
        /// </summary>
        public string WBSNo { get; set; }

        /// <summary>
        /// SAP_GL	GL Account
        /// </summary>
        public string GLAccount { get; set; }

        /// <summary>
        /// WEB_FYEAR	Fiscal Year
        /// </summary>
        public string FiscalYear { get; set; }

        /// <summary>
        /// SAP_BUD	Budget
        /// </summary>
        public string Budget { get; set; }

        /// <summary>
        /// SAP_ACT	Actual
        /// </summary>
        public string Actual { get; set; }

        /// <summary>
        /// SAP_CMT	Commitment
        /// </summary>
        public string Commitment { get; set; }

        /// <summary>
        /// SAP_DIS	Assigned/Distributed
        /// </summary>
        public string Assigned { get; set; }

        /// <summary>
        /// SAP_BAL	Budget Available
        /// </summary>
        public string Available { get; set; }
        /// <summary>
        /// SAP_MSG	Message Type
        /// </summary>
        public string MSGType { get; set; }

        /// <summary>
        /// SAP_MSG	Message Text
        /// </summary>
        public string MSG { get; set; }

        /// <summary>
        /// SAP_DATE	Date
        /// </summary>
        public string SAP_DATE { get; set; }

        /// <summary>
        /// SAP_TIME	Time
        /// </summary>
        public string SAP_TIME { get; set; }

    }
    public class CheckBudget
    {

        /// <summary>
        /// Case Method  ZSO_BUDGET_CHECK
        /// </summary> 
        public string CaseMethod { get; set; }
        public RequestCheckBudget[] GLBudget { get; set; }

    }
    public class RequestCheckBudget
    {
        public string caseMethod { get; set; }
        /// <summary>
        ///  Web-ORD_AUFNR	Internal order number	ZSO_IO - AUFNR
        /// </summary> 
        public string InternalOrderNo { get; set; }

        /// <summary>
        ///  Web-ORD_OBJNR	Internal order – Object number	ZSO_IO - OBJNR
        /// </summary> 
        public string InternalOrderID { get; set; }

        /// <summary>
        ///  Web-ORD_AUART	Internal order – Order type	ZSO_IO - AUART
        /// </summary> 
        public string InternalOrderType { get; set; }

        /// <summary>
        ///  Web-APPR_POSNR 	Appropriation Request - internal Number	ZSO_APPREQ - POSNR
        /// </summary> 
        public string AppNo { get; set; }

        /// <summary>
        ///  Web-APPR_POSID	Appropriation Request Number	ZSO_APPREQ - POSID
        /// </summary> 
        public string AppReqNo { get; set; }

        /// <summary>
        ///  Web-APPR_OBJNR	Appropriation request - Object number (IQ*)	ZSO_APPREQ - OBJNR
        /// </summary> 
        public string AppID { get; set; }

        /// <summary>
        ///  Web-APPR_IVART	Appropriation request type	ZSO_APPREQ - IVART
        /// </summary> 
        public string AppType { get; set; }

        /// <summary>
        ///  Web-WBS_POSID	WBS number level 1	"ZSO_PSCP - POSID, ZSO_PSOP - POSID"
        /// </summary> 
        public string WBSNo { get; set; }

        /// <summary>
        ///  Web-WBS_OBJNR	WBS object number level 1	"ZSO_PSCP - OBJNR, ZSO_PSOP - OBJNR"
        /// </summary> 
        public string WBSID { get; set; }

        /// <summary>
        ///  Web-WBS_PRART	Project type	"ZSO_PSCP - PRART, ZSO_PSOP - PRART"
        /// </summary> 
        public string ProjType { get; set; }

        /// <summary>
        ///  Web-FYEAR	Fiscal year (Budget)	Fiscal year - Budget
        /// </summary> 
        public string FiscalYear { get; set; }

    }
    public class ResponseCheckBudget
    {

        /// <summary>
        /// WEB_INDEX	Index No.
        /// </summary> 
        public string IndexNo { get; set; }

        /// <summary>
        /// SAP_COUNT	IF WEB-INDEX duplicate then add 1
        /// </summary> 
        public string SAP_COUNT { get; set; }
        /// <summary>
        /// WEB_ORD_AUFNR	Order Number
        /// </summary> 
        public string OrderNo { get; set; }

        /// <summary>
        /// WEB_APPR_POSID	Appropriation Request Number
        /// </summary> 
        public string AppReqNo { get; set; }

        /// <summary>
        /// WEB_WBS_POSID	WBS Element
        /// </summary> 
        public string WBSNo { get; set; }

        /// <summary>
        /// WEB_FYEAR	Fiscal Year
        /// </summary> 
        public string FiscalYear { get; set; }

        /// <summary>
        /// SAP_BUD	Budget
        /// </summary> 
        public decimal Budget { get; set; }

        /// <summary>
        /// SAP_ACT	Actual
        /// </summary> 
        public decimal Actual { get; set; }

        /// <summary>
        /// SAP_CMT	Commitment
        /// </summary> 
        public decimal Commitment { get; set; }

        /// <summary>
        /// SAP_DIS	Assigned/Distributed
        /// </summary> 
        public decimal Assigned { get; set; }

        /// <summary>
        /// SAP_BAL	Budget Available
        /// </summary> 
        public decimal Available { get; set; }

        /// <summary>
        /// SAP_MSG	Message Type
        /// </summary> 
        public string MSGType { get; set; }
        /// <summary>
        /// SAP_MSG	Message Text
        /// </summary> 
        public string MSG { get; set; }

        /// <summary>
        /// SAP_DATE	Date
        /// </summary> 
        public string SAP_DATE { get; set; }

        /// <summary>
        /// SAP_TIME	Time
        /// </summary> 
        public string SAP_TIME { get; set; }

    }


    #endregion

    public List<ResponseGLBudget> GetGLBudget(CheckGLBudget lstGLBudget)
    {
        List<ResponseGLBudget> lst = new List<ResponseGLBudget>();
        if (lstGLBudget != null)
        {
            new SOAPLog().SaveSOAPToTextFile(lstGLBudget, "ZSO_BUDGET_CHECK_GL/PreRequest");
            string _Client = Client("ZSO_BUDGET_CHECK_GL", "", "");
            using (var PI_Client = new ZSO_BUDGET_CHECK_GL.SocialCapitalBudgetGLCheckInquiry_Sync_Out_SIClient(_Client))//"HTTP_Port"
            {
                ///Credential
                PI_Client.ClientCredentials.UserName.UserName = this.SAPClientUserName;
                PI_Client.ClientCredentials.UserName.Password = this.SAPClientPassword;

                ZSO_BUDGET_CHECK_GL.SocialCapitalBudgetGLCheckInquiryRes_DT[] arrResponse;
                ZSO_BUDGET_CHECK_GL.SocialCapitalBudgetGLCheckInquiryReq_DT req = new ZSO_BUDGET_CHECK_GL.SocialCapitalBudgetGLCheckInquiryReq_DT();

                string _caseMethod = lstGLBudget.CaseMethod;
                switch (_caseMethod.ToUpper())
                {
                    case "OPX1":
                    case "OPX0":
                    case "RMA":
                    case "EXJ":
                        req.IV_ORD_AUFNR = lstGLBudget.GLBudget.InternalOrderNo;
                        req.IV_ORD_OBJNR = lstGLBudget.GLBudget.InternalOrderID;
                        req.IV_ORD_AUART = lstGLBudget.GLBudget.InternalOrdertype;
                        req.IV_FYEAR = lstGLBudget.GLBudget.FiscalYear;
                        break;
                    case "LES":
                        req.IV_ORD_AUFNR = lstGLBudget.GLBudget.InternalOrderNo;
                        req.IV_ORD_OBJNR = lstGLBudget.GLBudget.InternalOrderID;
                        req.IV_ORD_AUART = lstGLBudget.GLBudget.InternalOrdertype;
                        break;
                    case "06":
                    case "22":
                        req.IV_WBS_POSID = lstGLBudget.GLBudget.WBSNo;
                        req.IV_WBS_OBJNR = lstGLBudget.GLBudget.WBSID;
                        req.IV_WBS_PRART = lstGLBudget.GLBudget.ProjType;
                        break;
                    case "OP":
                        req.IV_WBS_POSID = lstGLBudget.GLBudget.WBSNo;
                        req.IV_WBS_OBJNR = lstGLBudget.GLBudget.WBSID;
                        req.IV_WBS_PRART = lstGLBudget.GLBudget.ProjType;
                        req.IV_FYEAR = lstGLBudget.GLBudget.FiscalYear;
                        break;
                    default:
                        req.IV_ORD_AUFNR = lstGLBudget.GLBudget.InternalOrderNo;
                        req.IV_ORD_OBJNR = lstGLBudget.GLBudget.InternalOrderID;
                        req.IV_ORD_AUART = lstGLBudget.GLBudget.InternalOrdertype;
                        req.IV_WBS_POSID = lstGLBudget.GLBudget.WBSNo;
                        req.IV_WBS_OBJNR = lstGLBudget.GLBudget.WBSID;
                        req.IV_WBS_PRART = lstGLBudget.GLBudget.ProjType;
                        req.IV_FYEAR = lstGLBudget.GLBudget.FiscalYear;

                        break;

                }

                new SOAPLog().SaveSOAPToTextFile(lstGLBudget, "ZSO_BUDGET_CHECK_GL/Request");
                var Response = PI_Client.SocialCapitalBudgetGLCheckInquiry_Sync_Out_SI(req);

                new SOAPLog().SaveSOAPToTextFile(Response, "ZSO_BUDGET_CHECK_GL/Response");

                if (Response.ET_ZBE_BUDGET_CHECK_GL != null)
                {
                    lst = Response.ET_ZBE_BUDGET_CHECK_GL.Select(s => new ResponseGLBudget
                    {
                        Actual = s.SAP_ACT,
                        Assigned = s.SAP_DIS,
                        Available = s.SAP_BAL,
                        Budget = s.SAP_BUD,
                        Commitment = s.SAP_CMT,
                        SAP_DATE = s.SAP_DATE,
                        SAP_TIME = s.SAP_TIME,
                        FiscalYear = s.WEB_FYEAR,
                        GLAccount = s.SAP_GL,
                        IndexNo = s.WEB_INDEX,
                        ItemNo = s.SAP_COUNT,
                        MSG = s.SAP_MSG,
                        MSGType = "",
                        OrderNo = s.WEB_ORD_AUFNR,
                        WBSNo = s.WEB_WBS_POSID
                    }).ToList();
                }
            }
        }
        return lst;
    }

    public List<ResponseCheckBudget> GetBudget(CheckBudget lstGLBudget)
    {
        List<ResponseCheckBudget> lst = new List<ResponseCheckBudget>();
        if (lstGLBudget != null)
        {
            new SOAPLog().SaveSOAPToTextFile(lstGLBudget, "ZSO_BUDGET_CHECK/PreRequest");
            string _Client = Client("ZSO_BUDGET_CHECK", "", "");
            using (var PI_Client = new ZSO_BUDGET_CHECK.SocialCapitalBudgetCheckInquiry_Sync_Out_SIClient(_Client))//"HTTP_Port"
            {
                ///Credential
                PI_Client.ClientCredentials.UserName.UserName = this.SAPClientUserName;
                PI_Client.ClientCredentials.UserName.Password = this.SAPClientPassword;

                lstGLBudget.GLBudget.Select(s => new RequestCheckBudget
                {
                    caseMethod = ((s.InternalOrderType == "OPX0" || s.InternalOrderType == "OPX1") ? "OPX1" : ((s.AppType == "P" || s.AppType == "E") ? "P" : ((s.ProjType == "06") ? "06" : ((s.ProjType == "22") ? "22" : ((s.InternalOrderType == "LES") ? "LES" : ((s.ProjType == "OP") ? "OP" : "")))))),
                    AppType = s.AppType,
                    AppID = s.AppID,
                    AppNo = s.AppNo,
                    AppReqNo = s.AppReqNo,
                    FiscalYear = s.FiscalYear,
                    InternalOrderType = s.InternalOrderType,
                    InternalOrderNo = s.InternalOrderNo,
                    InternalOrderID = s.InternalOrderID,
                    WBSID = s.WBSID,
                    WBSNo = s.WBSNo,
                    ProjType = s.ProjType
                }).ToArray();

                ZSO_BUDGET_CHECK.ZSO_BUDG_CHK_LOG[] arrResponse;
                ZSO_BUDGET_CHECK.ZSO_BUDGET_CHECK_S[] req = new ZSO_BUDGET_CHECK.ZSO_BUDGET_CHECK_S[lstGLBudget.GLBudget.Length];
                int idx = 0;
                foreach (var _req in lstGLBudget.GLBudget)
                {
                    ZSO_BUDGET_CHECK.ZSO_BUDGET_CHECK_S clsreq = new ZSO_BUDGET_CHECK.ZSO_BUDGET_CHECK_S();
                    switch ((_req.caseMethod + "").ToUpper())
                    {
                        case "OPX1":
                            clsreq.ORD_AUFNR = _req.InternalOrderNo;
                            clsreq.ORD_OBJNR = _req.InternalOrderID;
                            clsreq.ORD_AUART = _req.InternalOrderType;
                            clsreq.FYEAR = _req.FiscalYear;
                            break;

                        case "LES":
                            clsreq.ORD_AUFNR = _req.InternalOrderNo;
                            clsreq.ORD_OBJNR = _req.InternalOrderID;
                            clsreq.ORD_AUART = _req.InternalOrderType;
                            break;
                        case "P":
                        case "E":
                            clsreq.APPR_POSNR = _req.AppReqNo;
                            clsreq.APPR_POSID = _req.AppNo;
                            clsreq.APPR_OBJNR = _req.AppID;
                            clsreq.APPR_IVART = _req.AppType;
                            break;
                        case "06":
                            clsreq.WBS_POSID = _req.WBSNo;
                            clsreq.WBS_OBJNR = _req.WBSID;
                            clsreq.WBS_PRART = _req.ProjType;

                            break;
                        case "22":
                            clsreq.WBS_POSID = _req.WBSNo;
                            clsreq.WBS_OBJNR = _req.WBSID;
                            clsreq.WBS_PRART = _req.ProjType;
                            //clsreq.FYEAR = _req.FiscalYear;

                            break;
                        case "OP":
                            clsreq.WBS_POSID = _req.WBSNo;
                            clsreq.WBS_OBJNR = _req.WBSID;
                            clsreq.WBS_PRART = _req.ProjType;
                            clsreq.FYEAR = _req.FiscalYear;

                            break;
                        default:
                            clsreq.ORD_AUFNR = _req.InternalOrderNo;
                            clsreq.ORD_OBJNR = _req.InternalOrderID;
                            clsreq.ORD_AUART = _req.InternalOrderType;
                            clsreq.APPR_POSNR = _req.AppNo;
                            clsreq.APPR_POSID = _req.AppReqNo;
                            clsreq.APPR_OBJNR = _req.AppID;
                            clsreq.APPR_IVART = _req.AppType;
                            clsreq.WBS_POSID = _req.WBSNo;
                            clsreq.WBS_OBJNR = _req.WBSID;
                            clsreq.WBS_PRART = _req.ProjType;
                            clsreq.FYEAR = _req.FiscalYear;
                            break;

                    }
                    req[idx] = clsreq;
                    idx++;
                }
                new SOAPLog().SaveSOAPToTextFile(req, "ZSO_BUDGET_CHECK/Request");
                arrResponse = PI_Client.SocialCapitalBudgetCheckInquiry_Sync_Out_SI(req);

                new SOAPLog().SaveSOAPToTextFile(arrResponse, "ZSO_BUDGET_CHECK/Response");
                if (arrResponse != null)
                {
                    lst = arrResponse.Select(s => new ResponseCheckBudget
                    {
                        Actual = s.SAP_ACT,
                        Assigned = s.SAP_DIS,
                        Available = s.SAP_BAL,
                        Budget = s.SAP_BUD,
                        Commitment = s.SAP_CMT,
                        SAP_DATE = s.SAP_DATE,
                        SAP_TIME = s.SAP_TIME,
                        FiscalYear = s.WEB_FYEAR,
                        IndexNo = s.WEB_INDEX,
                        SAP_COUNT = s.SAP_COUNT,
                        MSG = s.SAP_MSG,
                        MSGType = "",
                        OrderNo = s.WEB_ORD_AUFNR,
                        WBSNo = s.WEB_WBS_POSID,
                        AppReqNo = s.WEB_APPR_POSID
                    }).ToList();
                }
            }
        }
        return lst;
    }
}