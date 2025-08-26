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
using System.Data.Linq;


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
            
            datos =  new LinqClassDataContext(Miconexion);
          

            MostrarClientes();

           

        }
        
        //esto es un objeto de la clase SqlConnection que sirve para conectar a la base de datos con la cadena como parametro
        SqlConnection conexionSql;
        LinqClassDataContext datos;
        
        
        
        private void MostrarClientes()
        {
            //Mostrar los clientes con linq (reduccion de codigo)
            try
            {
                var clientesActivos = datos.Cliente.Where(c => c.Activo).ToList();
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

                var clienteSeleccionado = dataGridClientes.SelectedValue as Cliente;

                if (clienteSeleccionado == null)
                {
                    MessageBox.Show("Seleccione un cliente.");
                    return;
                }

                var pedidos = (from c in datos.Cliente
                               join p in datos.Pedido on c.cod_cliente equals p.cod_cliente
                               where c.Id == clienteSeleccionado.Id 
                               select new
                               {
                                   p.Id,
                                   p.forma_pago,
                                   p.fecha_pedido,
                                   p.cod_cliente,
                                   detalleCompleto = $"Pedido #{p.Id} | Cliente: {p.cod_cliente} | Fecha: {p.fecha_pedido:yyyy-MM-dd} | Pago: {p.forma_pago}"
                               }).ToList();

                dataGridPedidos.ItemsSource = pedidos;
                dataGridPedidos.DisplayMemberPath = "detalleCompleto";
                dataGridPedidos.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar pedidos: " + ex.Message);
            }
        }



        private void Label_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void btn_InsertarCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente nuevaVentana = new AgregarCliente(datos);
            nuevaVentana.ShowDialog();      

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
                
                var clienteSeleccionado = dataGridClientes.SelectedValue as Cliente;

                if (clienteSeleccionado == null)
                {
                    MessageBox.Show("Seleccione el Cliente que desea Desactivar!");
                    return;
                }

                using (var contexto = new LinqClassDataContext(conexionSql)) 
                {
                    var cliente = contexto.Cliente.FirstOrDefault(c => c.Id == clienteSeleccionado.Id);

                    if (cliente != null)
                    {
                        cliente.Activo = false;
                        contexto.SubmitChanges();

                        MessageBox.Show("Cliente Desactivado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el cliente en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al eliminar el cliente.");
            }
        }

        private void btn_borrarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pedidoSeleccionado = dataGridPedidos.SelectedValue as Pedido;

                if (pedidoSeleccionado == null)
                {
                    MessageBox.Show("Seleccione el pedido que desea Desactivar!");
                    return;
                }
                var contexto = new LinqClassDataContext(conexionSql);

                var pedido = contexto.Pedido.FirstOrDefault(p => p.Id == pedidoSeleccionado.Id);
               
                if (pedido != null)
                {
                    pedido.Activo = false;
                    contexto.SubmitChanges();
                    
                    MessageBox.Show("Pedido Desactivado correctamente.");
                    
                    MostrarPedidos();
                }
                else
                {
                    MessageBox.Show("No se encontró el pedido en la base de datos.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Seleccione el pedido que desea eliminar!");
            }
        }



        private void btn_Insertar_pedido_Click(object sender, RoutedEventArgs e)
        {
           AgregarPedido agregarPedido = new AgregarPedido(conexionSql);
            agregarPedido.ShowDialog();

            MostrarPedidos();
        }      
        private void btn_EditarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridClientes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el cliente del que desea editar un pedido!");
                return;
            }

            EditarPedido editarPedido = new EditarPedido(datos,(int)dataGridPedidos.SelectedValue);
            editarPedido.ShowDialog();
        }
        private void btn_EditarCliente_Click(object sender, RoutedEventArgs e)
        {
            EditarCliente editarCliente = new EditarCliente(datos,(int)dataGridClientes.SelectedValue);  
            editarCliente.ShowDialog();

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MostrarClientes();
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

