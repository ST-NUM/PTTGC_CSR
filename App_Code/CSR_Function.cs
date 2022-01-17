using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for CSR_Function
/// </summary>
public class CSR_Function
{
    public CSR_Function()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Sync Organization    
    public static void Sync_Organization()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        string sOrgCSR = SystemFunction.sOrgCSR;
        var qOrgMain = HR_WebService.OrganizationService("", sOrgCSR, "", "").d.results.FirstOrDefault();
        if (qOrgMain != null)
        {
            var qOrg = db.TM_Organization.FirstOrDefault(w => w.sOrgID == sOrgCSR);
            if (qOrg == null)
            {
                qOrg = new TM_Organization();

                qOrg.sOrgID = sOrgCSR;
                db.TM_Organization.Add(qOrg);
            }

            qOrg.sOrgName = qOrgMain.O_TextTH;
            qOrg.sOrgNameAbbr = qOrgMain.O_ShortTextEN;
            qOrg.nLevel = CommonFunction.ParseIntNull(qOrgMain.O_Level);

            db.SaveChanges();

            var lstOrgSub = HR_WebService.OrganizationService("", "", "", sOrgCSR).d.results.ToList();
            SetOrgSub(lstOrgSub);
        }
    }

    public static void SetOrgSub(List<HR_WebService.HR_DataResult> lstOrg)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        foreach (var item in lstOrg)
        {
            string sOrgID = item.O_ObjID;
            var qOrg = db.TM_Organization.FirstOrDefault(w => w.sOrgID == sOrgID);
            if (qOrg == null)
            {
                qOrg = new TM_Organization();

                qOrg.sOrgID = sOrgID;
                db.TM_Organization.Add(qOrg);
            }

            qOrg.sOrgName = item.O_TextTH;
            qOrg.sOrgNameAbbr = item.O_ShortTextEN;
            qOrg.nLevel = CommonFunction.ParseIntNull(item.O_Level);

            db.SaveChanges();

            var lstOrgSub = HR_WebService.OrganizationService("", "", "", sOrgID).d.results.ToList();
            if (lstOrgSub.Any())
            {
                SetOrgSub(lstOrg);
            }
        }
    }
    #endregion

    #region Sync CCTR_Structure, MT_CostCenter, MT_InternalOrder, MT_GLAccount

    public static void Sync_CCTR_Structure()
    {
        string sCCID = SystemFunction.sCostCenterCSR;

        DataTable dtCCTR_Structure = CommonFunction.Get_Data(CommonFunction.connBE, "select * from MT_CCTR_Structure where Entity = '10'");
        foreach (DataColumn column in dtCCTR_Structure.Columns)
        {
            column.ColumnName = column.ColumnName.Replace(" ", "");
        }

        var lstCSAll = CommonFunction.ConvertDatableToList<MT_CCTR_Structure>(dtCCTR_Structure).ToList();
        var lstCS = new List<MT_CCTR_Structure>();
        var qMain = lstCSAll.FirstOrDefault(w => w.ID == sCCID);
        if (qMain != null)
        {
            lstCS.Add(qMain);

            if (!string.IsNullOrEmpty(qMain.H1_APPROVAL))
            {
                SetCCTR_StructureSub(lstCSAll, lstCS, qMain);
            }
        }

        if (lstCS.Any())
        {
            CommonFunction.ExecuteNonQuery("delete MT_CCTR_Structure");

            StringBuilder sb = new StringBuilder();

            #region Script Insert MT_CCTR_Structure
            string sInsert = @"INSERT INTO MT_CCTR_Structure
           ([ID]
           ,[Description]
           ,[Business_Unit]
           ,[CC_PC_BU]
           ,[DT_Costcenter]
           ,[DT_Costcenter_group]
           ,[Entity]
           ,[EVP_Costcenter]
           ,[OWNER]
           ,[PC_NODE]
           ,[PERFORMER]
           ,[PERFORM_FLAG]
           ,[PoolCCTR]
           ,[Profitcenter]
           ,[REVIEWER]
           ,[REVIEWER_BG]
           ,[Scaling]
           ,[ServiceCostcenter]
           ,[CommonCostcenter]
           ,[VP_Costcenter]
           ,[H1_APPROVAL]
           ,[H2_BU]
           ,[H3_ENTITY]
           ,[BudgetHolder])
        VALUES
           ({0},--<ID, varchar(20),>
           {1},--<Description, varchar(max),>
           {2},--<Business_Unit, varchar(max),>
           {3},--<CC_PC_BU, varchar(max),>
           {4},--<DT_Costcenter, varchar(max),>
           {5},--<DT_Costcenter_group, varchar(max),>
           {6},--<Entity, varchar(max),>
           {7},--<EVP_Costcenter, varchar(max),>
           {8},--<OWNER, varchar(max),>
           {9},--<PC_NODE, varchar(max),>
           {10},--<PERFORMER, varchar(max),>
           {11},--<PERFORM_FLAG, varchar(max),>
           {12},--<PoolCCTR, varchar(max),>
           {13},--<Profitcenter, varchar(max),>
           {14},--<REVIEWER, varchar(max),>
           {15},--<REVIEWER_BG, varchar(max),>
           {16},--<Scaling, varchar(max),>
           {17},--<ServiceCostcenter, varchar(max),>
           {18},--<CommonCostcenter, varchar(max),>
           {19},--<VP_Costcenter, varchar(max),>
           {20},--<H1_APPROVAL, varchar(max),>
           {21},--<H2_BU, varchar(max),>
           {22},--<H3_ENTITY, varchar(max),>
           {23})--<BudgetHolder, varchar(15),>" + Environment.NewLine;
            #endregion

            #region Insert MT_CCTR_Structure
            foreach (var item in lstCS)
            {
                sb.Append(string.Format(sInsert,
                CommonFunction.ConvertStringSQLNULL(item.ID),
                CommonFunction.ConvertStringSQLNULL(item.Description),
                CommonFunction.ConvertStringSQLNULL(item.Business_Unit),
                CommonFunction.ConvertStringSQLNULL(item.CC_PC_BU),
                CommonFunction.ConvertStringSQLNULL(item.DT_Costcenter),
                CommonFunction.ConvertStringSQLNULL(item.DT_Costcenter_group),
                CommonFunction.ConvertStringSQLNULL(item.Entity),
                CommonFunction.ConvertStringSQLNULL(item.EVP_Costcenter),
                CommonFunction.ConvertStringSQLNULL(item.OWNER),
                CommonFunction.ConvertStringSQLNULL(item.PC_NODE),
                CommonFunction.ConvertStringSQLNULL(item.PERFORMER),
                CommonFunction.ConvertStringSQLNULL(item.PERFORM_FLAG),
                CommonFunction.ConvertStringSQLNULL(item.PoolCCTR),
                CommonFunction.ConvertStringSQLNULL(item.Profitcenter),
                CommonFunction.ConvertStringSQLNULL(item.REVIEWER),
                CommonFunction.ConvertStringSQLNULL(item.REVIEWER_BG),
                CommonFunction.ConvertStringSQLNULL(item.Scaling),
                CommonFunction.ConvertStringSQLNULL(item.ServiceCostcenter),
                CommonFunction.ConvertStringSQLNULL(item.CommonCostcenter),
                CommonFunction.ConvertStringSQLNULL(item.VP_Costcenter),
                CommonFunction.ConvertStringSQLNULL(item.H1_APPROVAL),
                CommonFunction.ConvertStringSQLNULL(item.H2_BU),
                CommonFunction.ConvertStringSQLNULL(item.H3_ENTITY),
                CommonFunction.ConvertStringSQLNULL(item.BudgetHolder)));
            }

            CommonFunction.ExecuteNonQuery(sb.ToString());
            #endregion

            var lstCSID = lstCS.Select(s => s.ID).ToList();
            Sync_MT_CostCenter(lstCSID);
            Sync_MT_InternalOrder(lstCSID);
        }
    }

    public static List<MT_CCTR_Structure> SetCCTR_StructureSub(List<MT_CCTR_Structure> lstCSAll, List<MT_CCTR_Structure> lstCS, MT_CCTR_Structure qCS)
    {
        if (!string.IsNullOrEmpty(qCS.H1_APPROVAL))
        {
            var lstCS_ = lstCSAll.Where(w => (w.H1_APPROVAL == qCS.H1_APPROVAL || w.ID == qCS.H1_APPROVAL) && w.ID != qCS.ID).ToList();
            foreach (var item in lstCS_)
            {
                lstCS.Add(item);

                if (!string.IsNullOrEmpty(item.H1_APPROVAL) && !lstCS.Any(a => a.H1_APPROVAL == item.H1_APPROVAL))
                {
                    SetCCTR_StructureSub(lstCSAll, lstCS, item);
                }
            }
        }

        return lstCS;
    }

    public static void Sync_MT_CostCenter(List<string> lstCostID)
    {
        DataTable dtMT_CostCenter = CommonFunction.Get_Data(CommonFunction.connBE, "select * from MT_CostCenter where CostCenter in ('" + (lstCostID.Any() ? string.Join("','", lstCostID) : "") + "') and ComCode = '10'");
        var lstCC = CommonFunction.ConvertDatableToList<MT_CostCenter>(dtMT_CostCenter).ToList();

        if (lstCC.Any())
        {
            CommonFunction.ExecuteNonQuery("delete MT_CostCenter");

            StringBuilder sb = new StringBuilder();

            #region Script Insert MT_CostCenter
            string sInsert = @"INSERT INTO MT_CostCenter
           ([CostCenter]
           ,[ComCode]
           ,[BudgetHolder]
           ,[BudgetHolderName]
           ,[ShortDesc]
           ,[FullDesc]
           ,[Active]
           ,[ValidFrom]
           ,[ValidTo]
           ,[ProfitCenter]
           ,[CostCat]
           ,[UpdateBy]
           ,[UpdateOn]
           ,[ControlArea]
           ,[ObjectNumber]
           ,[LockActualPK]
           ,[LockPlanPK]
           ,[PersResp]
           ,[StdRelation]
           ,[GeneralName])
        VALUES
           ({0},--<CostCenter, varchar(10),>
           {1},--<ComCode, varchar(20),>
           {2},--<BudgetHolder{0},-- varchar(10){0},-->
           {3},--<BudgetHolderName{0},-- varchar(60){0},-->
           {4},--<ShortDesc{0},-- varchar(120){0},-->
           {5},--<FullDesc{0},-- varchar(max){0},-->
           {6},--<Active{0},-- varchar(1){0},-->
           {7},--<ValidFrom{0},-- datetime{0},-->
           {8},--<ValidTo{0},-- datetime{0},-->
           {9},--<ProfitCenter{0},-- varchar(15){0},-->
           {10},--<CostCat{0},-- varchar(10){0},-->
           {11},--<UpdateBy{0},-- varchar(20){0},-->
           {12},--<UpdateOn{0},-- datetime{0},-->
           {13},--<ControlArea{0},-- varchar(4){0},-->
           {14},--<ObjectNumber{0},-- varchar(22){0},-->
           {15},--<LockActualPK{0},-- varchar(1){0},-->
           {16},--<LockPlanPK{0},-- varchar(1){0},-->
           {17},--<PersResp{0},-- varchar(20){0},-->
           {18},--<StdRelation{0},-- varchar(12){0},-->
           {19})--<GeneralName{0},-- varchar(20){0},-->)" + Environment.NewLine;
            #endregion

            #region Insert MT_CostCenter
            foreach (var item in lstCC)
            {
                sb.Append(string.Format(sInsert,
                     CommonFunction.ConvertStringSQLNULL(item.CostCenter),
                     CommonFunction.ConvertStringSQLNULL(item.ComCode),
                     CommonFunction.ConvertStringSQLNULL(item.BudgetHolder),
                     CommonFunction.ConvertStringSQLNULL(item.BudgetHolderName),
                     CommonFunction.ConvertStringSQLNULL(item.ShortDesc),
                     CommonFunction.ConvertStringSQLNULL(item.FullDesc),
                     CommonFunction.ConvertStringSQLNULL(item.Active),
                     "'" + item.ValidFrom + "'",
                     "'" + item.ValidTo + "'",
                     CommonFunction.ConvertStringSQLNULL(item.ProfitCenter),
                     CommonFunction.ConvertStringSQLNULL(item.CostCat),
                     CommonFunction.ConvertStringSQLNULL(item.UpdateBy),
                     "'" + item.UpdateOn + "'",
                     CommonFunction.ConvertStringSQLNULL(item.ControlArea),
                     CommonFunction.ConvertStringSQLNULL(item.ObjectNumber),
                     CommonFunction.ConvertStringSQLNULL(item.LockActualPK),
                     CommonFunction.ConvertStringSQLNULL(item.LockPlanPK),
                     CommonFunction.ConvertStringSQLNULL(item.PersResp),
                     CommonFunction.ConvertStringSQLNULL(item.StdRelation),
                     CommonFunction.ConvertStringSQLNULL(item.GeneralName)
                    ));
            }

            CommonFunction.ExecuteNonQuery(sb.ToString());
            #endregion
        }
    }

    public static void Sync_MT_InternalOrder(List<string> lstCostID)
    {
        DataTable dtMT_InternalOrder = CommonFunction.Get_Data(CommonFunction.connBE, "select * from MT_InternalOrder where RespCCTR in ('" + (lstCostID.Any() ? string.Join("','", lstCostID) : "") + "') and ReleasedStatus = 'X' and ISNULL(LockStatus,'') <> 'X' and IOType in ('OPX0','OPX1','EXJ')");
        var lstIO = CommonFunction.ConvertDatableToList<MT_InternalOrder>(dtMT_InternalOrder).ToList();

        if (lstIO.Any())
        {
            CommonFunction.ExecuteNonQuery("delete MT_InternalOrder");

            StringBuilder sb = new StringBuilder();

            #region Script Insert MT_InternalOrder
            string sInsert = @"INSERT INTO MT_InternalOrder
           ([IO]
           ,[ComCode]
           ,[Description]
           ,[IOType]
           ,[IOCategory]
           ,[RespCCTR]
           ,[CreatedStatus]
           ,[ReleasedStatus]
           ,[CompletedStatus]
           ,[ClosedStatus]
           ,[DeleteFlag]
           ,[ProfitCenter]
           ,[BudgetHolder]
           ,[BudgetHolderName]
           ,[ExternalOrder]
           ,[UpdateBy]
           ,[UpdateOn]
           ,[ControlArea]
           ,[ObjectNumber]
           ,[dCreateOn]
           ,[CloseDate]
           ,[LockStatus])
        VALUES
           ({0},--<IO, nvarchar(50),>
           {1},--<ComCode, nvarchar(4),>
           {2},--<Description, nvarchar(max),>
           {3},--IOType, varchar(4),>
           {4},--IOCategory, varchar(7),>
           {5},--RespCCTR, varchar(10),>
           {6},--CreatedStatus, varchar(1),>
           {7},--ReleasedStatus, varchar(1),>
           {8},--CompletedStatus, varchar(1),>
           {9},--ClosedStatus, varchar(1),>
           {10},--DeleteFlag, varchar(1),>
           {11},--ProfitCenter, varchar(10),>
           {12},--BudgetHolder, varchar(12),>
           {13},--BudgetHolderName, varchar(80),>
           {14},--ExternalOrder, varchar(20),>
           {15},--UpdateBy, varchar(30),>
           {16},--UpdateOn, datetime,>
           {17},--ControlArea, varchar(4),>
           {18},--ObjectNumber, varchar(22),>
           {19},--dCreateOn, datetime,>
           {20},--CloseDate, datetime,>
           {21})--LockStatus, varchar(1),>)" + Environment.NewLine;
            #endregion

            #region Insert MT_InternalOrder
            foreach (var item in lstIO)
            {
                sb.Append(string.Format(sInsert,
                     CommonFunction.ConvertStringSQLNULL(item.IO),
                     CommonFunction.ConvertStringSQLNULL(item.ComCode),
                     CommonFunction.ConvertStringSQLNULL(item.Description),
                     CommonFunction.ConvertStringSQLNULL(item.IOType),
                     CommonFunction.ConvertStringSQLNULL(item.IOCategory),
                     CommonFunction.ConvertStringSQLNULL(item.RespCCTR),
                     CommonFunction.ConvertStringSQLNULL(item.CreatedStatus),
                     CommonFunction.ConvertStringSQLNULL(item.ReleasedStatus),
                     CommonFunction.ConvertStringSQLNULL(item.CompletedStatus),
                     CommonFunction.ConvertStringSQLNULL(item.ClosedStatus),
                     CommonFunction.ConvertStringSQLNULL(item.DeleteFlag),
                     CommonFunction.ConvertStringSQLNULL(item.ProfitCenter),
                     CommonFunction.ConvertStringSQLNULL(item.BudgetHolder),
                     CommonFunction.ConvertStringSQLNULL(item.BudgetHolderName),
                     CommonFunction.ConvertStringSQLNULL(item.ExternalOrder),
                     CommonFunction.ConvertStringSQLNULL(item.UpdateBy),
                     "'" + item.UpdateOn + "'",
                     CommonFunction.ConvertStringSQLNULL(item.ControlArea),
                     CommonFunction.ConvertStringSQLNULL(item.ObjectNumber),
                     "'" + item.dCreateOn + "'",
                     "'" + item.CloseDate + "'",
                     CommonFunction.ConvertStringSQLNULL(item.LockStatus)
                    ));
            }

            CommonFunction.ExecuteNonQuery(sb.ToString());
            #endregion

            Sync_InternalOrderFromSAP();
            Sync_GLAccountFromSAP();
            SetCostCenter();
        }
    }

    public static void SetCostCenter()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        DateTime dNow = DateTime.Now;
        int nYear = dNow.Year;
        int nUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0;

        var lstCCTR = db.MT_CCTR_Structure.ToList();
        var lstCC = db.MT_CostCenter.Where(w => w.ValidFrom <= dNow && w.ValidTo >= dNow).ToList();
        var lstIO = db.TB_InternalOrder.Where(w => w.nYear == nYear).ToList();
        foreach (var item in lstCC)
        {
            string sCostCenterID = item.CostCenter;
            var qCC = db.TB_CostCenter.FirstOrDefault(w => w.sCostCenterID == sCostCenterID && w.nYear == nYear);
            if (qCC == null)
            {
                qCC = new TB_CostCenter();
                qCC.sCostCenterID = sCostCenterID;
                qCC.nYear = nYear;

                qCC.nCreateBy = nUserID;
                qCC.dCreate = dNow;
                db.TB_CostCenter.Add(qCC);
            }
            qCC.sCostCenterName = item.GeneralName;

            var qCCTR = lstCCTR.FirstOrDefault(w => w.ID == sCostCenterID);
            qCC.sCostCenterHeadID = qCCTR != null ? qCCTR.H1_APPROVAL.Substring(0, 8) : "";
            qCC.nBudget = lstIO.Where(w => w.sCostCenterID == sCostCenterID).Sum(s => s.nBudget);
            //qCC.nBudgetUsed = 0;
            qCC.nUpdateBy = nUserID;
            qCC.dUpdate = dNow;
            db.SaveChanges();
        }

    }

    public static void Sync_InternalOrderFromSAP()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        var lstMT_InternalOrder = db.MT_InternalOrder.ToList();
        var lstIOID = lstMT_InternalOrder.Select(s => s.IO).ToList();
        if (lstIOID.Any())
        {
            DateTime dNow = DateTime.Now;
            int nYear = dNow.Year;
            int nUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0;

            #region Get IO From SAP// Insert/Update TB_InternalOrder
            foreach (var sIOID in lstIOID)
            {
                var qMTIO = lstMT_InternalOrder.FirstOrDefault(w => w.IO == sIOID);
                string sCCID = qMTIO != null ? qMTIO.RespCCTR : "";
                string sIOName = qMTIO != null ? qMTIO.Description : "";

                var qIO = GetBudgetAvailableFromSAP(sIOID, nYear + "").FirstOrDefault();
                if (qIO != null)
                {
                    var qIO_ = db.TB_InternalOrder.FirstOrDefault(w => w.sIOID == sIOID && w.nYear == nYear);
                    if (qIO_ == null)
                    {
                        qIO_ = new TB_InternalOrder();
                        qIO_.sIOID = sIOID;
                        qIO_.nYear = nYear;
                        qIO_.nCreateBy = nUserID;
                        qIO_.dCreate = dNow;
                        db.TB_InternalOrder.Add(qIO_);
                    }
                    qIO_.sIOName = sIOName;
                    qIO_.sCostCenterID = sCCID;
                    qIO_.nBudget = qIO.Budget;
                    //qIO_.nBudgetUsed = 0;
                    qIO_.nUpdateBy = nUserID;
                    qIO_.dUpdate = dNow;
                    db.SaveChanges();
                }
            }
            #endregion

        }
    }

    public static void Sync_GLAccountFromSAP()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var lstIO = db.MT_InternalOrder.ToList();
        var lstIOID = lstIO.Select(s => s.IO).ToList();
        if (lstIOID.Any())
        {
            DateTime dNow = DateTime.Now;
            int nYear = dNow.Year;
            int nUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0;

            var lstGLSAP = new List<cGLAccount>();

            #region Get GL From SAP
            foreach (var sIOID in lstIOID)
            {
                var lstGL_ = GetBudgetAvailableGLFromSAP(sIOID, nYear + "").ToList();
                foreach (var itemGL in lstGL_)
                {
                    lstGLSAP.Add(new cGLAccount()
                    {
                        sGLID = itemGL.GLAccount,
                        sIOID = sIOID,
                        nBudget = CommonFunction.ParseDecimalZero(itemGL.Budget)
                    });
                }
            }
            #endregion

            var lstGLID = lstGLSAP.Select(s => s.sGLID).Distinct().ToList();
            DataTable dtMT_GLAccount = CommonFunction.Get_Data(CommonFunction.connBE, "select * from MT_GLAccount where sGLCode in ('" + (lstGLID.Any() ? string.Join("','", lstGLID) : "") + "') and sCompCode = '10'");
            var lstGL = CommonFunction.ConvertDatableToList<MT_GLAccount>(dtMT_GLAccount).ToList();

            if (lstGL.Any())
            {
                CommonFunction.ExecuteNonQuery("delete MT_GLAccount");

                StringBuilder sb = new StringBuilder();

                #region Script Insert MT_GLAccount
                string sInsert = @"INSERT INTO MT_GLAccount
                ([sGLCode]
                ,[sCompCode]
                ,[sGLDesc]
                ,[cStatus]
                ,[sGLFullDesc]
                ,[sUpdateBy]
                ,[sUpdateOn]
                ,[sGLOPXGroup])
                VALUES
                ({0},--<sGLCode, varchar(10),>
                {1},--<sCompCode, varchar(2),>
                {2},--<sGLDesc, varchar(max),>
                {3},--<cStatus, varchar(1),>
                {4},--<sGLFullDesc, varchar(max),>
                {5},--<sUpdateBy, varchar(20),>
                {6},--<sUpdateOn, datetime,>
                {7})--<sGLOPXGroup, varchar(2),>)" + Environment.NewLine;
                #endregion

                #region Insert MT_GLAccount
                foreach (var item in lstGL)
                {
                    sb.Append(string.Format(sInsert,
                         CommonFunction.ConvertStringSQLNULL(item.sGLCode),
                         CommonFunction.ConvertStringSQLNULL(item.sCompCode),
                         CommonFunction.ConvertStringSQLNULL(item.sGLDesc),
                         CommonFunction.ConvertStringSQLNULL(item.cStatus),
                         CommonFunction.ConvertStringSQLNULL(item.sGLFullDesc),
                         CommonFunction.ConvertStringSQLNULL(item.sUpdateBy),
                         "'" + item.sUpdateOn + "'",
                         CommonFunction.ConvertStringSQLNULL(item.sGLOPXGroup)
                        ));
                }

                CommonFunction.ExecuteNonQuery(sb.ToString());
                #endregion

                #region Insert/Update TB_GLAccount                
                foreach (var item in lstGLSAP)
                {
                    string sGLID = item.sGLID;
                    string sIOID = item.sIOID;
                    var qGL = db.TB_GLAccount.FirstOrDefault(w => w.sGLID == sGLID && w.sIOID == sIOID && w.nYear == nYear);
                    if (qGL == null)
                    {
                        qGL = new TB_GLAccount();
                        qGL.sGLID = sGLID;
                        qGL.nYear = nYear;
                        qGL.sIOID = sIOID;

                        qGL.nCreateBy = nUserID;
                        qGL.dCreate = dNow;

                        db.TB_GLAccount.Add(qGL);
                    }

                    var qIO = lstIO.FirstOrDefault(w => w.IO == sIOID);
                    qGL.sCostCenterID = qIO != null ? qIO.RespCCTR : "";

                    var qGLNAme = lstGL.FirstOrDefault(w => w.sGLCode == sGLID);
                    qGL.sGLName = qGLNAme != null ? qGLNAme.sGLDesc : "";
                    qGL.nBudget = item.nBudget;
                    //qGL.nBudgetUsed = 0;
                    qGL.nUpdateBy = nUserID;
                    qGL.dUpdate = dNow;
                    db.SaveChanges();
                }
                #endregion
            }
        }
    }

    public class cGLAccount
    {
        public string sGLID { get; set; }
        public string sIOID { get; set; }
        public decimal nActual { get; set; }
        public decimal nAssigned { get; set; }
        public decimal nAvailable { get; set; }
        public decimal nBudget { get; set; }
        public decimal nCommitment { get; set; }
    }
    #endregion

    public static List<SAPnBPC_Interface.ResponseCheckBudget> GetBudgetAvailableFromSAP(string sOrderNo, string sYear)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        SAPnBPC_Interface.CheckBudget chkBudget = new SAPnBPC_Interface.CheckBudget();
        SAPnBPC_Interface.RequestCheckBudget GLBudget = new SAPnBPC_Interface.RequestCheckBudget();
        SAPnBPC_Interface.RequestCheckBudget[] arrGLBudget = new SAPnBPC_Interface.RequestCheckBudget[1];

        var qIO = db.MT_InternalOrder.FirstOrDefault(w => w.IO == sOrderNo);
        if (qIO != null)
        {
            chkBudget.CaseMethod = qIO.IOType;
            GLBudget.caseMethod = qIO.IOType;
            GLBudget.InternalOrderNo = qIO.IO;//AUFNR
            GLBudget.InternalOrderID = qIO.ObjectNumber;//OBJNR
            GLBudget.InternalOrderType = qIO.IOType;//AUART
            if (qIO.IOType == "OPX1")
            {
                GLBudget.FiscalYear = sYear;
            }
        }

        arrGLBudget[0] = GLBudget;
        chkBudget.GLBudget = arrGLBudget;
        List<SAPnBPC_Interface.ResponseCheckBudget> budget = new SAPnBPC_Interface().GetBudget(chkBudget);
        return budget;
    }

    public static List<SAPnBPC_Interface.ResponseGLBudget> GetBudgetAvailableGLFromSAP(string sOrderNo, string sYear)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        SAPnBPC_Interface.CheckGLBudget chkBudget = new SAPnBPC_Interface.CheckGLBudget();
        SAPnBPC_Interface.RequestGLBudget GLBudget = new SAPnBPC_Interface.RequestGLBudget();

        var qIO = db.MT_InternalOrder.FirstOrDefault(w => w.IO == sOrderNo);
        if (qIO != null)
        {
            chkBudget.CaseMethod = qIO.IOType;
            GLBudget.InternalOrderNo = qIO.IO;//AUFNR
            GLBudget.InternalOrderID = qIO.ObjectNumber;//OBJNR
            GLBudget.InternalOrdertype = qIO.IOType;//AUART
            if (qIO.IOType == "OPX1")
            {
                GLBudget.FiscalYear = sYear;
            }
        }

        chkBudget.GLBudget = GLBudget;
        List<SAPnBPC_Interface.ResponseGLBudget> budget = new SAPnBPC_Interface().GetGLBudget(chkBudget);
        return budget;
    }

    public static void UpdateLog(int nMenuID, string sMenuName, string sEvent)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qMenu = db.Database.SqlQuery<TM_Menu>("select top 1 * from TM_Menu where nMenuID = " + nMenuID).FirstOrDefault();
        //long nLogNo = qMax != null ? qMax.nLogID + 1 : 1;

        string sInsert = @"INSERT INTO TM_Log
           ([dLog]
           ,[nUserID]
           ,[nMenuID]
           ,[sMenuName]
           ,[sEvent])
            VALUES
           ('" + DateTime.Now + @"'
           ," + (!UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0) + @"
           ," + nMenuID + @"
           ,'" + (sMenuName != "" ? sMenuName : (qMenu != null ? qMenu.sMenuName + "" : "")) + @"'
           ,'" + sEvent + "')";

        CommonFunction.ExecuteNonQuery(sInsert);
    }

    public static bool IsAdmin(int? nUserID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        if (!nUserID.HasValue) nUserID = UserAccount.SessionInfo.nUserID;

        return db.TB_User.Any(w => w.nUserID == nUserID && w.nRole == 1 && w.IsActive && !w.IsDel);
    }

    public static bool IsStaff(int? nUserID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        if (!nUserID.HasValue) nUserID = UserAccount.SessionInfo.nUserID;

        return db.TB_User.Any(w => w.nUserID == nUserID && w.nRole == 2 && w.IsActive && !w.IsDel);
    }

    public static bool IsApprover(int? nUserID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        if (!nUserID.HasValue) nUserID = UserAccount.SessionInfo.nUserID;

        return db.TB_User.Any(w => w.nUserID == nUserID && w.nRole == 3 && w.IsActive && !w.IsDel);
    }

    public static string GetAppover(int nUserID)
    {
        string sRet = "";
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID);
        if (qUser != null)
        {
            string sUserID = qUser.sUserID;
            var lstApp1 = HR_WebService.EmployeeService_EmployeeID(sUserID).d.results.ToList();
            foreach (var item1 in lstApp1)
            {
                var lstApp2 = HR_WebService.EmployeeService_EmployeeID(item1.ManagerID).d.results.ToList();
                foreach (var item2 in lstApp2)
                {
                    if (item1.ManagerPositionID == item2.PositionID && (item2.PositionLevel == "45" || item2.PositionLevel == "40"))
                    {
                        //40 DM, 45 SM
                        sRet = item2.EmployeeID;
                        if (item2.PositionLevel == "40")
                        {
                            return sRet;
                        }
                    }
                }
            }
        }
        return sRet;
    }

    public static void SetLogProject(int nProjectID, int nStatusID, string sComment)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        db.T_Project_Approve.Add(new T_Project_Approve()
        {
            nProjectID = nProjectID,
            nStatusID = nStatusID,
            sComment = sComment,
            nActionBy = UserAccount.SessionInfo.nUserID,
            dAction = DateTime.Now
        });
        db.SaveChanges();
    }

    public static string GetUserID(int? nUserID)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        if (!nUserID.HasValue) nUserID = UserAccount.SessionInfo.nUserID;

        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.IsActive && !w.IsDel);
        return qUser != null ? qUser.sUserID : "";
    }

    public static string GetFirstNameNotAbbr(string sFName)
    {
        return sFName.Replace("นาง", "").Replace("นางสาว", "").Replace("น.ส.", "").Replace("นาย", "").Replace(" ", "");
    }

    #region Mail
    public static void SendMailProject(int nStep, T_Project qPro)
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        string _to = "", _cc = "", subject = "", sTitleName = "", message = "", sText = "";
        string sTemplate = CSR_Function.GET_TemplateEmail();
        TB_User qUser = new TB_User();

        switch (nStep)
        {
            case 0://Assign Project Owner
                qUser = db.TB_User.FirstOrDefault(w => w.nUserID == qPro.nOwnerID);
                _to = qUser != null ? qUser.sEmail : "";
                sTitleName = qUser != null ? GetFirstNameNotAbbr(qUser.sFirstname) + " " + qUser.sLastname : "";
                subject = "PTTGC-CSR | ผู้ดูแลระบบได้เพิ่มคุณเป็นผู้รับผิดชอบโครงการ";
                sText = "ผู้ดูแลระบบได้เพิ่มคุณเป็นผู้รับผิดชอบโครงการ " + qPro.sProjectName;
                break;
            case 1://ส่งอนุมัติ
                qUser = db.TB_User.FirstOrDefault(w => w.sUserID == qPro.sApproverID);
                if (qUser != null)
                {
                    _to = qUser != null ? qUser.sEmail : "";
                    sTitleName = qUser != null ? GetFirstNameNotAbbr(qUser.sFirstname) + " " + qUser.sLastname : "";
                }
                else
                {
                    var qApprover = HR_WebService.EmployeeService_EmployeeID(qPro.sApproverID).d.results.FirstOrDefault();
                    _to = qApprover != null ? qApprover.EmailAddress : "";
                    sTitleName = qApprover != null ? GetFirstNameNotAbbr(qApprover.THFirstName) + " " + qApprover.THLastName : "";
                }

                var lstCC = db.TB_User.Where(w => w.IsActive && !w.IsDel && w.sEmail + "" != "" && w.nRole == 1).Select(s => s.sEmail).ToList();
                _cc = lstCC.Any() ? string.Join(",", lstCC) : "";

                subject = "PTTGC-CSR | โครงการถูกส่งอนุมัติ";
                sText = qPro.sProjectName + " ถูกส่งอนุมัติ";
                break;
            case 2://อนุมัติ
                qUser = db.TB_User.FirstOrDefault(w => w.nUserID == qPro.nOwnerID);
                _to = qUser != null ? qUser.sEmail : "";
                sTitleName = qUser != null ? GetFirstNameNotAbbr(qUser.sFirstname) + " " + qUser.sLastname : "";
                subject = "PTTGC-CSR | โครงการถูกอนุมัติ";
                sText = qPro.sProjectName + " ถูกอนุมัติ";
                break;
            case 3://ไม่อนุมัติ
                qUser = db.TB_User.FirstOrDefault(w => w.nUserID == qPro.nOwnerID);
                _to = qUser != null ? qUser.sEmail : "";
                sTitleName = qUser != null ? GetFirstNameNotAbbr(qUser.sFirstname) + " " + qUser.sLastname : "";
                subject = "PTTGC-CSR | โครงการไม่ได้รับการอนุมัติ";
                sText = qPro.sProjectName + " ไม่ได้รับการอนุมัติ";
                break;
            case 4://ส่งกลับแก้ไข
                qUser = db.TB_User.FirstOrDefault(w => w.nUserID == qPro.nOwnerID);
                _to = qUser != null ? qUser.sEmail : "";
                sTitleName = qUser != null ? GetFirstNameNotAbbr(qUser.sFirstname) + " " + qUser.sLastname : "";
                subject = "PTTGC-CSR | โครงการถูกส่งกลับแก้ไข";
                sText = qPro.sProjectName + " ถูกส่งกลับแก้ไข";
                break;
            case 5://ปิดโครงการ
                var lstAdmin = db.TB_User.Where(w => !w.IsDel && w.IsActive && w.nRole == 1).Select(s => s.sEmail).ToList();
                _to = lstAdmin.Any() ? string.Join(",", lstAdmin) : "";
                sTitleName = "ผู้ดูแลระบบ";
                subject = "PTTGC-CSR | โครงการถูกปิดโครงการ";
                sText = qPro.sProjectName + " ถูกปิดโครงการ";
                break;
            case 6://ระงับโครงการ
                var lstAdmin1 = db.TB_User.Where(w => !w.IsDel && w.IsActive && w.nRole == 1).Select(s => s.sEmail).ToList();
                _to = lstAdmin1.Any() ? string.Join(",", lstAdmin1) : "";
                sTitleName = "ผู้ดูแลระบบ";
                sTitleName = "ผู้ดูแลระบบ";
                subject = "PTTGC-CSR | โครงการถูกระงับโครงการ";
                sText = qPro.sProjectName + " ถูกระงับโครงการ";
                break;
            default:
                break;
        }

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

        SendNetMail("", _to, _cc, subject, message, new List<string>());
    }

    public static bool SendNetMail(string sfrom, string sto, string scc, string subject, string message, List<string> lstFile)
    {
        bool IsSend = false;
        string sMessage_Error = "";
        try
        {
            if (!CommonFunction.IsUseRealMail())//Demo
            {
                //message += " to : " + sto + " cc : " + scc;
                sfrom = ConfigurationSettings.AppSettings["DemoMail_Sender"].ToString();
                sto = ConfigurationSettings.AppSettings["DemoMail_Reciever"].ToString();
                scc = "";
            }
            else
            {
                if (string.IsNullOrEmpty(sfrom))
                {
                    sfrom = ConfigurationSettings.AppSettings["SystemMail"].ToString();
                }
            }

            System.Net.Mail.MailMessage oMsg = new System.Net.Mail.MailMessage();

            // TODO: Replace with sender e-mail address. 
            oMsg.From = new System.Net.Mail.MailAddress(sfrom, "PTTGC CSR");

            // TODO: Replace with recipient e-mail address.
            string[] Arrsto = sto.Split(',');
            for (int i = 0; i < Arrsto.Length; i++)
            {
                if (!string.IsNullOrEmpty(Arrsto[i]))
                {
                    oMsg.To.Add(Arrsto[i]);
                }
            }

            // Add CC Mail
            string[] ArrsCc = scc.Split(',');
            for (int i = 0; i < ArrsCc.Length; i++)
            {
                if (!string.IsNullOrEmpty(ArrsCc[i]))
                {
                    oMsg.CC.Add(ArrsCc[i]);
                }
            }

            // TODO: Replace with subject.
            oMsg.Subject = subject;

            // SEND IN HTML FORMAT (comment this line to send plain text).
            oMsg.IsBodyHtml = true;

            oMsg.Body = "<HTML><BODY>" + message + "</BODY></HTML>";

            if (lstFile.Any())
            {
                lstFile.ForEach(f =>
                {
                    if (!string.IsNullOrEmpty(f))
                    {
                        string[] arr = f.Split('/');
                        string filename = arr[arr.Length - 1];
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(f);
                        attachment.Name = filename;  // set name here
                                                     //msg.Attachments.Add(attachment);
                                                     // oMsg.Attachments.Add(new System.Net.Mail.Attachment(f));
                        oMsg.Attachments.Add(attachment);
                    }
                });
            }
            // ADD AN ATTACHMENT.
            // TODO: Replace with path to attachment.
            // String sFile = @"D:\FTP\username\Htdocs\Hello.txt";
            // String sFile = @sfilepath;
            // MailAttachment oAttch = new MailAttachment(sfilepath, MailEncoding.Base64);
            // oMsg.Attachments.Add(oAttch);

            // TODO: Replace with the name of your remote SMTP server.
            /**/

            // TODO: Replace with the name of your remote SMTP server.
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Port = 25;
            smtp.Host = ConfigurationSettings.AppSettings["smtpmail"];
            smtp.Send(oMsg);
            oMsg = null;
            IsSend = true;//return true;
        }
        catch (Exception e)
        {
            IsSend = false; //return false;
            sMessage_Error = e.Message;
        }

        LogMailSend(sto, scc, subject, message, IsSend, sMessage_Error);

        return IsSend;
    }

    public static void LogMailSend(string sTo, string sCc, string sSubject, string sMessage, bool IsSend, string sMessage_Error)
    {
        // Encode the content for storing in Sql server.
        string htmlEncoded = WebUtility.HtmlEncode(sMessage);

        // Decode the content for showing on Web page.
        //string original = WebUtility.HtmlDecode(htmlEncoded);

        PTTGC_CSREntities db = new PTTGC_CSREntities();

        var tb = new TM_LogMail();
        tb.sTo = sTo;
        tb.sCc = sCc;
        tb.sSubject = sSubject;
        tb.sMessage = sMessage;
        tb.IsSend = IsSend;
        tb.sMessage_Error = !string.IsNullOrEmpty(sMessage_Error) ? sMessage_Error : null;
        tb.dSend = DateTime.Now;

        db.TM_LogMail.Add(tb);
        db.SaveChanges();
    }

    public static string GET_TemplateEmail()
    {
        return @"<div id=':km' class='ii gt adP adO'>
                <div id=':l9' class='a3s aXjCH m15f05c377e26ea4b'>
                    <u></u>
                    <div style='background: #f9f9f9'>
                        <div style='background-color: #f9f9f9'>

                            <div style='margin: 0px auto; /* max-width: 630px; */background: transparent;'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent;' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; /* padding: 40px 0px */'>
                                                <div style='font-size: 1px; line-height: 12px'>&nbsp;{4}</div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style='max-width: 640px; margin: 0 auto; border-radius: 4px; overflow: hidden'>
                                <div style='margin: 0px auto; max-width: 640px; background: #ffffff'>
                                    <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: #ffffff' align='center' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 40px 70px'>
                                                    <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                        <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>

                                                                            <h2 style='font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-weight: 500; font-size: 20px; color: #4f545c; letter-spacing: 0.27px'>{0}</h2>
                                                                            {1}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                {2}
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>
                                                                            {5}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding-top: 15px;'>
                                                                        <p style='font-size: 1px; margin: 0px auto; border-top: 1px solid #dcddde; width: 100%'></p>
                                                                    </td>
                                                                </tr>
                                                                <tr style='display:none;'>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #747f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 13px; line-height: 16px; text-align: left'>
                                                                            <p>
                                                                                {3}
                                                                            </p>
                                                                            <p>
                                                                                Best regards,<br>
                                                                                Technology Management System Team
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div style='margin: 0px auto; max-width: 640px; background: transparent'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 0px'>
                                                <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                    <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                        <tbody>
                                                            <tr>
                                                                <td style='word-break: break-word; font-size: 0px'>
                                                                    <div style='font-size: 1px; line-height: 12px'>&nbsp;</div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>";
    }
    #endregion
}