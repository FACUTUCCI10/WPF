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

        public EditarPedido(LinqClassDataContext dataContext,int id)
        {
            this.dataContext = dataContext;
            InitializeComponent();
            this.id = id;
            CargarPedido();
        }

        SqlConnection conexionSql;
        LinqClassDataContext dataContext;

       
        private void btn_aceptar_edicion_Click(object sender, RoutedEventArgs e)
        {
            //metodo nuevo de insercción con linq
            try
            {

               Pedido pedido = new Pedido();
               
                if (!int.TryParse(txt_cod_cliente.Text, out int codCliente))
                {
                    MessageBox.Show("El código de cliente debe ser un número entero válido.");
                    return;
                }

                pedido.cod_cliente = codCliente;
                pedido.forma_pago = cb_forma_pago_edit.Text;
                pedido.fecha_pedido = dp_fecha_pedido_edit.SelectedDate.Value;

                dataContext.Pedido.InsertOnSubmit(pedido);

                dataContext.SubmitChanges();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el pedido: " + ex.Message);
            }
        }
        private void CargarPedido()
        {
            //metodo actualizado con linq
            try
            {
                var pedido = dataContext.Pedido.FirstOrDefault(p => p.Id == id);

                if (pedido != null)
                {
                    txt_cod_cliente.Text = pedido.cod_cliente.ToString();

                    if (pedido.fecha_pedido != null)
                        dp_fecha_pedido_edit.SelectedDate = pedido.fecha_pedido;

                    cb_forma_pago_edit.Text = pedido.forma_pago;
                }
                else
                {
                    MessageBox.Show("No se encontró el pedido con el ID especificado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el pedido: " + ex.Message);
            }


        }

        private void btn_cancelar_edicion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
