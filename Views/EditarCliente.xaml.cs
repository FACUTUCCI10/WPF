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
    /// Lógica de interacción para EditarCliente.xaml
    /// </summary>
    public partial class EditarCliente : Window
    {
        private int id;
        public EditarCliente()
        {
           
        }

        public EditarCliente(SqlConnection conexion, int id)
        {
            InitializeComponent();
            conexionSql = conexion;
            this.id = id;
            CargarCliente();
        }

        SqlConnection conexionSql;

        private void btn_aceptar_click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Abrir solo si está cerrada
                if (conexionSql.State != ConnectionState.Open)
                {
                    conexionSql.Open();
                }

                string consulta = "UPDATE CLIENTE SET NOMBRE = @NOMBRE, APELLIDO = @APELLIDO, POBLACION = @POBLACION, TELEFONO = @TELEFONO, DIRECCION = @DIRECCION, COD_CLIENTE = @COD_CLIENTE WHERE ID = " + id;

                SqlCommand comando = new SqlCommand(consulta, conexionSql);

               // conexionSql.Open();

                comando.Parameters.AddWithValue("@NOMBRE", txt_nombre.Text);
                comando.Parameters.AddWithValue("@APELLIDO", txt_apellido.Text);
                comando.Parameters.AddWithValue("@POBLACION", txt_poblacion.Text);
                comando.Parameters.AddWithValue("@TELEFONO", txt_telefono.Text);
                comando.Parameters.AddWithValue("@DIRECCION", txt_direccion.Text);
                comando.Parameters.AddWithValue("@COD_CLIENTE", txt_cod_cliente.Text);


                comando.ExecuteNonQuery();

                conexionSql.Close();


                MessageBox.Show("Cliente Modificado correctamente");

                this.Close();

                //txt_nombre.Clear();
                //txt_apellido.Clear();
                //txt_poblacion.Clear();
                //txt_telefono.Clear();
                //txt_direccion.Clear();
                //txt_cod_cliente.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Modificar registro de cliente: " + ex.Message);
            }
        }

        private void CargarCliente()
        {
            try
            {
                string consulta = "SELECT * FROM CLIENTE WHERE ID = @Id";
                
                SqlCommand comando = new SqlCommand(consulta, conexionSql);
               
                comando.Parameters.AddWithValue("@Id", id);
                
                conexionSql.Open();
               
                SqlDataReader reader = comando.ExecuteReader();
                
                if (reader.Read())
                {
                    txt_nombre.Text = reader["NOMBRE"].ToString();
                    txt_apellido.Text = reader["APELLIDO"].ToString();
                    txt_poblacion.Text = reader["POBLACION"].ToString();
                    txt_telefono.Text = reader["TELEFONO"].ToString();
                    txt_direccion.Text = reader["DIRECCION"].ToString();
                    txt_cod_cliente.Text = reader["COD_CLIENTE"].ToString();
                }
               
                conexionSql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el cliente: " + ex.Message);
            }
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
