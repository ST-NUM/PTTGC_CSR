<%@ Page Language="C#" AutoEventWireup="true" CodeFile="query.aspx.cs" Inherits="query" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
            <asp:Button ID="btnQuery" Text="query" runat="server" />
        </div>

        <div>
            <asp:DataGrid ID="dgd" runat="server" AutoGenerateColumns="true">
            </asp:DataGrid>
        </div>       


    </form>
</body>
</html>
