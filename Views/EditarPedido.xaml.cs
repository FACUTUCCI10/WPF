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
    /// Lógica de interacción para EditarPedido.xaml
    /// </summary>
    public partial class EditarPedido : Window
    {
        private int id;

        public EditarPedido()
        {
            InitializeComponent();
        }
        public EditarPedido(SqlConnection conexion,int id)
        {
            conexionSql = conexion;
            InitializeComponent();
            this.id = id;
            CargarPedido();
        }

        private void btn_aceptar_edicion_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                if (conexionSql.State != ConnectionState.Open)
                {
                    conexionSql.Open();
                }

                string consulta = "UPDATE PEDIDO SET COD_CLIENTE = @COD_CLIENTE, FECHA_PEDIDO = @FECHA_PEDIDO, FORMA_PAGO = @FORMA_PAGO  WHERE ID = " + id;
               
                SqlCommand comando = new SqlCommand(consulta, conexionSql);
                 
                conexionSql.Open();
                
                comando.Parameters.AddWithValue("Id",id);
                comando.Parameters.AddWithValue("@COD_CLIENTE", txt_cod_cliente.Text.ToUpper());
                comando.Parameters.AddWithValue("@FECHA_PEDIDO", dp_fecha_pedido_edit.SelectedDate.Value);
                comando.Parameters.AddWithValue("@FORMA_PAGO", cb_forma_pago_edit.Text.ToUpper());

                comando.ExecuteNonQuery();

                MessageBoxResult result = MessageBox.Show(
                "Pedido modificado correctamente.\n¿Desea modificar otro pedido?",
                 "Confirmación",
                  MessageBoxButton.YesNo,
                 MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No)
                {
                    // Cierro la ventana porque no quiere agregar más
                    this.Close();
                }
                else
                {
                    // Limpio los campos para cargar otro
                    txt_cod_cliente.Clear();
                    dp_fecha_pedido_edit.SelectedDate = null;
                    cb_forma_pago_edit.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el pedido: " + ex.Message);
            } 
        }
        private void CargarPedido()
        {
            try
            {
                string consulta = "SELECT COD_CLIENTE, FECHA_PEDIDO, FORMA_PAGO FROM PEDIDO WHERE Id = @Id";
               
                SqlCommand comando = new SqlCommand(consulta, conexionSql);
                
                comando.Parameters.AddWithValue("@Id", id);

                conexionSql.Open();
               
                SqlDataReader lector = comando.ExecuteReader();

                if (lector.Read())
                {
                    txt_cod_cliente.Text = lector["COD_CLIENTE"].ToString();

                    if (lector["FECHA_PEDIDO"] != DBNull.Value)
                        dp_fecha_pedido_edit.SelectedDate = Convert.ToDateTime(lector["FECHA_PEDIDO"]);

                    cb_forma_pago_edit.Text = lector["FORMA_PAGO"].ToString();
                }

                lector.Close();
                conexionSql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el pedido: " + ex.Message);
                conexionSql.Close();
            }
        }


        private void btn_cancelar_edicion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        SqlConnection conexionSql;
    }
}
