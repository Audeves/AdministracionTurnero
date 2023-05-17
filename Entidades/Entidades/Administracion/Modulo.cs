using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Administracion
{
    public class Modulo : EntityBase
    {
        public Modulo()
        {
            SubModulos = new List<SubModulo>();
        }

        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<SubModulo> SubModulos { get; set; }

    }
}
