using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class task_close_project : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CommonFunction.SetLog(0, "", "Run task Close Project");

            CloseProject();
        }
        catch (Exception ex)
        {
            CommonFunction.SetLog(0, "", "Task Close Project Error " + ex.Message);
        }
        finally
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "window.opener = 'Self';window.open('','_parent',''); window.close();", true);
        }
    }


    public static void CloseProject()
    {
        var dNow = DateTime.Now.Date;
        var dClose = new DateTime(dNow.Year, 1, 1);
        if (dNow == dClose)
        {
            PTTGC_CSREntities db = new PTTGC_CSREntities();
            int nYearClose = dNow.Year - 1;
            var lstProject = db.T_Project.Where(w => !w.IsDel && w.IsPassApprove && w.nStatusID == 2 && w.nYear == nYearClose).ToList();
            if (lstProject.Any())
            {
                foreach (var item in lstProject)
                {
                    item.nStatusID = 5;

                    db.T_Project_Approve.Add(new T_Project_Approve()
                    {
                        nProjectID = item.nProjectID,
                        nStatusID = 5,
                        sComment = "ปิดโครงการโดยระบบ",
                        nActionBy = null,
                        dAction = DateTime.Now
                    });
                }
                db.SaveChanges();
            }
        }
    }
}