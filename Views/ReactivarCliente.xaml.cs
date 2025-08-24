using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace primer_wpf.Views
{
    /// <summary>
    /// Lógica de interacción para ReactivarCliente.xaml
    /// </summary>
    public partial class ReactivarCliente : Window
    {
        public ReactivarCliente()
        {
            
        }
        public ReactivarCliente(SqlConnection conexion)
        {
            InitializeComponent();
            conexionSql = conexion;
            MostrarClientesInactivos();
        }

        private void btnReactivar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "UPDATE Cliente SET Activo = 1 WHERE Id = @id";
                
                SqlCommand comando = new SqlCommand(consulta, conexionSql);
               
                comando.Parameters.AddWithValue("@id", dataGridClientesInactivos.SelectedValue);
                
                conexionSql.Open();
                
                comando.ExecuteNonQuery();
                
                conexionSql.Close();
               
                
                MessageBox.Show("Cliente reactivado correctamente.");
                
                this.Close();                
               
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error al reactivar el cliente: " + ex.Message);
            }
        }

        private void MostrarClientesInactivos()
        {
            try
            {
                string consulta = "SELECT *, CONCAT(NOMBRE, ' ', APELLIDO, ' - ', POBLACION, ' - ', TELEFONO,' - ',DIRECCION,' - ',COD_CLIENTE) AS DescripcionCompleta FROM Cliente where Activo = 0";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionSql);

                using (adaptador)
                {
                    DataTable ClientesTabla = new DataTable();
                    adaptador.Fill(ClientesTabla);

                    dataGridClientesInactivos.DisplayMemberPath = "DescripcionCompleta";
                    dataGridClientesInactivos.SelectedValuePath = "Id";
                    dataGridClientesInactivos.ItemsSource = ClientesTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        SqlConnection conexionSql;

        private void Window_Activated(object sender, EventArgs e)
        {
            MostrarClientesInactivos();
        }
    }
}
