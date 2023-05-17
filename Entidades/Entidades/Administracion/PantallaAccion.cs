using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Entidades.Entidades.Administracion
{
    public class PantallaAccion : EntityBase
    {
        [ScriptIgnore]
        public Pantalla Pantalla { get; set; }
        public long? IdPantalla { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public bool Activo { get; set; }
        public bool Privado { get; set; }
        public bool PublicaLibre { get; set; }


    }
}
