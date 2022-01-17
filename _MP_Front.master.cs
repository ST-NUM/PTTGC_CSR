using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _MP_Front : System.Web.UI.MasterPage
{
    protected string sName = "";
    protected string sRole = "";

    public HtmlGenericControl SetBody
    {
        get { return ((_MP_AllSource)Master).MainBody; }
    }

    public void SetBodyEventOnload(string sJsFunction)
    {
        ((_MP_AllSource)Master).SetBodyEventOnload(sJsFunction);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (UserAccount.IsExpired)
            {
                SetBodyEventOnload("PopupSessionTimeOut();");
                hidSessionTimeOut.Value = "Y";
            }
            else
            {
                sName = UserAccount.SessionInfo.sName;
                sRole = UserAccount.SessionInfo.sRoleName;

                SetMenu();
            }
        }
    }

    public void SetMenu()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        List<DATA_Menu> lstMenu = new List<DATA_Menu>();

        int nMenuID_Selected = MenuID_Selected;
        var lstTM_Menu = db.TM_Menu.Where(w => w.nMenuID != 15 ? w.IsActive : true).OrderBy(o => o.nMenuOrder).ToList();
        var lstMenuID = lstTM_Menu.Select(s => s.nMenuID).ToList();
        var lstMenuLV1 = lstTM_Menu.Where(w => w.nLevel == 1).ToList();

        var IsApprover = CSR_Function.IsApprover(UserAccount.SessionInfo.nUserID);

        int nUserID = UserAccount.SessionInfo.nUserID;
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.IsActive && !w.IsDel);
        var lstMenuHasPerID = db.TB_User_Permission.Where(w => w.nUserID == nUserID &&
        ((lstMenuID.Contains(w.nMenuID) && w.nPermission > 0) || (IsApprover ? w.nMenuID == 2 : false))).Select(s => s.nMenuID).ToList();
        if (qUser != null && lstMenuHasPerID.Any())
        {
            hddContract.Value = qUser.IsGC ? "N" : "Y";

            foreach (var item in lstMenuLV1)
            {
                var lstTM_MenuSub = lstTM_Menu.Where(w => w.nMenuHead == item.nMenuID).ToList();
                if (lstTM_MenuSub.Any())
                {
                    lstTM_MenuSub = lstTM_MenuSub.Where(w => lstMenuHasPerID.Contains(w.nMenuID)).ToList();
                    if (lstTM_MenuSub.Any())
                    {
                        DATA_Menu _Menu = new DATA_Menu() { sIcon = "<i class='fa " + item.sIcon + "'></i>", sName = item.sMenuName };

                        var lstMenuSub = new List<DATA_Menu>();
                        foreach (var item1 in lstTM_MenuSub)
                        {
                            lstMenuSub.Add(new DATA_Menu() { sIcon = "<i class='fa " + item1.sIcon + "'></i>", sName = item1.sMenuName, sURL = item1.sMenuLink, isSelected = nMenuID_Selected == item1.nMenuID });
                        }

                        _Menu.lstSubMenu = lstMenuSub;

                        lstMenu.Add(_Menu);
                    }
                }
                else if (lstMenuHasPerID.Contains(item.nMenuID) || item.nMenuID == 15)
                {
                    string sLink = item.sMenuLink;
                    if (item.nMenuID == 15)
                    {
                        var qEmail = db.TM_MasterData.FirstOrDefault(w => w.nMainID == 10);
                        string sEmail = qEmail != null ? qEmail.sDescription : "";

                        var qSubject = db.TM_MasterData.FirstOrDefault(w => w.nMainID == 11);
                        string sSubject = qSubject != null ? qSubject.sDescription : "";

                        sLink = "mailto:" + sEmail + "?subject=" + sSubject;
                    }
                    lstMenu.Add(new DATA_Menu() { sIcon = "<i class='fa " + item.sIcon + "'></i>", sName = item.sMenuName, sURL = sLink, isSelected = nMenuID_Selected == item.nMenuID });
                }
            }
        }

        menuTop.InnerHtml = string.Join("", lstMenu.Select(s => s.Build(false)).ToArray());
        menuSide.InnerHtml = string.Join("", lstMenu.Select(s => s.Build(true)).ToArray());
    }

    public int MenuID_Selected
    {
        get
        {
            int n = 0;
            int.TryParse(hidMenuID.Value, out n);
            return n;
        }
        set { hidMenuID.Value = value.ToString(); }
    }

    private class DATA_Menu
    {
        public string sIcon { get; set; }
        public string sName { get; set; }
        public string sURL { get; set; }
        public bool isSelected { get; set; }
        public List<DATA_Menu> lstSubMenu { get; set; }

        public DATA_Menu() { lstSubMenu = new List<DATA_Menu>(); }

        public string Build(bool isSideNav)
        {
            string sMenuSub = "";
            bool hasChildren = false;
            hasChildren = lstSubMenu.Any();
            if (hasChildren)
            {
                lstSubMenu.ForEach(m => sMenuSub += m.Build(isSideNav));
                sMenuSub =
                    "<ul class='menu-sub'>" +
                        (isSideNav ? "<li><a class='link-back'><i class='fa fa-chevron-left'></i> Back</a></li>" : "") +
                        sMenuSub +
                    "</ul>";
            }

            return
                "<li" + (hasChildren ? " class='has-children'" : "") + ">" +
                    "<a" + (!string.IsNullOrEmpty(sURL) ? " href='" + sURL + "'" : "") + (isSelected ? " class='active'" : "") + ">" +
                        sIcon + " " + sName +
                        (isSideNav && hasChildren ? "<div class='link-caret'><i class='fa fa-chevron-right'></i></div>" : "") +
                    "</a>" +
                    sMenuSub +
                "</li>";
        }
    }
}
