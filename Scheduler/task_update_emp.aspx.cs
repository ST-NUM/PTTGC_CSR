using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class task_update_emp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CommonFunction.SetLog(0, "", "Run task Update Employee");

            UpdateEmployee();
        }
        catch (Exception ex)
        {
            CommonFunction.SetLog(0, "", "Task Update Employee Error " + ex.Message);
        }
        finally
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "window.opener = 'Self';window.open('','_parent',''); window.close();", true);
        }
    }


    public static void UpdateEmployee()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();

        var lstEmp = db.TB_User.Where(w => w.IsGC && !w.IsDel).ToList();
        if (lstEmp.Any())
        {
            foreach (var item in lstEmp)
            {
                var qEmp_ = HR_WebService.EmployeeService_Search(item.sUserID, "", new List<string>()).d.results.FirstOrDefault();
                if (qEmp_ != null)
                {
                    item.sFirstname = qEmp_.NameTH.Split(' ')[0] + " " + qEmp_.THFirstName;
                    item.sLastname = qEmp_.THLastName;
                    item.sEmail = qEmp_.EmailAddress;
                }
            }
            db.SaveChanges();
        }
    }
}