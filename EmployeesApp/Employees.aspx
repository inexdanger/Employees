<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="EmployeesApp.Employees" %>

<!DOCTYPE html>
<html>
<head>
    <title>Formulario de Empleados</title>
</head>
<body>

    <form runat="server">
       
    <h2>Ingrese los datos del empleado:</h2>
         <label for="employeeId">EmployeeId:</label><br>
         <input type="text" id="employeeId" name="employeeId" onchange="getEmployeeData()"><br>

        <label for="names">Names:</label><br>
        <input type="text" id="names" name="names"><br>

        <label for="dpi">DPI:</label><br>
        <input type="text" id="dpi" name="dpi" ><br>

        <label for="bornDate">Born Date:</label><br>
        <input type="date" id="bornDate" name="bornDate"><br>

        <label for="sex">Sex:</label><br>
        <select id="sex" name="sex">
            <option value="M">Masculino</option>
            <option value="F">Femenino</option>
        </select>
        <br />

        <label for="dateEntryCompany">Date Entry Company:</label><br>
        <input type="date" id="dateEntryCompany" name="dateEntryCompany"><br>

        <label for="address">Address:</label><br>
        <input type="text" id="address" name="address"><br>

        <label for="nit">Nit:</label><br>
        <input type="text" id="nit" name="nit"><br>


        <label for="department">Department:</label><br>
        <asp:DropDownList ID="department" runat="server">
        </asp:DropDownList>
        
        <button type="button" onclick="Submit()">Submit</button>

        </form>
    </body>

    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
    <script>
        var namesInput = document.getElementById("names");
        var dpiInput = document.getElementById("dpi");
        var bornDateInput = document.getElementById("bornDate");
        var sexInput = document.getElementById("sex");
        var dateEntryCompanyInput = document.getElementById("dateEntryCompany");
        var addressInput = document.getElementById("address");
        var nitInput = document.getElementById("nit");
        var departmentInput = document.getElementById('<%= department.ClientID %>');
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
            })
                .then(function (response) {
                    var responseData = JSON.parse(response.data.d);
                    namesInput.value = responseData.Names
                    dpiInput.value = responseData.DPI
                    bornDateInput.value = responseData.BornDate
                    sexInput.value = responseData.Sex
                    dateEntryCompanyInput.value = responseData.DateEntryCompany
                    addressInput.value = responseData.Address
                    nitInput.value = responseData.NIT
                    departmentInput.value = responseData.Department
                })
                .catch(function (error) {
                    console.error(error);
                });
        }
    </script>
</html>