using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Administracion
{
    public class ExcepcionControladorAccion : EntityBase
    {

        public ExcepcionControladorAccion(string controlador, string accion)
        {
            this.Controlador = controlador;
            this.Accion = accion;
        }

        public ExcepcionControladorAccion()
        {

        }

        public string Controlador { get; set; }
        public string Accion { get; set; }
        public bool EsPantalla { get; set; }
        public bool EsAccion { get; set; }
        public bool PublicaLibre { get; set; }

    }
}
