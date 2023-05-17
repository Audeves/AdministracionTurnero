using Datos.Acceso.General;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.General
{
    public class InicioDataBase
    {

        public static string Conexion { get; set; }

        public InicioDataBase()
        {
            //Inicializar
            Database.SetInitializer(new CreateDatabaseIfNotExists<ContextoDataBase>());
        }

    }
}
