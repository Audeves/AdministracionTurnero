using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.General
{
    public class Empleado : SQLEntityBase
    {
        //prueba
        public string Emplid { get; set; }
        public string Nombre { get; set; }
        public string CuentaDominio { get; set; }
        public string ClaveDptoAdscripcion { get; set; }
        public string DptoAdscripcion { get; set; }
        public string DireccionAcademica { get; set; }
        public string ClaveDireccionAcademica { get; set; }

    }
}
