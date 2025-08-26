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

        public EditarCliente(LinqClassDataContext dataContext, int id)
        {
            InitializeComponent();
            this.dataContext = dataContext;
            this.id = id;
            CargarCliente();
        }

        SqlConnection conexionSql;
        LinqClassDataContext dataContext;
       
        private void btn_aceptar_click(object sender, RoutedEventArgs e)
        {
            //metodo nuevo de insercción con linq
            try
            {
                Cliente cliente = new Cliente();

                cliente = dataContext.Cliente.FirstOrDefault(c => c.Id == id);

                if (cliente == null) {
                    MessageBox.Show("No se encontró el cliente con el ID especificado.");
                    return;
                }
                else
                {
                    cliente.nombre = txt_nombre.Text;
                    cliente.apellido = txt_apellido.Text;
                    cliente.poblacion = txt_poblacion.Text;
                    cliente.telefono = txt_telefono.Text;
                    cliente.direccion = txt_direccion.Text;
                    if (!int.TryParse(txt_cod_cliente.Text, out int codCliente))
                    {
                        MessageBox.Show("El código de cliente debe ser un número entero válido.");
                        return;
                    }
                    cliente.cod_cliente = codCliente;
                    dataContext.SubmitChanges();
                    MessageBox.Show("Cliente Modificado correctamente");
                }

                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Modificar registro de cliente: " + ex.Message);
            }
        }
       
        private void CargarCliente()
        {
            // Cargar los datos del cliente desde la base de datos utilizando LINQ
            try
            {
                var cliente = dataContext.Cliente.FirstOrDefault(c => c.Id == id);

                if (cliente != null)
                {
                    txt_nombre.Text = cliente.nombre;
                    txt_apellido.Text = cliente.nombre;
                    txt_poblacion.Text = cliente.poblacion;
                    txt_telefono.Text = cliente.telefono;
                    txt_direccion.Text = cliente.direccion;
                    txt_cod_cliente.Text = cliente.cod_cliente.ToString();
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente con el ID especificado.");
                }
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
