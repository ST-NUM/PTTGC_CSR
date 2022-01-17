<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fileall_directory.aspx.cs" Inherits="fileall_directory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtPath" runat="server" Width="300px"></asp:TextBox>
            <asp:Button ID="btn" runat="server" Text="Click" OnClick="btn_Click" />
            <asp:GridView ID="gvw" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemTemplate>
                            <asp:Label ID="lblF" runat="server" Text='<%#Bind("FileName") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="90%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Date">
                        <ItemTemplate>
                            <asp:Label ID="lblD" runat="server" Text='<%#Bind("sDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
