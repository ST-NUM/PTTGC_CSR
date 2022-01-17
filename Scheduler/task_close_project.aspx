<%@ Page Language="C#" AutoEventWireup="true" CodeFile="task_close_project.aspx.cs" Inherits="task_close_project" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Scripts/jquery.min.js"></script>

    <script type="text/javascript">
        function window_close() {

            window.opener = window.self;
            window.open('', '_self');
            window.close();
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
