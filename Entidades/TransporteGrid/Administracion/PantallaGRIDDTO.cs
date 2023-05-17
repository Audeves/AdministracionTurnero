using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.TransporteGrid.Administracion
{
    public class PantallaGRIDDTO 
    {

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Modulo { get; set; }
        public string SubModulo { get; set; }
        public bool Privado { get; set; }
        public string PrivadoStr { get { return this.Privado ? "Privado" : "Publico"; } }
        public bool Activo { get; set; }
        public string ActivoStr { get { return this.Activo ? "Activo" : "Inactivo"; } }


    }
}
