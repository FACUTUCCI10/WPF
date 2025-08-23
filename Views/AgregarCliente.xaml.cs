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
    /// Lógica de interacción para AgregarCliente.xaml
    /// </summary>
    public partial class AgregarCliente : Window
    {
        public AgregarCliente()
        {
           
        }
        public AgregarCliente(SqlConnection conexion)
        {
            InitializeComponent();
            conexionSql = conexion;
        }


        private void btn_aceptar_click(object sender, RoutedEventArgs e)
        {
            if (conexionSql.State != ConnectionState.Open)
                conexionSql.Open();

            try
            {
                string consulta = "INSERT INTO CLIENTE (NOMBRE,APELLIDO,POBLACION,TELEFONO,DIRECCION,COD_CLIENTE) VALUES (@NOMBRE,@APELLIDO,@POBLA,@TEL,@DIRE,@COD)  " ;

                SqlCommand comando = new SqlCommand(consulta, conexionSql);

                //conexionSql.Open();
             
                comando.Parameters.AddWithValue("@NOMBRE", txt_nombre.Text);
                comando.Parameters.AddWithValue("@APELLIDO", txt_apellido.Text);
                comando.Parameters.AddWithValue("@POBLA", txt_poblacion.Text);
                comando.Parameters.AddWithValue("@TEL", txt_telefono.Text);
                comando.Parameters.AddWithValue("@DIRE", txt_direccion.Text);
                comando.Parameters.AddWithValue("@COD",txt_cod_cliente.Text);
                
                comando.ExecuteNonQuery();

                MessageBoxResult result = MessageBox.Show(
                 "Cliente agregado correctamente.\n¿Desea agregar otro?",
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
                    txt_nombre.Clear();
                    txt_apellido.Clear();
                    txt_poblacion.Clear();
                    txt_telefono.Clear();
                    txt_direccion.Clear();
                    txt_cod_cliente.Clear();
                    // Limpio los campos para cargar otro
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Digite los datos del registro que desea agregar!");
            }
        }
        SqlConnection conexionSql;


       

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
