using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnQuery.Click += BtnQuery_Click;
    }

    private void BtnQuery_Click(object sender, EventArgs e)
    {
        string conn = WebConfigurationManager.ConnectionStrings["PTTGC_CSR_ConnectionString"].ConnectionString.ToString();

        if (!string.IsNullOrEmpty(txtQuery.Text))
        {
            dgd.DataSource = CommonFunction.Get_Data(conn, CommonFunction.ReplaceInjection(txtQuery.Text));
            dgd.DataBind();

        }
    }
}