using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace EmployeesApp
{
    public partial class Employees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Genero las listas desplegables
                GenerateDropDownList();
            }
        }

        //Metodo para generar las listas desplegables de departamentos y estados
        protected void GenerateDropDownList()
        {
            //Obtengo el catálogo de departamentos y estados
            DataTable departmentCatalog = GetDepartmentCatalog();
            DataTable statusCatalog = GetStatusCatalog();

            //Establezco los datos de los departamentos en la lista desplegable
            department.DataSource = departmentCatalog;
            department.DataTextField = "nombreDepartamento";
            department.DataValueField = "codigoDepartamento";
            department.DataBind();

            //Establezco los datos de los estados en la lista desplegable
            status.DataSource = statusCatalog;
            status.DataTextField = "Descripcion";
            status.DataValueField = "ID";
            status.DataBind();
        }

        //Metodo para obtener el catalogo de departamentos desde la base de datos
        protected DataTable GetDepartmentCatalog()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT codigoDepartamento, nombreDepartamento FROM dbo.ca_departamentoArea;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable catalogData = new DataTable();
                adapter.Fill(catalogData);
                return catalogData;
            }
        }

        //Metodo para obtener el catalogo de estados desde la base de datos
        protected DataTable GetStatusCatalog()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT ID, Descripcion FROM dbo.status;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable catalogData = new DataTable();
                adapter.Fill(catalogData);
                return catalogData;
            }
        }

        //Matodo para insertar un nuevo empleado en la base de datos
        [WebMethod]
        public static string InsertEmployee(string names, string dpi, string bornDate, string sex, string dateEntryCompany, string address, string nit, string department, string status)
        {
            try
            {
                //Verifico si algun campo requerido esta vacio
                if (!(new[] { names, dpi, bornDate, sex, dateEntryCompany, address, nit, department }).All(field => !string.IsNullOrWhiteSpace(field)))
                {
                    return "Error: One or more required fields are missing.";
                }

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string query = "INSERT INTO dbo.employees (Nombres, DPI, FechaNacimiento, Sexo, FechaIngresoEmpresa, Direccion, NIT, CodigoDepartamento, Status) VALUES (@Names, @Dpi, @BornDate, @Sex, @DateEntryCompany, @Address, @NIT, @Department, @Status)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //Establezco los parametros para la consulta SQL
                        command.Parameters.AddWithValue("@Names", names);
                        command.Parameters.AddWithValue("@Dpi", dpi);
                        command.Parameters.AddWithValue("@BornDate", bornDate);
                        command.Parameters.AddWithValue("@Sex", sex);
                        command.Parameters.AddWithValue("@DateEntryCompany", dateEntryCompany);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@NIT", nit);
                        command.Parameters.AddWithValue("@Department", department);
                        command.Parameters.AddWithValue("@Status", status);

                        //Ejecuto la consulta SQL
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        //Verifico si se insertaron filas en la base de datos
                        if (rowsAffected > 0)
                        {
                            return "Insert successful";
                        }
                        else
                        {
                            return "Insert failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        //Metodo para actualizar la información de un empleado en la base de datos
        [WebMethod]
        public static string UpdateEmployee(string names, string dpi, string bornDate, string sex, string dateEntryCompany, string address, string nit, string department, int employeeID)
        {
            try
            {
                //Verifico si algun campo requerido esta vacio o si el ID del empleado es invalido
                if (!(new[] { names, dpi, bornDate, sex, dateEntryCompany, address, nit, department }).All(field => !string.IsNullOrWhiteSpace(field)) || employeeID <= 0)
                {
                    return "Error: One or more required fields are missing.";
                }

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string query = "UPDATE dbo.employees SET Nombres = @Names, DPI = @Dpi, FechaNacimiento = @BornDate, Sexo = @Sex, FechaIngresoEmpresa = @DateEntryCompany, Direccion = @Address, NIT = @NIT, CodigoDepartamento = @Department WHERE EmployeeID = @EmployeeID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //Establezco los parametros para la consulta SQL
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        command.Parameters.AddWithValue("@Names", names);
                        command.Parameters.AddWithValue("@Dpi", dpi);
                        command.Parameters.AddWithValue("@BornDate", bornDate);
                        command.Parameters.AddWithValue("@Sex", sex);
                        command.Parameters.AddWithValue("@DateEntryCompany", dateEntryCompany);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@NIT", nit);
                        command.Parameters.AddWithValue("@Department", department);

                        //Ejecuto la consulta SQL
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        //Verifico si se actualizaron filas en la base de datos
                        if (rowsAffected > 0)
                        {
                            return new JavaScriptSerializer().Serialize(new { Message = "Insert successful" });
                        }
                        else
                        {
                            return new JavaScriptSerializer().Serialize(new { Error = "Insert failed" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        //Metodo para obtener los datos de un empleado por su ID
        [WebMethod]
        public static string GetEmployeeData(string id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string query = "SELECT Nombres, DPI, CONVERT(VARCHAR(10), FechaNacimiento, 120) AS FechaNacimiento, Sexo, CONVERT(VARCHAR(10), FechaIngresoEmpresa, 120) AS FechaIngresoEmpresa, Direccion, NIT, CodigoDepartamento, EmployeeID FROM dbo.employees WHERE EmployeeID = @EmployeeID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //Establezco el parametro para la consulta SQL
                        command.Parameters.AddWithValue("@EmployeeID", id);

                        //Ejecuto la consulta SQL
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        //Verifico si se encontraron resultados
                        if (reader.Read())
                        {
                            //Obtengo los datos del empleado
                            string names = reader["Nombres"].ToString();
                            string dpi = reader["DPI"].ToString();
                            string bornDate = reader["FechaNacimiento"].ToString();
                            string sex = reader["Sexo"].ToString();
                            string dateEntryCompany = reader["FechaIngresoEmpresa"].ToString();
                            string address = reader["Direccion"].ToString();
                            string nit = reader["NIT"].ToString();
                            string department = reader["CodigoDepartamento"].ToString();
                            string employeeID = reader["EmployeeID"].ToString();

                            //Creo un objeto anónimo con los datos del empleado
                            var employeeDetails = new
                            {
                                EmployeeID = employeeID,
                                Names = names,
                                DPI = dpi,
                                BornDate = bornDate,
                                Sex = sex,
                                DateEntryCompany = dateEntryCompany,
                                Address = address,
                                NIT = nit,
                                Department = department
                            };

                            reader.Close();
                            connection.Close();

                            //Serializo el objeto anónimo a JSON
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            string json = serializer.Serialize(employeeDetails);

                            return json;
                        }
                        else
                        {
                            reader.Close();
                            connection.Close();
                            return "Employee not found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}