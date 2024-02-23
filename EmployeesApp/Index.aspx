<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="EmployeesApp.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASP.NET Web Forms Ejemplo</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnSaludar" runat="server" Text="Saludar" OnClick="btnSaludar_Click" />
            <br />
            <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>