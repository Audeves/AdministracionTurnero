using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Administracion
{
    public class SubModulo : EntityBase
    {

        public string Nombre { get; set; }
        public Modulo Modulo { get; set; }
        public long IdModulo { get; set; }
        public bool Activo { get; set; }
        public string ClaseIcono { get; set; }


    }
}
