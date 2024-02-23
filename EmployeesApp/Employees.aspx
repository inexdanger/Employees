<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="EmployeesApp.Employees" %>
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

    <div class="container">
        <form runat="server">
            <h2>Ingrese los datos del empleado:</h2>
            <label for="action">Acción:</label><br/>
            <select id="action" name="action" onchange="toggleEmployeeId()">
                <option value="insert">Ingresar usuario</option>
                <option value="update">Actualizar usuario</option>
            </select>
            <br />
            <div id="employeeIdDiv" style="display:none;">
                <label for="employeeId">Id Empleado:</label><br/>
                <input type="text" id="employeeId" name="employeeId" onchange="getEmployeeData()"><br/>
            </div>

            <label for="names">Names:</label><br/>
            <input type="text" id="names" name="names"><br/>

            <label for="dpi">DPI:</label><br/>
            <input type="text" id="dpi" name="dpi"><br/>

            <label for="bornDate">Born Date:</label><br/>
            <input type="date" id="bornDate" name="bornDate"><br/>

            <label for="sex">Sex:</label><br/>
            <select id="sex" name="sex">
                <option value="M">Masculino</option>
                <option value="F">Femenino</option>
            </select>
            <br />

            <label for="dateEntryCompany">Date Entry Company:</label><br/>
            <input type="date" id="dateEntryCompany" name="dateEntryCompany"><br/>

            <label for="address">Address:</label><br/>
            <input type="text" id="address" name="address"><br/>

            <label for="nit">Nit:</label><br/>
            <input type="text" id="nit" name="nit"><br/>

            <label for="department">Department:</label><br/>
            <asp:DropDownList ID="department" runat="server"></asp:DropDownList>
            <br/>
            <label for="status">Status:</label><br/>
            <asp:DropDownList ID="status" runat="server"></asp:DropDownList>
            <br/>
            <br/>
            <button type="button" Class="btn btn-primary" onclick="Submit()">Submit</button>
        </form>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.min.js"></script>
    <script>
        var namesInput = document.getElementById("names")
        var dpiInput = document.getElementById("dpi")
        var bornDateInput = document.getElementById("bornDate")
        var sexInput = document.getElementById("sex")
        var dateEntryCompanyInput = document.getElementById("dateEntryCompany")
        var addressInput = document.getElementById("address")
        var nitInput = document.getElementById("nit")
        var departmentInput = document.getElementById('<%= department.ClientID %>')
        var employeeId = document.getElementById("employeeId")
        var statusInput = document.getElementById("status")
        var method = ''
        var url = ''

        function Submit() {

            if (namesInput.value === '' || dpiInput.value === '' || bornDateInput.value === '' ||
                sexInput.value === '' || dateEntryCompanyInput.value === '' || addressInput.value === '' ||
                nitInput.value === '' || departmentInput.value === '') {
                alert("Please fill in all fields.")
                return
            }

            if (document.getElementById("action").value === "update" && employeeId.value === '') {
                alert("Please enter the Employee ID.")
                return
            }
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
                    status: statusInput.value,
                    employeeID: employeeId.value,
                }
            }).then(function (response) {
                if (response.data.d == "Insert successful") {
                    alert("Insert successful")
                } else {
                    alert("Insert unsuccessful")
                }
                location.reload()
            }).catch(function (error) {
                console.error(error)
            })
        }

        function getEmployeeData() {
            if (employeeId.value == '') { return }
            axios.post('/Employees.aspx/GetEmployeeData', {
                id: employeeId.value,
            })
                .then(function (response) {
                    console.log(response)
                    if (response.data.d == "Employee not found") {
                        alert(response.data.d)
                        clearFields()
                        return
                    }
                    var responseData = JSON.parse(response.data.d)
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
                    console.error(error)
                })
        }

        function toggleEmployeeId() {
            var actionSelect = document.getElementById("action")
            var employeeIdDiv = document.getElementById("employeeIdDiv")

            if (actionSelect.value === "update") {
                employeeIdDiv.style.display = "block"
            } else {
                employeeIdDiv.style.display = "none"
                employeeIdDiv.value = ''
                clearFields()
            }
        }

        function clearFields() {
            namesInput.value = ''
            dpiInput.value = ''
            bornDateInput.value = ''
            sexInput.value = ''
            dateEntryCompanyInput.value = ''
            addressInput.value = ''
            nitInput.value = ''
            departmentInput.value = ''
        }
    </script>
</body>
</html>