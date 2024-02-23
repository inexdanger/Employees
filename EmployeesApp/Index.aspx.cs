using System;

namespace EmployeesApp
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Lógica del evento Page_Load para la página Index
        }
        protected void btnSaludar_Click(object sender, EventArgs e)
        {
            // Este método se ejecutará cuando se haga clic en el botón
            lblMensaje.Text = "¡Hola Mundo!";
        }
    }

}