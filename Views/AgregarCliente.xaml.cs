using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
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
        public AgregarCliente(LinqClassDataContext dataContext)
        {
            // Constructor que recibe un LinqClassDataContext
            InitializeComponent();
            this.dataContext = dataContext;
        }
       
        
        SqlConnection conexionSql;
        LinqClassDataContext dataContext;
        
        
        
        //private void btn_aceptar_click(object sender, RoutedEventArgs e)
        //{
        //    if (conexionSql.State != ConnectionState.Open)
        //        conexionSql.Open();

        //    try
        //    {
        //        string consulta = "INSERT INTO CLIENTE (NOMBRE,APELLIDO,POBLACION,TELEFONO,DIRECCION,COD_CLIENTE) VALUES (@NOMBRE,@APELLIDO,@POBLA,@TEL,@DIRE,@COD)  ";

        //        SqlCommand comando = new SqlCommand(consulta, conexionSql);

        //        //conexionSql.Open();

        //        comando.Parameters.AddWithValue("@NOMBRE", txt_nombre.Text);
        //        comando.Parameters.AddWithValue("@APELLIDO", txt_apellido.Text);
        //        comando.Parameters.AddWithValue("@POBLA", txt_poblacion.Text);
        //        comando.Parameters.AddWithValue("@TEL", txt_telefono.Text);
        //        comando.Parameters.AddWithValue("@DIRE", txt_direccion.Text);
        //        comando.Parameters.AddWithValue("@COD", txt_cod_cliente.Text);

        //        comando.ExecuteNonQuery();

        //        MessageBoxResult result = MessageBox.Show(
        //         "Cliente agregado correctamente.\n¿Desea agregar otro?",
        //          "Confirmación",
        //           MessageBoxButton.YesNo,
        //          MessageBoxImage.Question
        //         );

        //        if (result == MessageBoxResult.No)
        //        {
        //            // Cierro la ventana porque no quiere agregar más
        //            this.Close();
        //        }
        //        else
        //        {
        //            txt_nombre.Clear();
        //            txt_apellido.Clear();
        //            txt_poblacion.Clear();
        //            txt_telefono.Clear();
        //            txt_direccion.Clear();
        //            txt_cod_cliente.Clear();
        //            // Limpio los campos para cargar otro
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Digite los datos del registro que desea agregar!");
        //    }
        //}


        private void btn_aceptar_click(object sender, RoutedEventArgs e)
        {
            //metodo nuevo de insercción con linq

            if (!int.TryParse(txt_cod_cliente.Text, out int codCliente))
            {
                MessageBox.Show("El código de cliente debe ser un número entero válido.");
                return;
            }

            try
            {
                Cliente cliente = new Cliente();

                cliente.nombre = txt_nombre.Text;
                cliente.apellido = txt_apellido.Text;
                cliente.poblacion = txt_poblacion.Text;
                cliente.telefono = txt_telefono.Text;
                cliente.direccion = txt_direccion.Text;
                cliente.cod_cliente = codCliente;
                cliente.Activo = true; // Asignar el valor por defecto de Activo

                dataContext.Cliente.InsertOnSubmit(cliente);

                dataContext.SubmitChanges();
               
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Digite los datos del registro que desea agregar!");
            }
        }

       

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
