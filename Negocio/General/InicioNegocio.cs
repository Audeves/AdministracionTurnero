using Datos.Configuracion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.General
{
    public class InicioNegocio
    {

        public InicioNegocio(string conexion)
        {
            InicioDataBase.Conexion = conexion;
        }

    }
}
