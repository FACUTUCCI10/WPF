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
    /// Lógica de interacción para Data.xaml
    /// </summary>
    public partial class Data : Window
    {
        public Data()
        {
            InitializeComponent();
        }
        public Data(SqlConnection conexion)
        {
            InitializeComponent();
            //Conexion a DB
            conexionSql = conexion;
            MostrarClientes();
        }

       


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
    }
}
