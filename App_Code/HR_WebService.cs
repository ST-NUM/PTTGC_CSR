using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;


/// <summary>
/// Summary description for HR_WebService
/// </summary>
public class HR_WebService
{
    public static string sBaseURL_HR_Service = ConfigurationManager.AppSettings["BaseURL_HR_Service"];
    public static string sUsername_HR_Service = ConfigurationManager.AppSettings["Username_HR_Service"];
    public static string sPassword_HR_Service = ConfigurationManager.AppSettings["Password_HR_Service"];
    public static string sBudgettingOrg = ConfigurationManager.AppSettings["BudgettingOrg"];
    public static string sBudgettingOrgID = ConfigurationManager.AppSettings["BudgettingOrgID"];
    public static string URLRestFormat = "json";
    public static string SyncType = "REST";//  SERVICE
    public HR_WebService()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static ObjectData EmployeeService(string sTop, string sEmployeeID, string sOrgID, string sPosID, string sIndicator)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sEmployeeID))
        {
            _Filter += " and EmployeeID eq '" + sEmployeeID + "'";
        }
        if (!string.IsNullOrEmpty(sOrgID))
        {
            var arrOrg = sOrgID.Split(',');
            _Filter += " and (OrgID eq '" + (string.Join("' or OrgID eq '", arrOrg)) + "')";
        }
        if (!string.IsNullOrEmpty(sPosID))
        {
            _Filter += " and PositionID eq '" + sPosID + "'";
        }
        if (!string.IsNullOrEmpty(sIndicator))
        {
            _Filter += " and Indicator eq '" + sIndicator + "'";
        }
        if (!string.IsNullOrEmpty(_Filter))
        {

            //_Filter = "&$filter= Indicator ne 'null' and Name ne 'null' " + _Filter;
            _Filter = "&$filter= 1 eq 1" + _Filter;
            //select TOP
            sTop = string.IsNullOrEmpty(sTop) ? "" : "&$top=" + sTop;
            string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{0}{1}";//&$orderby=PositionLevel
            //re format URL
            sURL = string.Format(sURL, sTop, _Filter);
            WebClient serviceRequest = new WebClient();
            serviceRequest.Encoding = UTF8Encoding.UTF8;
            serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
            //https://test-hr-ws.pttgc.corp:4320/pttgc/hcm/hrwebservices/project/services/HR/services/HR_WebServices.xsodata/EmployeeService?$format=json&$top=1&$filter= Indicator ne 'null' and Name ne 'null'  and EmployeeID eq '26000913'
            //https://test-hr-ws.pttgc.corp:4320/pttgc/hcm/hrwebservices/project/services/HR/services/HR_WebServices.xsodata/EmployeeService?$format=json&$top=1&$filter= Indicator ne 'null' and Name ne 'null'  and Indicator eq  'F-MA-BG'
            //https://test-hr-ws.pttgc.corp:4320/pttgc/hcm/hrwebservices/project/services/HR/services/HR_WebServices.xsodata/EmployeeService?$format=json&$top=1&$filter= Indicator ne 'null' and Name ne 'null'  and Indicator eq  'F-MA-BG'

            string response = serviceRequest.DownloadString(new Uri(sURL));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                data = new SystemFunction().JsonDeserialize<ObjectData>(response);
            }
        }
        return data;
    }
    public static ObjectData EmployeeService_PersonalID(string sPersonalID)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sPersonalID))
        {
            _Filter = "&$filter= PersonalID eq '" + sPersonalID + "'";
            string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{1}&$orderby=PositionLevel";
            //re format URL
            sURL = string.Format(sURL, "", _Filter);
            WebClient serviceRequest = new WebClient();
            serviceRequest.Encoding = UTF8Encoding.UTF8;
            serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
            string response = serviceRequest.DownloadString(new Uri(sURL));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                data = new SystemFunction().JsonDeserialize<ObjectData>(response);
            }
        }
        return data;
    }
    public static ObjectData EmployeeService_EmployeeID(string sPersonalID)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sPersonalID))
        {
            _Filter = "&$filter= EmployeeID eq '" + sPersonalID + "'";
            string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{1}&$orderby=PositionLevel";
            //re format URL
            sURL = string.Format(sURL, "", _Filter);
            WebClient serviceRequest = new WebClient();
            serviceRequest.Encoding = UTF8Encoding.UTF8;
            serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
            string response = serviceRequest.DownloadString(new Uri(sURL));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                data = new SystemFunction().JsonDeserialize<ObjectData>(response);
            }
        }
        return data;
    }

    public static bool CheckEmployeeInOrg(string sEmployeeID, string sOrgID)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sEmployeeID))
        {
            _Filter += " and EmployeeID eq '" + sEmployeeID + "'";
        }
        if (!string.IsNullOrEmpty(sOrgID))
        {
            var arrOrg = sOrgID.Split(',');
            _Filter += " and (OrgID eq '" + (string.Join("' or OrgID eq '", arrOrg)) + "')";
        }

        if (!string.IsNullOrEmpty(_Filter))
        {
            _Filter = "&$filter= 1 eq 1" + _Filter;
            //select TOP
            string sTop = "&$top=1&select=EmployeeID";
            string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{0}{1}";//&$orderby=PositionLevel
            //re format URL
            sURL = string.Format(sURL, sTop, _Filter);
            WebClient serviceRequest = new WebClient();
            serviceRequest.Encoding = UTF8Encoding.UTF8;
            serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);

            string response = serviceRequest.DownloadString(new Uri(sURL));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                data = new SystemFunction().JsonDeserialize<ObjectData>(response);
            }
        }
        return data.d.results.FirstOrDefault() != null;
    }

    public static ObjectData GetEmployeeInOrg(string sEmployeeID, string sOrgID)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sEmployeeID))
        {
            var arrEmp = sEmployeeID.Split(',');
            _Filter += " and (EmployeeID eq '" + (string.Join("' or EmployeeID eq '", arrEmp)) + "')";
        }
        if (!string.IsNullOrEmpty(sOrgID))
        {
            var arrOrg = sOrgID.Split(',');
            _Filter += " and (OrgID eq '" + (string.Join("' or OrgID eq '", arrOrg)) + "')";
        }

        if (!string.IsNullOrEmpty(_Filter))
        {
            _Filter = "&$filter= 1 eq 1" + _Filter;
            //select TOP
            string sTop = "&$top=20&select=EmployeeID";
            string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{0}{1}";//&$orderby=PositionLevel
            //re format URL
            sURL = string.Format(sURL, sTop, _Filter);
            WebClient serviceRequest = new WebClient();
            serviceRequest.Encoding = UTF8Encoding.UTF8;
            serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);

            string response = serviceRequest.DownloadString(new Uri(sURL));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                data = new SystemFunction().JsonDeserialize<ObjectData>(response);
            }
        }
        return data;
    }

    public static ObjectData EmployeeService_Search(string sSearch)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sSearch))
        {
            _Filter += " and (substringof('" + sSearch + "',EmployeeID) or substringof('" + sSearch.ToLower() + "',tolower(Name)))";
        }
        //UserID eq EmployeeID and Indicator ne 'null' and 
        _Filter = "&$filter= Name ne 'null' and Name ne ''  " + _Filter;
        //select TOP
        string sTop = "&$top=20";
        string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{0}{1}&$orderby=EmployeeID";
        //re format URL
        sURL = string.Format(sURL, sTop, _Filter);
        WebClient serviceRequest = new WebClient();
        serviceRequest.Encoding = UTF8Encoding.UTF8;
        serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
        string response = serviceRequest.DownloadString(new Uri(sURL));
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
        {
            data = new SystemFunction().JsonDeserialize<ObjectData>(response);
        }
        return data;
    }
    public static ObjectData EmployeeService_Search(string sSearch, string sOrgID, List<string> lstNotCode)
    {
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = " and EmploymentStatus eq '3'";

        if (!string.IsNullOrEmpty(sSearch))
        {
            _Filter += " and (substringof('" + sSearch + "',EmployeeID) or substringof('" + sSearch.ToLower() + "',tolower(NameTH)))";
        }
        if (!string.IsNullOrEmpty(sOrgID))
        {
            var arrOrg = sOrgID.Split(',');
            _Filter += " and (OrgID eq '" + (string.Join("' or OrgID eq '", arrOrg)) + "')";
        }
        if (lstNotCode.Any())
        {
            _Filter += " and (EmployeeID ne '" + (string.Join("' and EmployeeID ne '", lstNotCode)) + "')";
        }

        //UserID eq EmployeeID and Indicator ne 'null' and 
        _Filter = "&$filter= Name ne 'null' and Name ne ''  " + _Filter;

        //select TOP
        string sTop = "&$top=20";

        string sSelect = "&$select=EmployeeID,NameTH,THFirstName,THLastName,EmailAddress";

        string sURL = sBaseURL_HR_Service + "EmployeeService?$format=json{0}{1}{2}&$orderby=EmployeeID";

        //re format URL
        sURL = string.Format(sURL, sTop, sSelect, _Filter);
        WebClient serviceRequest = new WebClient();
        serviceRequest.Encoding = UTF8Encoding.UTF8;
        serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
        string response = serviceRequest.DownloadString(new Uri(sURL));
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
        {
            data = new SystemFunction().JsonDeserialize<ObjectData>(response);
        }
        return data;
    }
    public static ObjectData OrganizationService(string sTop, string sOrgID, string sOrgName, string sParentO_ObjID)
    {
        //UserID for access these services is  “odata” with password “Hana#1234” 
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = "&$filter=(ManagerFlg eq 'X' or startswith(S_TextEN, 'Acting '))";
        if (!string.IsNullOrEmpty(sOrgID))
        {
            _Filter += " and O_ObjID eq '" + sOrgID + "'";
        }
        if (!string.IsNullOrEmpty(sOrgName))
        {
            _Filter += " and O_ShortTextEN eq '" + sOrgName + "'";
        }
        if (!string.IsNullOrEmpty(sParentO_ObjID))
        {
            _Filter += " and ParentO_ObjID eq '" + sParentO_ObjID + "'";
        }
        //select TOP
        //Country_Region_Code eq 'ES' and Payment_Terms_Code eq '14 DAYS'
        sTop = string.IsNullOrEmpty(sTop) ? "" : "&$top=" + sTop;
        string sURL = sBaseURL_HR_Service + "OrganizationService?$format=json{0}{1}&$orderby=O_Level";//&$select=O_ShortTextEN,O_TextEN,O_ObjID,ManagerFlg,EmployeeID,ParentO_ObjID
        //re format URL
        sURL = string.Format(sURL, sTop, _Filter);
        WebClient serviceRequest = new WebClient();
        serviceRequest.Encoding = UTF8Encoding.UTF8;
        serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
        string response = serviceRequest.DownloadString(new Uri(sURL));
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
        {
            data = new SystemFunction().JsonDeserialize<ObjectData>(response);
        }
        return data;
    }
    public static ObjectData OS_Detail(string sTop, string sOBJID, string sSHORTTEXTEN)
    {
        //UserID for access these services is  “odata” with password “Hana#1234” 
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = "&$filter=";
        if (!string.IsNullOrEmpty(sOBJID))
        {
            _Filter += "OBJID eq '" + sOBJID + "'";
        }
        if (!string.IsNullOrEmpty(sSHORTTEXTEN))
        {
            _Filter += "SHORTTEXTEN eq '" + sSHORTTEXTEN + "'";
        }
        //select TOP
        sTop = string.IsNullOrEmpty(sTop) ? "" : "&$top=" + sTop;
        string sURL = sBaseURL_HR_Service + "OS_Details?$format=json{0}{1}";//&$select=O_ShortTextEN,O_TextEN,O_ObjID,ManagerFlg,EmployeeID,ParentO_ObjID
        //re format URL
        sURL = string.Format(sURL, sTop, _Filter);
        WebClient serviceRequest = new WebClient();
        serviceRequest.Encoding = UTF8Encoding.UTF8;
        serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
        string response = serviceRequest.DownloadString(new Uri(sURL));
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
        {
            data = new SystemFunction().JsonDeserialize<ObjectData>(response);
        }
        return data;
    }
    public static ObjectData OS_Detail_List(string sTop, string sOBJID, string sSearch)
    {
        //UserID for access these services is  “odata” with password “Hana#1234” 
        ObjectData data = new ObjectData();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        string _Filter = "&$filter=(replace(SHORTTEXTEN, ' ', '') ne '' and replace(SHORTTEXTEN, ' ', '') ne 'null' and replace(SHORTTEXTEN, ' ', '') ne '-')";
        if (!string.IsNullOrEmpty(sOBJID))
        {
            // += "OBJID eq '" + sOBJID + "'";
        }
        if (!string.IsNullOrEmpty(sSearch))
        {
            //_Filter += "and (substringof(OBJID,'" + sSearch + "') or substringof(replace(SHORTTEXTEN, ' ', ''),'" + sSearch.ToUpper() + "') or substringof(concat(concat(OBJID, ' - '), SHORTTEXTEN),'" + sSearch.ToUpper() + "'))";
            _Filter += " and (substringof('" + sSearch + "',OBJID) or substringof('" + sSearch.ToUpper() + "',replace(SHORTTEXTEN, ' ', '')))";
        }
        //if (string.IsNullOrEmpty(sSearch) && string.IsNullOrEmpty(sOBJID))
        //    _Filter = "";
        //select TOP
        sTop = "&$top=20";
        string sURL = sBaseURL_HR_Service + "OS_Details?$format=json{0}{1}";//&$select=O_ShortTextEN,O_TextEN,O_ObjID,ManagerFlg,EmployeeID,ParentO_ObjID
        //re format URL
        sURL = string.Format(sURL, sTop, _Filter);
        WebClient serviceRequest = new WebClient();
        serviceRequest.Encoding = UTF8Encoding.UTF8;
        serviceRequest.Credentials = new System.Net.NetworkCredential(sUsername_HR_Service, sPassword_HR_Service);
        string response = serviceRequest.DownloadString(new Uri(sURL));
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObjectData));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
        {
            data = new SystemFunction().JsonDeserialize<ObjectData>(response);
        }
        return data;
    }
    public static string ODATA_ReplaceNULLnNA(string sVal)
    {
        return (sVal + "").Replace("null", "").Replace("n/a", "");
    }
    #region Class data
    [Serializable, XmlRoot, DataContract]
    public class Metadata
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string uri { get; set; }
    }
    [Serializable, XmlRoot, DataContract]
    public class HR_DataResult
    {
        [DataMember]
        public Metadata __metadata { get; set; }
        // EmployeeService
        #region EmployeeService
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string PersonalID { get; set; }
        [DataMember]
        public string EmployeeID { get; set; }
        [DataMember]
        public string CompanyCode { get; set; }
        [DataMember]
        public string CompanyShortTxt { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ENFirstName { get; set; }
        [DataMember]
        public string ENLastName { get; set; }
        [DataMember]
        public string NameTH { get; set; }
        [DataMember]
        public string THFirstName { get; set; }
        [DataMember]
        public string THLastName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public string MobilePhone { get; set; }
        [DataMember]
        public string Indicator { get; set; }
        [DataMember]
        public string OrgID { get; set; }
        [DataMember]
        public string OrgTextEN { get; set; }
        [DataMember]
        public string OrgShortTextEN { get; set; }
        [DataMember]
        public string OrgLevel { get; set; }
        [DataMember]
        public string PositionID { get; set; }
        [DataMember]
        public string PositionLevel { get; set; }

        [DataMember]
        public string PositionTextEN { get; set; }
        [DataMember]
        public string PositionShortTextEN { get; set; }
        [DataMember]
        public string ManagerialFlag { get; set; }
        [DataMember]
        public string ParentOrgID { get; set; }
        [DataMember]
        public string ManagerID { get; set; }
        [DataMember]
        public string ManagerPositionID { get; set; }
        [DataMember]
        public string ManagerOrgID { get; set; }
        [DataMember]
        public string MainPositionCostCenter { get; set; }
        [DataMember]
        public string AssignmentCostCenter { get; set; }
        [DataMember]
        public string EmploymentStatus { get; set; }
        [DataMember]
        public string EmploymentStatusTxt { get; set; }

        #endregion
        //OS_Detail
        #region OS_Detail
        [DataMember]
        public string OTYPE { get; set; }
        [DataMember]
        public string OBJID { get; set; }
        [DataMember]
        public string OBJLV { get; set; }
        [DataMember]
        public string SHORTTEXTEN { get; set; }
        [DataMember]
        public string SHORTTEXTTH { get; set; }
        [DataMember]
        public string TEXTEN { get; set; }
        [DataMember]
        public string TEXTTH { get; set; }
        [DataMember]
        public string EmpSubGroup { get; set; }
        [DataMember]
        public string EmpSubGroupTxt { get; set; }

        #endregion
        //OrganizationService
        #region OrganizationService
        [DataMember]
        public string O_ObjID { get; set; }
        [DataMember]
        public string O_Level { get; set; }
        [DataMember]
        public string O_ShortTextEN { get; set; }
        [DataMember]
        public string O_ShortTextTH { get; set; }
        [DataMember]
        public string O_TextEN { get; set; }
        [DataMember]
        public string O_TextTH { get; set; }
        [DataMember]
        public string S_ObjID { get; set; }
        [DataMember]
        public string S_Level { get; set; }
        [DataMember]
        public string S_ShortTextEN { get; set; }
        [DataMember]
        public string S_ShortTextTH { get; set; }
        [DataMember]
        public string S_TextEN { get; set; }
        [DataMember]
        public string ManagerFlg { get; set; }
        [DataMember]
        public string MainPositionFlg { get; set; }
        [DataMember]
        public string ParentO_ObjID { get; set; }
        #endregion
    }
    [Serializable, XmlRoot, DataContract]
    public class D
    {
        [DataMember]
        public List<HR_DataResult> results { get; set; }
    }
    [Serializable, XmlRoot, DataContract]
    public class ObjectData
    {
        [DataMember]
        public D d { get; set; }
    }
    [Serializable]
    public class ST_OrganizationService
    {
        public string O_ObjID { get; set; }
        public string O_ShortTextEN { get; set; }
        public string O_TextEN { get; set; }
        public string EmployeeID { get; set; }
        public string ManagerFlg { get; set; }
        public string ParentO_ObjID { get; set; }
    }
    #endregion
}