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
    /// Lógica de interacción para AgregarPedido.xaml
    /// </summary>
    public partial class AgregarPedido : Window
    {
        public AgregarPedido()
        {
            
        }
        public AgregarPedido(SqlConnection conexionSql)
        {
            InitializeComponent();
            this.conexionSql = conexionSql;
            
        }
        SqlConnection conexionSql;

        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Abrir solo si está cerrada
                if (conexionSql.State != ConnectionState.Open)
                {
                    conexionSql.Open();
                }

                string consulta = "INSERT INTO PEDIDO (COD_CLIENTE,FECHA_PEDIDO,FORMA_PAGO) VALUES (@COD_CLIENTE,@FECHA_PEDIDO,@FORMA_PAGO)";
                SqlCommand comando = new SqlCommand(consulta, conexionSql);
                
                //conexionSql.Open();
                
                comando.Parameters.AddWithValue("@COD_CLIENTE", txt_cod_cliente.Text.ToUpper());
                comando.Parameters.AddWithValue("@FECHA_PEDIDO", dp_fecha_pedido.SelectedDate.Value);
                comando.Parameters.AddWithValue("@FORMA_PAGO", cb_forma_pago.Text.ToUpper());
                
                comando.ExecuteNonQuery();

                MessageBoxResult result = MessageBox.Show(
                "Pedido agregado correctamente.\n¿Desea agregar otro pedido?",
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
                    dp_fecha_pedido.SelectedDate = null;
                    cb_forma_pago.SelectedIndex = -1;
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el pedido: " + ex.Message);
            }

        }
    }
}
