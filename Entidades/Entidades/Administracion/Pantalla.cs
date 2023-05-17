using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Administracion
{
    public class Pantalla : EntityBase
    {

        public Pantalla()
        {
            PantallasAcciones = new List<PantallaAccion>();
        }

        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public Modulo Modulo { get; set; }
        public long? IdModulo { get; set; }
        public SubModulo SubModulo { get; set; }
        public long? IdSubModulo { get; set; }
        public bool Activo { get; set; }
        public List<PantallaAccion> PantallasAcciones { get; set; }
        public string ClaseIcono { get; set; }
        public bool Privado { get; set; }
        public bool Desarrollador { get; set; }
        public bool PublicaLibre { get; set; }



    }
}
