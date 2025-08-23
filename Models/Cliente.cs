using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primer_wpf.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }
      
        public string Poblacion { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public int Cod_cliente { get; set; }
    }
}
