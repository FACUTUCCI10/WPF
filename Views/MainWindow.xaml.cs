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
            
            dataContext =  new LinqClassDataContext(Miconexion);
          

            MostrarClientes();

           

        }
        
        //esto es un objeto de la clase SqlConnection que sirve para conectar a la base de datos con la cadena como parametro
        SqlConnection conexionSql;
        LinqClassDataContext dataContext;
      
        
        //private void MostrarClientes()
        //{
        //    try
        //    {
        //        string consulta = "SELECT *, CONCAT(NOMBRE, ' ', APELLIDO, ' - ', POBLACION, ' - ', TELEFONO,' - ',DIRECCION,' - ',COD_CLIENTE) AS DescripcionCompleta FROM Cliente where Activo = 1";

        //        SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionSql);

        //        using (adaptador)
        //        {
        //            DataTable ClientesTabla = new DataTable();
        //            adaptador.Fill(ClientesTabla);

        //            dataGridClientes.DisplayMemberPath = "DescripcionCompleta";
        //            dataGridClientes.SelectedValuePath = "Id";
        //            dataGridClientes.ItemsSource = ClientesTabla.DefaultView;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.ToString());
        //    }
        //}

        private void MostrarClientes()
        {
            //Mostrar los clientes con linq (reduccion de codigo)
            try
            {
                var clientesActivos = dataContext.Cliente.Where(c => c.Activo).ToList();
                dataGridClientes.ItemsSource = clientesActivos;

            }
            catch (Exception e)
            {
                MessageBox.Show("Error mostrando clientes: " + e.Message);
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

        //private void btn_InsertarCliente_Click(object sender, RoutedEventArgs e)
        //{
        //    AgregarCliente nuevaVentana = new AgregarCliente(conexionSql);
        //    nuevaVentana.ShowDialog();       // Muestra la nueva ventana  

        //    MostrarClientes();

        //}

        private void btn_InsertarCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente nuevaVentana = new AgregarCliente(dataContext);
            nuevaVentana.ShowDialog();       // Muestra la nueva ventana  

            MostrarClientes();

        }

        private void lista_clientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridClientes.SelectedValue == null||dataGridClientes.SelectedItem==null)
            {
                MessageBox.Show("Seleccione un cliente para ver sus pedidos.");
                return;
            }
            else
            {
                MostrarPedidos();
            }
              
        }


        private void btn_Eliminar_cliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string consulta = "UPDATE Cliente SET ACTIVO = 0 WHERE Id = @clienteid";

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
        }
        //private void btn_Eliminar_cliente_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // Asegurate de que el DataGrid tenga el objeto Cliente como SelectedItem
        //        var clienteSeleccionado = dataGridClientes.SelectedValue as Cliente;

        //        if (clienteSeleccionado == null)
        //        {
        //            MessageBox.Show("Seleccione el Cliente que desea eliminar!");
        //            return;
        //        }

        //        using (var contexto = new TuDbContext()) // Reemplazá con el nombre real de tu DbContext
        //        {
        //            var cliente = contexto.Clientes.FirstOrDefault(c => c.Id == clienteSeleccionado.Id);

        //            if (cliente != null)
        //            {
        //                cliente.Activo = false;
        //                contexto.SaveChanges();

        //                MessageBox.Show("Cliente Desactivado correctamente.");
        //            }
        //            else
        //            {
        //                MessageBox.Show("No se encontró el cliente en la base de datos.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ocurrió un error al eliminar el cliente.");
        //    }
       // }




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
                MessageBox.Show("Seleccione el cliente del que desea editar un pedido!");
                return;
            }

                int pedidoId = (int)dataGridClientes.SelectedValue;

                EditarPedido editarPedido = new EditarPedido(conexionSql, pedidoId);
                editarPedido.ShowDialog(); // Muestra la nueva ventana de editar pedido
            
        }

        private void btn_EditarCliente_Click(object sender, RoutedEventArgs e)
        {
            EditarCliente editarCliente = new EditarCliente(conexionSql, (int)dataGridClientes.SelectedValue);
            editarCliente.ShowDialog();

        }
       

        private void Window_Activated(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void btn_data_Click(object sender, RoutedEventArgs e)
        {
            Data data = new Data(conexionSql);
            data.ShowDialog(); 

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
            ReactivarCliente reactivarCliente = new ReactivarCliente(conexionSql);
            reactivarCliente.ShowDialog(); 
        }

    }
}

