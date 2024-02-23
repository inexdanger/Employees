<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="EmployeesApp.Report" %>

<!DOCTYPE html>
<html>
<head>
    <title>Reporte</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            padding-top: 60px;
        }
        .container {
            max-width: 960px;
        }
        h1 {
            margin-top: 20px;
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
        <div class="container">
            <a class="navbar-brand" href="/">Proyecto Employees</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="Employees.aspx">Employees</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="Report.aspx">Report</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <form runat="server">
        <div class="container">
            <h1>Catalogo</h1>
            <asp:DropDownList ID="departmentDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="departmentDropDownChanged" CssClass="form-control">
            </asp:DropDownList>
            <br/><br/>
            <asp:GridView ID="employeesGrid" runat="server" CssClass="table table-striped">
            </asp:GridView>
        </div>
        <br/>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
</body>
</html>