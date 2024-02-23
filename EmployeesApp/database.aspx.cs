using System;
using System.Data;
using System.Data.SqlClient;

public class UsuarioDAL
{
    private readonly string connectionString;

    public UsuarioDAL()
    {
        connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    }

    public DataTable ObtenerUsuarios()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM usuario", connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }

    public void InsertarUsuario(string nombre, string contraseña, string tipo)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("INSERT INTO usuario (nombre, contraseña, tipo) VALUES (@nombre, @contraseña, @tipo)", connection))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                cmd.ExecuteNonQuery();
            }
        }
    }

}