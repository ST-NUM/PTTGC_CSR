using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClassExecute
/// </summary>
namespace ClassExecute
{
    public class ST_Result
    {
        public string sStatus { get; set; }
        public string sMessage { get; set; }
        public string sMessage1 { get; set; }
        public string sMessage2 { get; set; }
        public string sMessage3 { get; set; }
    }

    public class ST_CCTR
    {
        public string sCCTRCode { get; set; }
        public string sCCTRShortName { get; set; }
        public string sCCTRFullName { get; set; }
        public string sCompanyCode { get; set; }
        public string sValue { get; set; }
        public string sText { get; set; }
    }

    public class CData_File
    {
        public int? nID { get; set; }
        public int? nProjectID { get; set; }
        public string sDescription { get; set; }
        public string sFilename { get; set; }
        public string sPath { get; set; }
        public string sSysFileName { get; set; }
        public int? nUserID { get; set; }
        public string sUpdate { get; set; }
    }
}