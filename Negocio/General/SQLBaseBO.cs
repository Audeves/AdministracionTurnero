using Datos.Acceso.General;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.General
{
    /// <summary>
    /// Clase base de negocios para toda la funcionalidad que sea a traves de SQL directamente (Store Procedures, Views, Queries, etc)
    /// Los metodos heredados de la clase BaseBO<> NO DEBEN SER UTILIZADOS, porque las entidades no son parte del modelo 
    /// de base de datos y puede ocasionar problemas al intentar acceder a esa funcionalidad
    /// </summary>
    public class SQLBaseBO : BaseBO<SQLEntityBase>
    {

        public SQLBaseBO(ContextoDataBase db) : base(db)
        {

        }

    }
}
