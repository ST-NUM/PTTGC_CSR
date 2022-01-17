<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="System.IO" %>
<%@ Application Language="C#" %>
<script RunAt="server">
    public System.Data.SqlClient.SqlConnection _conn = new System.Data.SqlClient.SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

        // Misc
        FixAppDomainRestartWhenTouchingFiles();
        // Assign Application_Error as a callback error handler for DevExpress web controls 
        //DevExpress.Web.ASPxClasses.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception err = null;
        try
        {
            err = (Exception)Server.GetLastError().InnerException;
        }
        catch { err = null; }
        if (err != null)
        {
            ///Paths File
            string strPathName = "./UploadFiles/ErrorLog/";

            #region Create Directory (เช็ค และสร้างไดเร็คเตอรี)
            if (!Directory.Exists(Server.MapPath(strPathName)))
            {
                Directory.CreateDirectory(Server.MapPath(strPathName));
            }
            #endregion
            ///Create a text file containing error details
            string strFileName = DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("th-TH")) + ".txt";
            string errorText = "\r\nError Message: " + err.Message + "\r\nStack Trace: " + err.StackTrace + "\r\n";
            string logMessage = String.Format(@"[{0}] :: {1} ", DateTime.Now.ToString("HH:mm:ss", new System.Globalization.CultureInfo("th-TH")), errorText);
            FileInfo file = new FileInfo(Request.MapPath(strPathName + strFileName));
            StreamWriter _sw = file.AppendText();
            _sw.Write(logMessage);
            _sw.Write(_sw.NewLine);
            _sw.WriteLine(("*").PadLeft(logMessage.Length, '*'));
            _sw.Write(_sw.NewLine);
            _sw.Close();

            #region insert Error Log to DB
            /* TLogError errorlog = new TLogError(null, _conn);
        errorlog.sError = logMessage;
        errorlog.sUserID = (Session["suserid"]==null || Session["suserid"].ToString().Equals("")) ? "Session Expire" : Session["suserid"].ToString();
        errorlog.dLog = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss", new System.Globalization.CultureInfo("en-US"));
        errorlog.sWorkFlow = "";
        errorlog.Insert();*/
            #endregion
        }

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised. 
        //Response.Write(@"<script>alert('เนื่องจากคุณไม่ได้ใช้งานระบบเกินเวลาที่กำหนดไว้กรุณา Login เพื่อใช้งานอีกครั้ง');<\/script>");
    }
    private void FixAppDomainRestartWhenTouchingFiles()
    {
        if (GetCurrentTrustLevel() == AspNetHostingPermissionLevel.Unrestricted)
        {
            // From: http://forums.asp.net/p/1310976/2581558.aspx
            // FIX disable AppDomain restart when deleting subdirectory
            // This code will turn off monitoring from the root website directory.
            // Monitoring of Bin, App_Themes and other folders will still be operational, so updated DLLs will still auto deploy.
            System.Reflection.PropertyInfo p = typeof(HttpRuntime).GetProperty("FileChangesMonitor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            object o = p.GetValue(null, null);
            System.Reflection.FieldInfo f = o.GetType().GetField("_dirMonSubdirs", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            System.Reflection.MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            m.Invoke(monitor, new object[] { });
        }
    }
    private AspNetHostingPermissionLevel GetCurrentTrustLevel()
    {
        foreach (AspNetHostingPermissionLevel trustLevel in
                new AspNetHostingPermissionLevel[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal 
            })
        {
            try
            {
                new AspNetHostingPermission(trustLevel).Demand();
            }
            catch (System.Security.SecurityException)
            {
                continue;
            }

            return trustLevel;
        }

        return AspNetHostingPermissionLevel.None;
    }   
</script>
