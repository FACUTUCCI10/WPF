using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primer_wpf.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public DateTime Fecha_pedido { get; set; }

        public string Forma_pago { get; set; }

        public int Cod_cliente { get; set; }
    }
}
