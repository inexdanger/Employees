<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="EmployeesApp.Report" %>


<!DOCTYPE html>
<html>
<head>
    <title>Reporte</title>
</head>
<body>
    <form runat="server">
        <div>
            <h1>Catalogo</h1>
            <asp:DropDownList ID="departmentDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="departmentDropDownChanged">
            </asp:DropDownList><br/><br/>
            <asp:GridView ID="employeesGrid" runat="server">
            </asp:GridView>
        </div>
    </form>
</body>
    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
    <script>
        var employeeId = document.getElementById("employeeId")
        var method = ''
        var url = ''

        function Submit() {
            if (employeeId.value != '') {
                url = '/Employees.aspx/UpdateEmployee'                
            } else {
                url = '/Employees.aspx/InsertEmployee'
            }
            axios({
                method: 'POST',
                url: url,
                data: {
                    names: namesInput.value,
                    dpi: dpiInput.value,
                    bornDate: bornDateInput.value,
                    sex: sexInput.value,
                    dateEntryCompany: dateEntryCompanyInput.value,
                    address: addressInput.value,
                    nit: nitInput.value,
                    department: departmentInput.value,
                    employeeID: employeeId.value,
                }
            }).then(function (response) {
                  console.log(response);
            }).catch(function (error) {
                console.error(error);
            });
        }

        function getEmployeeData() { 
            if (employeeId.value == '') { return };
            axios.post('/Employees.aspx/GetEmployeeData', {
               employeeID: employeeId.value,
            }).then(function (response) {
                var responseData = JSON.parse(response.data.d);
                namesInput.value = responseData.Names
                dpiInput.value = responseData.DPI
                bornDateInput.value = responseData.BornDate
                sexInput.value = responseData.Sex
                dateEntryCompanyInput.value = responseData.DateEntryCompany
                addressInput.value = responseData.Address
                nitInput.value = responseData.NIT
                departmentInput.value = responseData.Department
            }).catch(function (error) {
                console.error(error);
            });
        }
    </script>
</html>