using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Citas
{
    public class TramiteCita : EntityBase
    {

        public string Tramite { get; set; }
        public bool Activo { get; set; }

        public string ComentarioAceptar { get; set; }
        public string ComentarioRechazar { get; set; }

    }
}
