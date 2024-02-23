using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;


namespace EmployeesApp
{

    public partial class Employees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCatalogDataToDropDownList();
            }
        }

        protected void BindCatalogDataToDropDownList()
        {
            DataTable catalogData = GetCatalogDataFromDatabase();

            department.DataSource = catalogData;
            department.DataTextField = "nombreDepartamento";
            department.DataValueField = "codigoDepartamento";
            department.DataBind();
        }

        protected DataTable GetCatalogDataFromDatabase()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string query = "select codigoDepartamento, nombreDepartamento from dbo.ca_departamentoArea;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable catalogData = new DataTable();
                adapter.Fill(catalogData);
                return catalogData;
            }
        }

        [WebMethod]
        public static string InsertEmployee(string names, string dpi, string bornDate, string sex, string dateEntryCompany, string address, string nit, string department)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string query = "INSERT INTO dbo.employees (Nombres, DPI, FechaNacimiento, Sexo, FechaIngresoEmpresa, Direccion, NIT, CodigoDepartamento) VALUES (@Names, @Dpi, @BornDate, @Sex, @DateEntryCompany, @Address, @NIT, @Department)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Names", names);
                        command.Parameters.AddWithValue("@Dpi", dpi);
                        command.Parameters.AddWithValue("@BornDate", bornDate);
                        command.Parameters.AddWithValue("@Sex", sex);
                        command.Parameters.AddWithValue("@DateEntryCompany", dateEntryCompany);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@NIT", nit);
                        command.Parameters.AddWithValue("@Department", department);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

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

        [WebMethod]
        public static string UpdateEmployee(string names, string dpi, string bornDate, string sex, string dateEntryCompany, string address, string nit, string department, int employeeID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string query = "UPDATE dbo.employees SET Nombres = @Names, DPI = @Dpi, FechaNacimiento = @BornDate, Sexo = @Sex, FechaIngresoEmpresa = @DateEntryCompany, Direccion = @Address, NIT = @NIT, CodigoDepartamento = @Department WHERE EmployeeID = @EmployeeID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        command.Parameters.AddWithValue("@Names", names);
                        command.Parameters.AddWithValue("@Dpi", dpi);
                        command.Parameters.AddWithValue("@BornDate", bornDate);
                        command.Parameters.AddWithValue("@Sex", sex);
                        command.Parameters.AddWithValue("@DateEntryCompany", dateEntryCompany);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@NIT", nit);
                        command.Parameters.AddWithValue("@Department", department);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected > 0)
                        {
                            return "Update successful";
                        }
                        else
                        {
                            return "Update failed: No rows affected";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod]
        public static string getEmployeeData(string employeeID)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string query = "SELECT Nombres, DPI, CONVERT(VARCHAR(10), FechaNacimiento, 120) AS FechaNacimiento, Sexo, CONVERT(VARCHAR(10), FechaIngresoEmpresa, 120) AS FechaIngresoEmpresa, Direccion, NIT, CodigoDepartamento FROM dbo.employees WHERE EmployeeID = @EmployeeID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string names = reader["Nombres"].ToString();
                            string dpi = reader["DPI"].ToString();
                            string bornDate = reader["FechaNacimiento"].ToString();
                            string sex = reader["Sexo"].ToString();
                            string dateEntryCompany = reader["FechaIngresoEmpresa"].ToString();
                            string address = reader["Direccion"].ToString();
                            string nit = reader["NIT"].ToString();
                            string department = reader["CodigoDepartamento"].ToString();

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