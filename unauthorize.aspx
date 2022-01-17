<%@ Page Language="C#" AutoEventWireup="true" CodeFile="unauthorize.aspx.cs" Inherits="unauthorize" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no" />

    <title></title>

    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="Images/ico/ptt_weblogo-144.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="Images/ico/ptt_weblogo-114.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="Images/ico/ptt_weblogo-72.png" />
    <link rel="apple-touch-icon-precomposed" sizes="57x57" href="Images/ico/ptt_weblogo-57.png" />
    <link rel="shortcut icon" href="Images/ico/ptt_weblogo-favicon.png" />

    <script src="Scripts/jquery.min.js"></script>

    <%--Bootstrap--%>
    <link href="Styles/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Styles/bootstrap/js/popper.min.js"></script>
    <script src="Styles/bootstrap/js/bootstrap.min.js"></script>
    <script src="Styles/bootstrap/js/bootstrap.bundle.min.js"></script>

    <style type="text/css">
        .bgMain {
            background-image: url('Images/bg-main.jpg');
            background-repeat: no-repeat;
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
            background-position: top center;
            background-attachment: fixed;
            width: 100%;
        }

        .p10 {
            padding-top: 10%;
        }
    </style>
</head>
<body class="bgMain">
    <form id="form1" runat="server">
        <div class="text-center p10">
            <div class="jumbotron" style="background-color: transparent;">
                <h1>ไม่ได้รับอนุญาตในการเข้าถึง</h1>
                <p>กรุณาติดต่อผู้ดูแลระบบ</p>
            </div>
        </div>
    </form>
</body>
</html>
