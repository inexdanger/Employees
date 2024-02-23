using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EmployeesApp
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCatalogo();
            }
        }

        private void CargarCatalogo()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select codigoDepartamento, nombreDepartamento from dbo.ca_departamentoArea;";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable departmentsDataTable = new DataTable();
                adapter.Fill(departmentsDataTable);

                departmentDropDown.DataSource = departmentsDataTable;
                departmentDropDown.DataTextField = "nombreDepartamento";
                departmentDropDown.DataValueField = "codigoDepartamento";
                departmentDropDown.DataBind();

                departmentDropDown.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            }
        }

        protected void departmentDropDownChanged(object sender, EventArgs e)
        {
            MostrarDatos();
        }

        protected void MostrarDatos()
        {
            string departmentCode = departmentDropDown.SelectedValue;

            string connectionString = (ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select * from dbo.employees where CodigoDepartamento= @departmentCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@departmentCode", departmentCode);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable DatosDataTable = new DataTable();
                adapter.Fill(DatosDataTable);

                employeesGrid.DataSource = DatosDataTable;
                employeesGrid.DataBind();
            }
        }
    }
}