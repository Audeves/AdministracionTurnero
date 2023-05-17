using Entidades.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.General
{
    public class CamposRegistroBase : EntityBase
    {

        public string EstatusRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long IdUsuarioRegistro { get; set; }

    }
}
