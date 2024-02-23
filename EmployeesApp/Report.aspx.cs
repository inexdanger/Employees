using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace EmployeesApp
{
    public partial class Report : System.Web.UI.Page
    {
        //Cadena de conexion para la base de datos
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Cargo el catálogo de departamentos cuando la pagina no se esta cargando debido a una 
            if (!IsPostBack)
            {
                LoadCatalog();
            }
        }

        [WebMethod]
        public static string GenerateReport(string departmentCode)
        {
            //Hago la conexión con la base de datos y ejecuto la consulta para generar el reporte
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string query = "SELECT e.Nombres, e.DPI, e.FechaNacimiento, e.Sexo, e.FechaIngresoEmpresa, e.Edad, e.Direccion, e.NIT, e.CodigoDepartamento, s.Descripcion FROM dbo.employees e JOIN dbo.status s ON s.ID = e.Status WHERE e.CodigoDepartamento = @departmentCode";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@departmentCode", departmentCode);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        //Si encuentro al menos un empleado, construyo el objeto y serializo a JSON
                        if (reader.Read())
                        {
                            var employeeDetails = new
                            {
                                Names = reader["Nombres"].ToString(),
                                DPI = reader["DPI"].ToString(),
                                FechaNacimiento = reader["FechaNacimiento"].ToString(),
                                Sexo = reader["Sexo"].ToString(),
                                FechaIngresoEmpresa = reader["FechaIngresoEmpresa"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                NIT = reader["NIT"].ToString(),
                                CodigoDepartamento = reader["CodigoDepartamento"].ToString()
                            };

                            reader.Close();
                            return new JavaScriptSerializer().Serialize(employeeDetails);
                        }
                        //Si no encuentro ningún empleado, devuelvo un mensaje indicando que no encontre empleados
                        else
                        {
                            reader.Close();
                            return "Employees not found";
                        }
                    }
                    //Manejo las excepciones en caso de algún error durante la ejecución de la consulta SQL
                    catch (Exception ex)
                    {
                        return "Error: " + ex.Message;
                    }
                }
            }
        }

        //Metodo para cargar el catalogo de departamentos
        private void LoadCatalog()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select codigoDepartamento, nombreDepartamento from dbo.ca_departamentoArea;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable departmentsDataTable = new DataTable();
                    adapter.Fill(departmentsDataTable);

                    //Enlazo el catálogo de departamentos al control DropDownList
                    departmentDropDown.DataSource = departmentsDataTable;
                    departmentDropDown.DataTextField = "nombreDepartamento";
                    departmentDropDown.DataValueField = "codigoDepartamento";
                    departmentDropDown.DataBind();

                    //Agrego un ítem por defecto al DropDownList
                    departmentDropDown.Items.Insert(0, new ListItem("Choose an option", "0"));
                }
            }
        }

        //Evento que se ejecuta cuando se cambia la selección en el DropDownList de departamentos
        protected void departmentDropDownChanged(object sender, EventArgs e)
        {
            //Imprimo la información de los empleados del departamento seleccionado
            PrintData();
        }

        //Método para imprimir la información de los empleados del departamento seleccionado
        protected void PrintData()
        {
            string departmentCode = departmentDropDown.SelectedValue;

            //Establezco la conexión con la base de datos y ejecuto la consulta para obtener la información de los empleados
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select e.Nombres, e.DPI, e.FechaNacimiento, e.Sexo, e.FechaIngresoEmpresa, e.Edad, e.Direccion, e.NIT, e.CodigoDepartamento, s.Descripcion from dbo.employees e, dbo.status s where s.ID = e.Status and CodigoDepartamento=@departmentCode";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@departmentCode", departmentCode);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable DataTable = new DataTable();
                    adapter.Fill(DataTable);

                    //Si no encuentro empleados en el departamento seleccionado, agrego una fila con un mensaje indicando que no hay usuarios en el departamento
                    if (DataTable.Rows.Count == 0)
                    {
                        DataRow newRow = DataTable.NewRow();
                        newRow["Nombres"] = "THERE'S NO USERS ON THIS DEPARTMENT";
                        newRow["DPI"] = "THERE'S NO USERS ON THIS DEPARTMENT";
                        newRow["FechaNacimiento"] = DateTime.MinValue;
                        newRow["Sexo"] = "THERE'S NO USERS ON THIS DEPARTMENT";
                        newRow["FechaIngresoEmpresa"] = DateTime.MinValue;
                        newRow["Edad"] = 0;
                        newRow["Direccion"] = "THERE'S NO USERS ON THIS DEPARTMENT";
                        newRow["NIT"] = "THERE'S NO USERS ON THIS DEPARTMENT";
                        newRow["CodigoDepartamento"] = departmentCode;
                        newRow["Descripcion"] = "THERE'S NO USERS ON THIS DEPARTMENT";
                        DataTable.Rows.Add(newRow);
                    }

                    //Enlazo la información de los empleados al control GridView
                    employeesGrid.DataSource = DataTable;
                    employeesGrid.DataBind();
                }
            }
        }
    }
}