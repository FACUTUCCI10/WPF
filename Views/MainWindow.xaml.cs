using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls.Primitives;


namespace primer_wpf.Views
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Conexion a DB

            string Miconexion = ConfigurationManager.ConnectionStrings["primer_wpf.Properties.Settings.GestionConnectionString"].ConnectionString;

            conexionSql = new SqlConnection(Miconexion);
            AgregarCliente ventana = new AgregarCliente(conexionSql);

            MostrarClientes();
           

        }
        //esto es un objeto de la clase SqlConnection que sirve para conectar a la base de datos con la cadena como parametro
        SqlConnection conexionSql;

        private void MostrarClientes()
        {
            try
            {
                string consulta = "SELECT *, CONCAT(NOMBRE, ' ', APELLIDO, ' - ', POBLACION, ' - ', TELEFONO,' - ',DIRECCION,' - ',COD_CLIENTE) AS DescripcionCompleta FROM Cliente";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionSql);

                using (adaptador)
                {
                    DataTable ClientesTabla = new DataTable();
                    adaptador.Fill(ClientesTabla);

                    dataGridClientes.DisplayMemberPath = "DescripcionCompleta";
                    dataGridClientes.SelectedValuePath = "Id";
                    dataGridClientes.ItemsSource = ClientesTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MostrarPedidos()
        {
            
            try
            {
                //string consulta = "SELECT *, CONCAT ('COD:',cod_cliente,'fECHA DE PEDIDO:',' /',fecha_pedido,'FORMA DE PAGO:',' /', forma_pago) AS INFO from pedido";

                //consulta parametrizada que muestra los pedidos de un cliente seleccionado en la lista de clientes

                string consulta = "SELECT P.Id, P.cod_cliente, P.forma_pago, P.fecha_pedido, " +
                  "CONCAT('Pedido #', P.Id, ' | Cliente: ', P.cod_cliente, ' | Fecha: ', FORMAT(P.fecha_pedido, 'yyyy-MM-dd'), ' | Pago: ', P.forma_pago) AS detalleCompleto " +
                  "FROM PEDIDO P INNER JOIN CLIENTE C ON P.cod_cliente = C.cod_cliente " +
                  "WHERE C.Id = @ClienteId"; ;

                SqlCommand comando = new SqlCommand(consulta, conexionSql);

                SqlDataAdapter adaptador = new SqlDataAdapter(comando);

                using (comando)
                {
                    comando.Parameters.AddWithValue("@ClienteId", dataGridClientes.SelectedValue);
                    DataTable PedidosTabla = new DataTable();

                    adaptador.Fill(PedidosTabla);

                    dataGridPedidos.DisplayMemberPath = "detalleCompleto";
                    dataGridPedidos.SelectedValuePath = "Id";
                    dataGridPedidos.ItemsSource = PedidosTabla.DefaultView;

                }
            }

            catch (Exception e)
            {
                {
                    MessageBox.Show(e.ToString());
                }

            }
        }




        private void Label_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void btn_InsertarCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente nuevaVentana = new AgregarCliente(conexionSql);
            nuevaVentana.ShowDialog();       // Muestra la nueva ventana  

            MostrarClientes();

        }

        private void lista_clientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Cliente seleccionado: " + lista_clientes.SelectedValue.ToString());
            //MostrarPedidos((int)lista_clientes.SelectedValue);
            MostrarPedidos();
        }

        private void btn_Eliminar_cliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
               string consulta = "DELETE FROM Cliente WHERE Id = @clienteid";

                SqlCommand comando = new SqlCommand(consulta, conexionSql);


                conexionSql.Open();

                comando.Parameters.AddWithValue("@clienteid", dataGridClientes.SelectedValue);

                comando.ExecuteNonQuery();

                conexionSql.Close();

                MessageBox.Show("Cliente eliminado correctamente.");
               
            }

            catch (Exception ex)
            {
                MessageBox.Show("Seleccione el Cliente que desea eliminar!");
            }

          //  MostrarClientes();

        }

        //  private void btn_borrarPedido_Click(object sender, RoutedEventArgs e)
        //  {

        //      try
        //      {
        //          if (pedidos_cliente.SelectedValue == null)
        //          {
        //              MessageBox.Show("Seleccione el pedido que desea eliminar!");
        //              return;
        //          }

        //          MessageBoxResult result = MessageBox.Show(
        //          "¿Está seguro que desea eliminar este pedido?",
        //          "Confirmación",
        //           MessageBoxButton.YesNo,
        //           MessageBoxImage.Warning
        //);
        //          if (result == MessageBoxResult.No)
        //          {
        //              return; // Si el usuario no confirma, no se hace nada
        //          }
        //          else
        //          {
        //              string consulta = "DELETE FROM pedido WHERE Id = @Pedidoid";

        //              SqlCommand comando = new SqlCommand(consulta, conexionSql);
        //              conexionSql.Open();

        //              comando.Parameters.AddWithValue("@Pedidoid", pedidos_cliente.SelectedValue);

        //              comando.ExecuteNonQuery();

        //              conexionSql.Close();
        //          }

        //          MessageBox.Show("Pedido eliminado correctamente.");
        //          MostrarPedidos();
        //      }
        //      catch (Exception ex)
        //      {
        //          MessageBox.Show("Seleccione el pedido que desea eliminar!");
        //      }

        //  }


        private void btn_borrarPedido_Click(object sender, RoutedEventArgs e) 
        { 
            try 
            { 
                              
                string consulta = "DELETE FROM pedido WHERE Id = @Pedidoid"; 
                
                SqlCommand comando = new SqlCommand(consulta, conexionSql); 
                
                conexionSql.Open(); 
                
                comando.Parameters.AddWithValue("@Pedidoid", dataGridClientes.SelectedValue);
                
                comando.ExecuteNonQuery(); 
                
                conexionSql.Close(); MostrarPedidos(); 

            } 

            catch (Exception ex) 
            {
                
                
                MessageBox.Show("Seleccione el pedido que desea eliminar!"); 
            } 
        }


        private void btn_Insertar_pedido_Click(object sender, RoutedEventArgs e)
        {
           AgregarPedido agregarPedido = new AgregarPedido(conexionSql);
            agregarPedido.ShowDialog(); // Muestra la nueva ventana de agregar pedido

            MostrarPedidos();
        }

        private void btn_EditarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridClientes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el pedido que desea editar!");
                return;
            }

           
            
                int pedidoId = (int)dataGridClientes.SelectedValue;

                EditarPedido editarPedido = new EditarPedido(conexionSql, pedidoId);
                editarPedido.ShowDialog(); // Muestra la nueva ventana de editar pedido
            
            

            //MostrarPedidos();
        }

        private void btn_EditarCliente_Click(object sender, RoutedEventArgs e)
        {
            EditarCliente editarCliente = new EditarCliente(conexionSql, (int)dataGridClientes.SelectedValue);
            editarCliente.ShowDialog(); // Muestra la nueva ventana de editar cliente

            //MostrarClientes();
        }
       

        private void Window_Activated(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void btn_data_Click(object sender, RoutedEventArgs e)
        {
            Data data = new Data(conexionSql);
            data.ShowDialog(); // Muestra la nueva ventana de datos

        }

        private void dataGridClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGridPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGridClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MostrarPedidos();
        }

        private void btnReactivarCliente_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

